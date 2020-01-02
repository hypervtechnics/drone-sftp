using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Drone.SFTP
{
    public static class SftpClientExtensions
    {
        public static int DeleteDirectoryRecursively(this SftpClient sftp, string path, bool verbose)
        {
            var filesDeleted = 0;

            void ClearFiles(List<SftpFile> entities)
            {
                var files = entities.Where(e => e.IsRegularFile).ToList();
                var directories = entities.Where(e => e.IsDirectory && (e.Name != "." && e.Name != "..")).ToList();

                foreach (var file in files)
                {
                    sftp.DeleteFile(file.FullName);
                    filesDeleted++;

                    if (verbose)
                    {
                        Console.WriteLine($"Deleted file {file.FullName}");
                    }
                }

                foreach (var dir in directories)
                {
                    ClearFiles(sftp.ListDirectory(dir.FullName).ToList());
                    sftp.DeleteDirectory(dir.FullName);

                    if (verbose)
                    {
                        Console.WriteLine($"Deleted directory {dir.FullName}");
                    }
                }
            }

            ClearFiles(sftp.ListDirectory(path).ToList());

            return filesDeleted;
        }

        public static void CreateDirectoryRecursively(this SftpClient sftp, string path)
        {
            string current = "";

            if (path[0] == '/')
            {
                path = path.Substring(1);
            }

            while (!string.IsNullOrEmpty(path))
            {
                int p = path.IndexOf('/');
                current += '/';
                if (p >= 0)
                {
                    current += path.Substring(0, p);
                    path = path.Substring(p + 1);
                }
                else
                {
                    current += path;
                    path = "";
                }

                try
                {
                    SftpFileAttributes attrs = sftp.GetAttributes(current);
                    if (!attrs.IsDirectory)
                    {
                        throw new Exception("not directory");
                    }
                }
                catch (SftpPathNotFoundException)
                {
                    sftp.CreateDirectory(current);
                }
            }
        }
    }
}
