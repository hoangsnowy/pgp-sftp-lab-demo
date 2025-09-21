namespace Vaultlink.Core.Models;

public class PgpKeyInfo
{
    public string Id { get; set; } = string.Empty;
    public string Uid { get; set; } = string.Empty;
    public string Fingerprint { get; set; } = string.Empty;
    public bool CanSign { get; set; }
    public bool CanEncrypt { get; set; }
    public DateTime CreationTime { get; set; }
    public bool IsSecret { get; set; }
}

public class EncryptRequest
{
    public string RecipientKeyId { get; set; } = string.Empty;
    public bool Armor { get; set; } = true;
    public string? SignKeyId { get; set; }
    public string? Passphrase { get; set; }
}

public class DecryptRequest
{
    public string Passphrase { get; set; } = string.Empty;
}

public class DecryptResult
{
    public byte[] PlaintextData { get; set; } = Array.Empty<byte>();
    public string Filename { get; set; } = string.Empty;
    public bool SignatureValid { get; set; }
    public string? SignerKeyId { get; set; }
}

public class SshTestRequest
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 22;
    public string User { get; set; } = string.Empty;
    public string? PrivateKeyPem { get; set; }
    public string? PrivateKeyPath { get; set; }
    public string? Password { get; set; }
}

public class SftpUploadRequest
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 22;
    public string User { get; set; } = string.Empty;
    public string? PrivateKeyPem { get; set; }
    public string? PrivateKeyPath { get; set; }
    public string? Password { get; set; }
    public string RemotePath { get; set; } = string.Empty;
}

public class SftpListRequest
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 22;
    public string User { get; set; } = string.Empty;
    public string? PrivateKeyPem { get; set; }
    public string? PrivateKeyPath { get; set; }
    public string? RemotePath { get; set; } = "/";
}