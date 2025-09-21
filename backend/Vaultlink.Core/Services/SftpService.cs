using Renci.SshNet;
using Vaultlink.Core.Models;

namespace Vaultlink.Core.Services;

public class SftpService
{
    private ConnectionInfo CreateConnectionInfo(string host, int port, string user, string? privateKeyPem = null, string? privateKeyPath = null, string? password = null)
    {
        if (!string.IsNullOrEmpty(privateKeyPem))
        {
            using var keyStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(privateKeyPem));
            var privateKeyFile = new PrivateKeyFile(keyStream);
            return new ConnectionInfo(host, port, user, new PrivateKeyAuthenticationMethod(user, privateKeyFile));
        }
        else if (!string.IsNullOrEmpty(privateKeyPath) && File.Exists(privateKeyPath))
        {
            var privateKeyFile = new PrivateKeyFile(privateKeyPath);
            return new ConnectionInfo(host, port, user, new PrivateKeyAuthenticationMethod(user, privateKeyFile));
        }
        else if (!string.IsNullOrEmpty(password))
        {
            return new ConnectionInfo(host, port, user, new PasswordAuthenticationMethod(user, password));
        }
        else
        {
            throw new ArgumentException("Either PrivateKeyPem, PrivateKeyPath, or Password must be provided");
        }
    }

    public Task<bool> TestSshConnectionAsync(SshTestRequest request)
    {
        try
        {
            var connectionInfo = CreateConnectionInfo(request.Host, request.Port, request.User, request.PrivateKeyPem, request.PrivateKeyPath, request.Password);

            using var client = new SshClient(connectionInfo);
            client.Connect();
            var isConnected = client.IsConnected;
            client.Disconnect();

            return Task.FromResult(isConnected);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SSH connection test failed: {ex.Message}");
            return Task.FromResult(false);
        }
    }

    public Task<bool> UploadFileAsync(SftpUploadRequest request, Stream fileStream, string fileName)
    {
        try
        {
            var connectionInfo = CreateConnectionInfo(request.Host, request.Port, request.User, request.PrivateKeyPem, request.PrivateKeyPath, request.Password);

            using var client = new SftpClient(connectionInfo);
            client.Connect();

            var remotePath = Path.Combine(request.RemotePath, fileName).Replace('\\', '/');
            
            // Ensure directory exists
            var remoteDirectory = Path.GetDirectoryName(remotePath)?.Replace('\\', '/');
            if (!string.IsNullOrEmpty(remoteDirectory) && !client.Exists(remoteDirectory))
            {
                client.CreateDirectory(remoteDirectory);
            }

            client.UploadFile(fileStream, remotePath, true);
            client.Disconnect();

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SFTP upload failed: {ex.Message}");
            return Task.FromResult(false);
        }
    }

    public Task<Stream?> DownloadFileAsync(SftpUploadRequest request, string remoteFilePath)
    {
        try
        {
            var connectionInfo = CreateConnectionInfo(request.Host, request.Port, request.User, request.PrivateKeyPem, request.PrivateKeyPath, request.Password);

            using var client = new SftpClient(connectionInfo);
            client.Connect();

            if (!client.Exists(remoteFilePath))
            {
                client.Disconnect();
                return Task.FromResult<Stream?>(null);
            }

            var downloadStream = new MemoryStream();
            client.DownloadFile(remoteFilePath, downloadStream);
            downloadStream.Position = 0;
            
            client.Disconnect();

            return Task.FromResult<Stream?>(downloadStream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SFTP download failed: {ex.Message}");
            return Task.FromResult<Stream?>(null);
        }
    }

    public Task<List<string>> ListFilesAsync(SftpUploadRequest request, string remotePath)
    {
        try
        {
            var connectionInfo = CreateConnectionInfo(request.Host, request.Port, request.User, request.PrivateKeyPem, request.PrivateKeyPath, request.Password);

            using var client = new SftpClient(connectionInfo);
            client.Connect();

            if (!client.Exists(remotePath))
            {
                client.Disconnect();
                return Task.FromResult(new List<string>());
            }

            var files = client.ListDirectory(remotePath)
                .Where(f => f.IsRegularFile)
                .Select(f => f.Name)
                .ToList();

            client.Disconnect();

            return Task.FromResult(files);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SFTP list files failed: {ex.Message}");
            return Task.FromResult(new List<string>());
        }
    }
}