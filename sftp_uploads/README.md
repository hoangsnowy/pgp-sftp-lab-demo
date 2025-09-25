# SFTP Upload Directory

This directory is mounted as `/home/demo/upload/` in the SFTP container.

Files uploaded through the SFTP interface will appear here.

## Usage
- **SFTP Server**: `localhost:2222`
- **Username**: `demo`
- **Password**: `pass`
- **Upload Path**: `/home/demo/upload/`

## Note
This directory is automatically created when the SFTP container starts.
All uploaded files will be stored here and can be managed through the VaultLink interface.