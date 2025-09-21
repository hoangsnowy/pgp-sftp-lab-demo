#!/bin/bash

# Generate SSH Ed25519 key for demo user
KEY_DIR="$(dirname "$0")/../ssh/keys"
KEY_NAME="demo_ed25519"

# Create keys directory if it doesn't exist
mkdir -p "$KEY_DIR"

# Generate Ed25519 key pair
ssh-keygen -t ed25519 -f "$KEY_DIR/$KEY_NAME" -N "" -C "demo@vaultlink.local"

# Copy public key for SFTP container
cp "$KEY_DIR/$KEY_NAME.pub" "$KEY_DIR/00_demo.pub"

echo "SSH Ed25519 key generated successfully:"
echo "Private key: $KEY_DIR/$KEY_NAME"
echo "Public key: $KEY_DIR/$KEY_NAME.pub"
echo "Authorized key: $KEY_DIR/00_demo.pub"

# Set proper permissions
chmod 600 "$KEY_DIR/$KEY_NAME"
chmod 644 "$KEY_DIR/$KEY_NAME.pub"
chmod 644 "$KEY_DIR/00_demo.pub"

echo "Permissions set correctly."