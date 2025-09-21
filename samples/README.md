# Sample Files for PGP + SFTP Demo

## 📁 Directory Purpose
This `samples/` directory contains demonstration files to help you test the PGP encryption and SFTP transfer functionality.

## 📄 Files Included

### `hello.txt`
- **Purpose**: Sample plaintext file for encryption testing
- **Usage**: Upload this file in the "Encrypt/Sign" tab to test PGP encryption
- **Content**: Simple text with instructions for testing

### `keys/` Directory
Contains sample PGP keys for demonstration:

#### `client_pub.asc`
- **Type**: PGP Public Key (ASCII Armored)
- **Owner**: Sample Client (client@demo.com)
- **Key ID**: 0753C2AA682FDCD4
- **Usage**: Import this in "Key Management" tab to test encryption for this recipient

#### `client_priv.asc` *(Coming Soon)*
- **Type**: PGP Private Key (ASCII Armored)
- **Password**: demo123
- **Usage**: Import to test decryption of files encrypted for this key

#### `server_pub.asc` & `server_priv.asc` *(Coming Soon)*
- **Purpose**: Additional key pairs for multi-user scenario testing

## 🔄 Testing Workflow

1. **Import Keys**: Go to "Key Management" → Import the sample public key
2. **Encrypt File**: Go to "Encrypt/Sign" → Upload `hello.txt` → Select recipient key → Download encrypted file
3. **SFTP Transfer**: Go to "SSH/SFTP" → Upload encrypted file to server
4. **Download & Decrypt**: Download from SFTP → Go to "Decrypt/Verify" → Decrypt with password
5. **Verify**: Check that decrypted content matches original `hello.txt`

## 🎯 Demo Scenarios

- **Single User**: Generate your own keys and encrypt/decrypt files
- **Multi User**: Use sample keys to simulate sending encrypted files between users
- **File Transfer**: Test complete workflow with SFTP server integration
- **Security**: Verify digital signatures and encryption integrity