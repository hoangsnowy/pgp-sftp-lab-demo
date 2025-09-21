using Vaultlink.Core.Services;
using Vaultlink.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register services
var keystoreDir = Environment.GetEnvironmentVariable("KEYSTORE_DIR") ?? "./.keys";
builder.Services.AddSingleton(new PgpService(keystoreDir));
builder.Services.AddSingleton<SftpService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// PGP endpoints
app.MapPost("/pgp/generate", async (PgpService pgpService, GenerateKeyRequest request) =>
{
    try
    {
        var result = await pgpService.GenerateKeyPairAsync(request.Name, request.Email, request.Passphrase);
        return Results.Ok(new { PublicKey = result.publicKeyArmored, KeyId = result.keyId });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to generate PGP key pair: {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.MapPost("/pgp/import-public", async (PgpService pgpService, IFormFile file) =>
{
    try
    {
        using var stream = file.OpenReadStream();
        await pgpService.ImportPublicKeyAsync(stream);
        return Results.Ok(new { Message = "Public key imported successfully" });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to import public key: {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.MapPost("/pgp/import-private", async (PgpService pgpService, IFormFile file, string passphrase) =>
{
    try
    {
        using var stream = file.OpenReadStream();
        await pgpService.ImportPrivateKeyAsync(stream, passphrase);
        return Results.Ok(new { Message = "Private key imported successfully" });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to import private key: {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.MapGet("/pgp/keys", async (PgpService pgpService) =>
{
    try
    {
        var keys = await pgpService.GetKeysAsync();
        return Results.Ok(keys);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to get PGP keys: {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.MapPost("/pgp/encrypt", async (PgpService pgpService, IFormFile file, string recipientKeyId, bool armor = false, string? signKeyId = null, string? passphrase = null) =>
{
    try
    {
        using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var data = memoryStream.ToArray();

        var encryptedData = await pgpService.EncryptAsync(data, file.FileName, recipientKeyId, armor, signKeyId, passphrase);

        var fileName = armor ? $"{file.FileName}.asc" : $"{file.FileName}.pgp";
        var contentType = armor ? "text/plain" : "application/octet-stream";

        Console.WriteLine($"Encrypted file {file.FileName} for recipient {recipientKeyId}");
        
        return Results.File(encryptedData, contentType, fileName);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to encrypt file: {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.MapPost("/pgp/decrypt", async (PgpService pgpService, IFormFile file, string passphrase) =>
{
    try
    {
        using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var encryptedData = memoryStream.ToArray();

        var result = await pgpService.DecryptAsync(encryptedData, passphrase);

        Console.WriteLine($"Decrypted file {result.Filename}, signature valid: {result.SignatureValid}");
        
        return Results.File(result.PlaintextData, "application/octet-stream", result.Filename);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to decrypt file: {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

// SSH/SFTP endpoints
app.MapPost("/ssh/test", async (SftpService sftpService, SshTestRequest request) =>
{
    try
    {
        var result = await sftpService.TestSshConnectionAsync(request);
        return Results.Ok(new { Success = result });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SSH test failed for {request.Host}:{request.Port} - {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.MapPost("/sftp/upload", async (SftpService sftpService, IFormFile file, string host, int port, string user, string? privateKeyPath, string remotePath) =>
{
    try
    {
        var request = new SftpUploadRequest
        {
            Host = host,
            Port = port,
            User = user,
            PrivateKeyPath = privateKeyPath,
            RemotePath = remotePath
        };

        using var stream = file.OpenReadStream();
        var result = await sftpService.UploadFileAsync(request, stream, file.FileName);
        
        if (result)
        {
            Console.WriteLine($"Uploaded file {file.FileName} to {host}:{remotePath}");
            return Results.Ok(new { Success = true, Message = "File uploaded successfully" });
        }
        else
        {
            return Results.BadRequest(new { Error = "Upload failed" });
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SFTP upload failed for {host}:{port} - {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.MapGet("/sftp/list", async (SftpService sftpService, string host, int port, string user, string? privateKeyPath, string remotePath) =>
{
    try
    {
        var request = new SftpUploadRequest
        {
            Host = host,
            Port = port,
            User = user,
            PrivateKeyPath = privateKeyPath,
            RemotePath = remotePath
        };

        var files = await sftpService.ListFilesAsync(request, remotePath);
        return Results.Ok(files);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SFTP list failed for {host}:{port} - {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

app.Run();

// Request models
public record GenerateKeyRequest(string Name, string Email, string Passphrase);
