using Renci.SshNet;
using System;
using System.IO;

namespace Drone.SFTP
{
    static class Program
    {
        static void Main(string[] args)
        {
            var config = Config.Load();

            var connectionInfo = new ConnectionInfo(config.Host, config.Port, config.Username, new PasswordAuthenticationMethod(config.Username, config.Password));

            using (var sftp = new SftpClient(connectionInfo))
            {
                Console.WriteLine("Connecting to host");
                sftp.Connect();

                if (!sftp.IsConnected)
                {
                    Console.WriteLine("Could not connect to host");
                    return;
                }

                // Create target directory if it does not exist
                if (!sftp.Exists(config.Target))
                {
                    Console.WriteLine("Creating target directory as it does not exist yet");
                    sftp.CreateDirectory(config.Target);
                }

                // Clear if wanted
                if (config.Clear)
                {
                    Console.WriteLine("Starting to clear files and directories in target directory");
                    var filesDeleted = sftp.DeleteDirectoryRecursively(config.Target, config.Verbose);
                    Console.WriteLine($"Cleared {filesDeleted} files in target directory");
                }

                // Prepare the upload
                var filesToUpload = Directory.GetFiles(config.Source, config.Filter, SearchOption.AllDirectories);
                Console.WriteLine($"Starting to upload {filesToUpload.Length} files");

                // Do the actual upload
                foreach (var sourceFile in filesToUpload)
                {
                    var pathSegment = Path.GetRelativePath(config.Source, sourceFile);
                    var targetFile = Path.Combine(config.Target, pathSegment).Replace("\\", "/");
                    var targetPath = Path.GetDirectoryName(targetFile).Replace("\\", "/");

                    if (config.Verbose)
                    {
                        Console.WriteLine($"Uploading {sourceFile} to {targetFile}");
                    }

                    using (var stream = File.OpenRead(sourceFile))
                    {
                        if (!sftp.Exists(targetPath))
                        {
                            sftp.CreateDirectoryRecursively(targetPath);
                        }

                        sftp.UploadFile(stream, targetFile, config.Overwrite);
                    }
                }
                Console.WriteLine($"Uploaded {filesToUpload.Length} files");
            }
        }
    }
}
