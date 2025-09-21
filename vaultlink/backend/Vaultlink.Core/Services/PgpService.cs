using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Vaultlink.Core.Models;

namespace Vaultlink.Core.Services;

public class PgpService
{
    private readonly string _keystoreDir;

    public PgpService(string keystoreDir)
    {
        _keystoreDir = keystoreDir;
        Directory.CreateDirectory(_keystoreDir);
    }

    public Task<(string publicKeyArmored, string keyId)> GenerateKeyPairAsync(string name, string email, string passphrase)
    {
        var keyPair = GenerateRsaKeyPair(4096);
        var keyRingGenerator = new PgpKeyRingGenerator(
            PgpSignature.PositiveCertification,
            keyPair,
            $"{name} <{email}>",
            SymmetricKeyAlgorithmTag.Aes256,
            passphrase.ToCharArray(),
            true,
            null,
            null,
            new SecureRandom());

        var publicKeyRing = keyRingGenerator.GeneratePublicKeyRing();
        var secretKeyRing = keyRingGenerator.GenerateSecretKeyRing();

        var keyId = publicKeyRing.GetPublicKey().KeyId.ToString("X16");

        // Export public key
        using var publicOut = new MemoryStream();
        using var armoredOut = new ArmoredOutputStream(publicOut);
        publicKeyRing.Encode(armoredOut);
        armoredOut.Close();
        var publicKeyArmored = System.Text.Encoding.UTF8.GetString(publicOut.ToArray());

        // Save secret key
        var secretKeyPath = Path.Combine(_keystoreDir, $"{keyId}_secret.asc");
        using var secretOut = new FileStream(secretKeyPath, FileMode.Create);
        using var armoredSecretOut = new ArmoredOutputStream(secretOut);
        secretKeyRing.Encode(armoredSecretOut);
        armoredSecretOut.Close();

        return Task.FromResult((publicKeyArmored, keyId));
    }

    public Task ImportPublicKeyAsync(Stream keyStream)
    {
        using var decoderStream = PgpUtilities.GetDecoderStream(keyStream);
        var keyRingBundle = new PgpPublicKeyRingBundle(decoderStream);

        foreach (PgpPublicKeyRing keyRing in keyRingBundle.GetKeyRings())
        {
            var publicKey = keyRing.GetPublicKey();
            var keyId = publicKey.KeyId.ToString("X16");
            var keyPath = Path.Combine(_keystoreDir, $"{keyId}_public.asc");

            using var output = new FileStream(keyPath, FileMode.Create);
            using var armoredOut = new ArmoredOutputStream(output);
            keyRing.Encode(armoredOut);
            armoredOut.Close();
        }

        return Task.CompletedTask;
    }

    public Task ImportPrivateKeyAsync(Stream keyStream, string passphrase)
    {
        using var decoderStream = PgpUtilities.GetDecoderStream(keyStream);
        var secretKeyRingBundle = new PgpSecretKeyRingBundle(decoderStream);

        foreach (PgpSecretKeyRing keyRing in secretKeyRingBundle.GetKeyRings())
        {
            var secretKey = keyRing.GetSecretKey();
            var keyId = secretKey.KeyId.ToString("X16");
            var keyPath = Path.Combine(_keystoreDir, $"{keyId}_secret.asc");

            using var output = new FileStream(keyPath, FileMode.Create);
            using var armoredOut = new ArmoredOutputStream(output);
            keyRing.Encode(armoredOut);
            armoredOut.Close();
        }

        return Task.CompletedTask;
    }

