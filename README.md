# drone-sftp

A plugin for drone to upload files to an SFTP server.

## Usage

```yaml
- name: deploy
  image: hypervtechnics/drone-sftp
  settings:
    host: sftp.example.com        # The host
    port: 22                      # The port
    username:                     # The username
      from_secret: sftp_username  # Use username from secret store
    password:                     # The password
      from_secret: sftp_password  # Use password from secret store
    source: ./                    # The source directory
    filter: *.*                   # The filter for the files to upload. *.* is for all
    target: /                     # The target directory
    clean: false                  # Removes all files and directories in target
    overwrite: false              # Allows overwriting already existing files
```
