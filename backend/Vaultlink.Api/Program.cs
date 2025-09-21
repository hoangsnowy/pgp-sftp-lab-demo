using Vaultlink.Core.Services;
using Vaultlink.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery();

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
app.UseAntiforgery();

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
}).DisableAntiforgery();

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
}).DisableAntiforgery();

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

app.MapPost("/pgp/encrypt", async (PgpService pgpService, HttpRequest request) =>
{
    try
    {
        var form = await request.ReadFormAsync();
        var file = form.Files["file"];
        var recipientKeyId = form["recipientKeyId"].ToString();
        var armor = bool.Parse(form["armor"].ToString() ?? "false");
        var signKeyId = form["signKeyId"].ToString();
        var passphrase = form["passphrase"].ToString();

        if (file == null || string.IsNullOrEmpty(recipientKeyId))
        {
            return Results.BadRequest(new { Error = "File and recipient key ID are required" });
        }

        using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var data = memoryStream.ToArray();

        var encryptedData = await pgpService.EncryptAsync(data, file.FileName, recipientKeyId, armor, 
            string.IsNullOrEmpty(signKeyId) ? null : signKeyId, 
            string.IsNullOrEmpty(passphrase) ? null : passphrase);

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
}).DisableAntiforgery();

app.MapPost("/pgp/decrypt", async (PgpService pgpService, HttpRequest request) =>
{
    try
    {
        var form = await request.ReadFormAsync();
        var file = form.Files["file"];
        var passphrase = form["passphrase"].ToString();

        if (file == null || string.IsNullOrEmpty(passphrase))
        {
            return Results.BadRequest(new { Error = "File and passphrase are required" });
        }

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
}).DisableAntiforgery();

// SSH/SFTP endpoints
app.MapPost("/ssh/test", async (SftpService sftpService, SshTestRequest request) =>
{
    try
    {
        var result = await sftpService.TestSshConnectionAsync(request);
        if (result)
        {
            return Results.Ok(new { success = true, message = "SSH connection successful" });
        }
        else
        {
            return Results.BadRequest(new { success = false, error = "SSH connection failed" });
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SSH test failed for {request.Host}:{request.Port} - {ex.Message}");
        return Results.BadRequest(new { success = false, error = ex.Message });
    }
});

app.MapPost("/sftp/upload", async (HttpContext context, SftpService sftpService) =>
{
    try
    {
        var form = await context.Request.ReadFormAsync();
        var file = form.Files["file"];
        
        if (file == null || file.Length == 0)
        {
            return Results.BadRequest(new { Error = "No file provided" });
        }

        var request = new SftpUploadRequest
        {
            Host = form["host"],
            Port = int.Parse(form["port"]),
            User = form["user"],
            PrivateKeyPath = form["privateKeyPath"],
            Password = form["password"], // Add password support
            RemotePath = form["remotePath"]
        };

        using var stream = file.OpenReadStream();
        var result = await sftpService.UploadFileAsync(request, stream, file.FileName);
        
        if (result)
        {
            Console.WriteLine($"Uploaded file {file.FileName} to {request.Host}:{request.RemotePath}");
            return Results.Ok(new { Success = true, Message = "File uploaded successfully" });
        }
        else
        {
            return Results.BadRequest(new { Error = "Upload failed" });
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SFTP upload failed - {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
}).DisableAntiforgery();

app.MapPost("/sftp/list", async (SftpService sftpService, SftpListRequest request) =>
{
    try
    {
        var uploadRequest = new SftpUploadRequest
        {
            Host = request.Host,
            Port = request.Port,
            User = request.User,
            PrivateKeyPath = request.PrivateKeyPath,
            Password = request.Password,
            RemotePath = request.RemotePath
        };

        var files = await sftpService.ListFileDetailsAsync(request, request.RemotePath ?? "/");
        return Results.Ok(files);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SFTP list failed for {request.Host}:{request.Port} - {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

// SFTP download endpoint
app.MapPost("/sftp/download", async (HttpRequest request, SftpService sftpService) =>
{
    try
    {
        var form = await request.ReadFormAsync();
        var hostPorts = form["host"].ToString().Split(':');
        var host = hostPorts[0];
        var port = hostPorts.Length > 1 ? int.Parse(hostPorts[1]) : 22;
        var username = form["username"];
        var password = form["password"];
        var remotePath = form["remotePath"];
        var fileName = form["fileName"];

        var downloadRequest = new SftpUploadRequest
        {
            Host = host,
            Port = port,
            User = username,
            Password = password
        };

        var fullRemotePath = remotePath.ToString().TrimEnd('/') + "/" + fileName;
        Console.WriteLine($"Downloading file from: {fullRemotePath}");

        var fileStream = await sftpService.DownloadFileAsync(downloadRequest, fullRemotePath);
        
        if (fileStream == null)
        {
            return Results.NotFound(new { Error = "File not found" });
        }

        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        fileStream.Dispose();
        
        return Results.File(memoryStream.ToArray(), "application/octet-stream", fileName);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SFTP download failed: {ex.Message}");
        return Results.BadRequest(new { Error = ex.Message });
    }
});

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

app.Run();

// Request models
public record GenerateKeyRequest(string Name, string Email, string Passphrase);
