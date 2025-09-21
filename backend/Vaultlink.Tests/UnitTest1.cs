using Vaultlink.Core.Services;
using System.Text;

namespace Vaultlink.Tests;

public class PgpServiceTests : IDisposable
{
    private readonly string _testKeystoreDir;

    public PgpServiceTests()
    {
        _testKeystoreDir = Path.Combine(Path.GetTempPath(), "vaultlink-test-keys", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testKeystoreDir);
    }

    [Fact]
    public async Task GenerateKeyPair_ShouldCreateValidKeyPair()
    {
        // Arrange
        var pgpService = new PgpService(_testKeystoreDir);
        var name = "Test User";
        var email = "test@example.com";
        var passphrase = "testpassphrase123";

        // Act
        var result = await pgpService.GenerateKeyPairAsync(name, email, passphrase);

        // Assert
        Assert.NotNull(result.publicKeyArmored);
        Assert.NotNull(result.keyId);
        Assert.Contains("BEGIN PGP PUBLIC KEY BLOCK", result.publicKeyArmored);
        Assert.Contains("END PGP PUBLIC KEY BLOCK", result.publicKeyArmored);
        Assert.Equal(16, result.keyId.Length); // Key ID should be 16 hex characters
    }

    [Fact]
    public async Task EncryptDecrypt_RoundTrip_ShouldWork()
    {
        // Arrange
        var pgpService = new PgpService(_testKeystoreDir);
        var name = "Test User";
        var email = "test@example.com";
        var passphrase = "testpassphrase123";
        var testData = Encoding.UTF8.GetBytes("Hello, PGP World!");
        var filename = "test.txt";

        // Generate key pair
        var keyResult = await pgpService.GenerateKeyPairAsync(name, email, passphrase);

        // Act - Encrypt
        var encryptedData = await pgpService.EncryptAsync(testData, filename, keyResult.keyId, armor: false);
        
        // Act - Decrypt
        var decryptResult = await pgpService.DecryptAsync(encryptedData, passphrase);

        // Assert
        Assert.Equal(testData, decryptResult.PlaintextData);
        Assert.Equal(filename, decryptResult.Filename);
    }

    [Fact]
    public async Task GetKeys_AfterGenerating_ShouldReturnKey()
    {
        // Arrange
        var pgpService = new PgpService(_testKeystoreDir);
        var name = "Test User";
        var email = "test@example.com";
        var passphrase = "testpassphrase123";

        // Act
        var keyResult = await pgpService.GenerateKeyPairAsync(name, email, passphrase);
        var keys = await pgpService.GetKeysAsync();

        // Assert
        Assert.Single(keys);
        var key = keys.First();
        Assert.Equal(keyResult.keyId, key.Id);
        Assert.Contains(email, key.Uid);
        Assert.True(key.IsSecret);
        Assert.True(key.CanSign);
        Assert.True(key.CanEncrypt);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testKeystoreDir))
        {
            Directory.Delete(_testKeystoreDir, recursive: true);
        }
    }
}
