#!/bin/bash

# Generate GPG keys for demo purposes
KEYS_DIR="$(dirname "$0")/../../samples/keys"

# Create keys directory if it doesn't exist
mkdir -p "$KEYS_DIR"

# GPG key generation parameters
CLIENT_NAME="Client Demo"
CLIENT_EMAIL="client@example.com"
SERVER_NAME="Server Demo"
SERVER_EMAIL="server@example.com"

echo "Generating GPG keys for demo..."

# Create batch file for client key generation
cat > /tmp/client_gpg_batch << EOF
%echo Generating client key
Key-Type: RSA
Key-Length: 4096
Subkey-Type: RSA
Subkey-Length: 4096
Name-Real: $CLIENT_NAME
Name-Email: $CLIENT_EMAIL
Expire-Date: 2y
Passphrase: client123
%commit
%echo Client key generated
EOF

# Create batch file for server key generation
cat > /tmp/server_gpg_batch << EOF
%echo Generating server key
Key-Type: RSA
Key-Length: 4096
Subkey-Type: RSA
Subkey-Length: 4096
Name-Real: $SERVER_NAME
Name-Email: $SERVER_EMAIL
Expire-Date: 2y
Passphrase: server123
%commit
%echo Server key generated
EOF

# Generate client keys
echo "Generating client GPG key..."
gpg --batch --generate-key /tmp/client_gpg_batch

# Generate server keys
echo "Generating server GPG key..."
gpg --batch --generate-key /tmp/server_gpg_batch

# Export client keys
echo "Exporting client keys..."
gpg --armor --export "$CLIENT_EMAIL" > "$KEYS_DIR/client_pub.asc"
gpg --armor --export-secret-keys "$CLIENT_EMAIL" > "$KEYS_DIR/client_priv.asc"

# Export server keys
echo "Exporting server keys..."
gpg --armor --export "$SERVER_EMAIL" > "$KEYS_DIR/server_pub.asc"
gpg --armor --export-secret-keys "$SERVER_EMAIL" > "$KEYS_DIR/server_priv.asc"

# Clean up batch files
rm -f /tmp/client_gpg_batch /tmp/server_gpg_batch

echo "GPG keys generated successfully:"
echo "Client public key: $KEYS_DIR/client_pub.asc"
echo "Client private key: $KEYS_DIR/client_priv.asc (passphrase: client123)"
echo "Server public key: $KEYS_DIR/server_pub.asc"
echo "Server private key: $KEYS_DIR/server_priv.asc (passphrase: server123)"

echo ""
echo "Note: These are demo keys for testing purposes only!"
echo "In production, generate keys with strong passphrases and keep private keys secure."