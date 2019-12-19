# drone-sftp

A plugin for drone to upload files to an SFTP server.

## Usage

For now this is not the native drone plugin usage style.

```yaml
- name: deploy
  image: hypervtechnics/drone-sftp
  commands:
  - dotnet /app/Drone.SFTP.dll
  environment:
    PLUGIN_HOST: sftp.example.com        # The host
    PLUGIN_PORT: 22                      # The port
    PLUGIN_USERNAME:                     # The username
      from_secret: sftp_username         # Use username from secret store
    PLUGIN_PASSWORD:                     # The password
      from_secret: sftp_password         # Use password from secret store
    PLUGIN_SOURCE: ./                    # The source directory
    PLUGIN_FILTER: *.*                   # The filter for the files to upload. *.* is for all
    PLUGIN_TARGET: /                     # The target directory
    PLUGIN_CLEAR: false                  # Removes all files and directories in target
    PLUGIN_OVERWRITE: false              # Allows overwriting already existing files
```