    public Task<List<PgpKeyInfo>> GetKeysAsync()
    {
        var keys = new List<PgpKeyInfo>();
        var keyFiles = Directory.GetFiles(_keystoreDir, "*.asc");

        foreach (var keyFile in keyFiles)
        {
            try
            {
                using var fileStream = new FileStream(keyFile, FileMode.Open, FileAccess.Read);
                using var decoderStream = PgpUtilities.GetDecoderStream(fileStream);

                if (keyFile.Contains("_secret"))
                {
                    var secretKeyRingBundle = new PgpSecretKeyRingBundle(decoderStream);
                    foreach (PgpSecretKeyRing keyRing in secretKeyRingBundle.GetKeyRings())
                    {
                        var secretKey = keyRing.GetSecretKey();
                        var publicKey = secretKey.PublicKey;
                        keys.Add(CreatePgpKeyInfo(publicKey, true));
                    }
                }
                else if (keyFile.Contains("_public"))
                {
                    var publicKeyRingBundle = new PgpPublicKeyRingBundle(decoderStream);
                    foreach (PgpPublicKeyRing keyRing in publicKeyRingBundle.GetKeyRings())
                    {
                        var publicKey = keyRing.GetPublicKey();
                        keys.Add(CreatePgpKeyInfo(publicKey, false));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading key file {keyFile}: {ex.Message}");
            }
        }

        return Task.FromResult(keys.DistinctBy(k => k.Id).ToList());
    }

    public Task<byte[]> EncryptAsync(byte[] data, string filename, string recipientKeyId, bool armor, string? signKeyId = null, string? passphrase = null)
    {
        var recipientKey = GetPublicKey(recipientKeyId);
        if (recipientKey == null)
            throw new ArgumentException($"Recipient key {recipientKeyId} not found");

        using var outputStream = new MemoryStream();
        Stream finalOutputStream = outputStream;

        if (armor)
        {
            finalOutputStream = new ArmoredOutputStream(outputStream);
        }

        var encryptedDataGenerator = new PgpEncryptedDataGenerator(
            SymmetricKeyAlgorithmTag.Aes256, true, new SecureRandom());
        encryptedDataGenerator.AddMethod(recipientKey);

        using var encryptedOut = encryptedDataGenerator.Open(finalOutputStream, new byte[1 << 16]);

        var compressedDataGenerator = new PgpCompressedDataGenerator(CompressionAlgorithmTag.ZLib);
        using var compressedOut = compressedDataGenerator.Open(encryptedOut);

        var literalDataGenerator = new PgpLiteralDataGenerator();
        using var literalOut = literalDataGenerator.Open(compressedOut, PgpLiteralData.Binary, filename, (uint)data.Length, DateTime.UtcNow);

        literalOut.Write(data);
        literalOut.Close();
        compressedOut.Close();
        encryptedOut.Close();

        if (armor)
        {
            finalOutputStream.Close();
        }

        return Task.FromResult(outputStream.ToArray());
    }

    public Task<DecryptResult> DecryptAsync(byte[] data, string passphrase)
    {
        using var inputStream = new MemoryStream(data);
        using var decoderStream = PgpUtilities.GetDecoderStream(inputStream);

        var objectFactory = new PgpObjectFactory(decoderStream);
        var obj = objectFactory.NextPgpObject();

        PgpEncryptedDataList? encDataList = null;
        if (obj is PgpEncryptedDataList list)
        {
            encDataList = list;
        }
        else
        {
            encDataList = (PgpEncryptedDataList)objectFactory.NextPgpObject();
        }

        PgpPrivateKey? privateKey = null;
        PgpPublicKeyEncryptedData? encData = null;

        foreach (PgpPublicKeyEncryptedData pked in encDataList.GetEncryptedDataObjects())
        {
            var secretKey = GetSecretKey(pked.KeyId.ToString("X16"));
            if (secretKey != null)
            {
                privateKey = secretKey.ExtractPrivateKey(passphrase.ToCharArray());
                encData = pked;
                break;
            }
        }

        if (privateKey == null || encData == null)
            throw new ArgumentException("Unable to find matching private key or passphrase is incorrect");

        using var clearStream = encData.GetDataStream(privateKey);
        var plainObjectFactory = new PgpObjectFactory(clearStream);

        var message = plainObjectFactory.NextPgpObject();
        
        if (message is PgpCompressedData compressedData)
        {
            using var compressedStream = compressedData.GetDataStream();
            var compressedObjectFactory = new PgpObjectFactory(compressedStream);
            message = compressedObjectFactory.NextPgpObject();
        }

        var result = new DecryptResult();

        if (message is PgpLiteralData literalData)
        {
            result.Filename = literalData.FileName;
            using var literalStream = literalData.GetInputStream();
            using var outputStream = new MemoryStream();
            literalStream.CopyTo(outputStream);
            result.PlaintextData = outputStream.ToArray();
        }

        return Task.FromResult(result);
    }

    private PgpKeyInfo CreatePgpKeyInfo(PgpPublicKey publicKey, bool isSecret)
    {
        var userIds = new List<string>();
        foreach (string userId in publicKey.GetUserIds())
        {
            userIds.Add(userId);
        }

        return new PgpKeyInfo
        {
            Id = publicKey.KeyId.ToString("X16"),
            Uid = userIds.FirstOrDefault() ?? "",
            Fingerprint = BitConverter.ToString(publicKey.GetFingerprint()).Replace("-", ""),
            CanSign = true, // RSA keys can usually sign
            CanEncrypt = publicKey.IsEncryptionKey,
            CreationTime = publicKey.CreationTime,
            IsSecret = isSecret
        };
    }

    private PgpPublicKey? GetPublicKey(string keyId)
    {
        var keyFiles = Directory.GetFiles(_keystoreDir, "*_public.asc")
            .Concat(Directory.GetFiles(_keystoreDir, "*_secret.asc"));

        foreach (var keyFile in keyFiles)
        {
            try
            {
                using var fileStream = new FileStream(keyFile, FileMode.Open, FileAccess.Read);
                using var decoderStream = PgpUtilities.GetDecoderStream(fileStream);

                if (keyFile.Contains("_secret"))
                {
                    var secretKeyRingBundle = new PgpSecretKeyRingBundle(decoderStream);
                    foreach (PgpSecretKeyRing keyRing in secretKeyRingBundle.GetKeyRings())
                    {
                        var secretKey = keyRing.GetSecretKey();
                        if (secretKey.KeyId.ToString("X16").Equals(keyId, StringComparison.OrdinalIgnoreCase))
                        {
                            return secretKey.PublicKey;
                        }
                    }
                }
                else
                {
                    var publicKeyRingBundle = new PgpPublicKeyRingBundle(decoderStream);
                    foreach (PgpPublicKeyRing keyRing in publicKeyRingBundle.GetKeyRings())
                    {
                        var publicKey = keyRing.GetPublicKey();
                        if (publicKey.KeyId.ToString("X16").Equals(keyId, StringComparison.OrdinalIgnoreCase))
                        {
                            return publicKey;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading key file {keyFile}: {ex.Message}");
            }
        }

        return null;
    }

    private PgpSecretKey? GetSecretKey(string keyId)
    {
        var keyFiles = Directory.GetFiles(_keystoreDir, "*_secret.asc");

        foreach (var keyFile in keyFiles)
        {
            try
            {
                using var fileStream = new FileStream(keyFile, FileMode.Open, FileAccess.Read);
                using var decoderStream = PgpUtilities.GetDecoderStream(fileStream);
                var secretKeyRingBundle = new PgpSecretKeyRingBundle(decoderStream);

                foreach (PgpSecretKeyRing keyRing in secretKeyRingBundle.GetKeyRings())
                {
                    var secretKey = keyRing.GetSecretKey();
                    if (secretKey.KeyId.ToString("X16").Equals(keyId, StringComparison.OrdinalIgnoreCase))
                    {
                        return secretKey;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading secret key file {keyFile}: {ex.Message}");
            }
        }

        return null;
    }

    private static Org.BouncyCastle.Bcpg.OpenPgp.PgpKeyPair GenerateRsaKeyPair(int strength)
    {
        var keyGenerator = new RsaKeyPairGenerator();
        keyGenerator.Init(new RsaKeyGenerationParameters(
            BigInteger.ValueOf(0x10001), new SecureRandom(), strength, 12));
        
        var keyPair = keyGenerator.GenerateKeyPair();
        return new Org.BouncyCastle.Bcpg.OpenPgp.PgpKeyPair(PublicKeyAlgorithmTag.RsaGeneral, keyPair, DateTime.UtcNow);
    }
}