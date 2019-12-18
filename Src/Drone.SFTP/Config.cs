using System;
using System.IO;

namespace Drone.SFTP
{
    public class Config
    {
        private Config()
        {
        }

        public static Config Load()
        {
            return new Config().FromEnvironment().Validated();
        }

        private Config Validated()
        {
            if (string.IsNullOrEmpty(Host))
            {
                throw new ArgumentException("Host must be given.", nameof(Host));
            }

            if (string.IsNullOrEmpty(Username))
            {
                throw new ArgumentException("Username must be given.", nameof(Username));
            }

            if (Port <= 0 || Port > 65535)
            {
                throw new ArgumentException("Port is not in the valid range.", nameof(Port));
            }

            if (!Directory.Exists(Source))
            {
                throw new ArgumentException("Source directory does not exist.", nameof(Source));
            }

            if (string.IsNullOrEmpty(Filter))
            {
                Filter = "*.*";
            }

            return this;
        }

        private Config FromEnvironment()
        {
            Host = Environment.GetEnvironmentVariable("PLUGIN_HOST") ?? "";
            Port = int.TryParse(Environment.GetEnvironmentVariable("PLUGIN_PORT"), out int port) ? port : 22;
            Username = Environment.GetEnvironmentVariable("PLUGIN_USERNAME") ?? "";
            Password = Environment.GetEnvironmentVariable("PLUGIN_PASSWORD") ?? "";
            Source = Environment.GetEnvironmentVariable("PLUGIN_SOURCE") ?? "./";
            Filter = Environment.GetEnvironmentVariable("PLUGIN_FILTER") ?? "*.*";
            Target = Environment.GetEnvironmentVariable("PLUGIN_TARGET") ?? "/";
            Clear = bool.TryParse(Environment.GetEnvironmentVariable("PLUGIN_CLEAR"), out bool clear) ? clear : false;
            Overwrite = bool.TryParse(Environment.GetEnvironmentVariable("PLUGIN_OVERWRITE"), out bool overwrite) ? overwrite : false;

            return this;
        }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Source { get; set; }

        public string Filter { get; set; }

        public string Target { get; set; }

        public bool Clear { get; set; }

        public bool Overwrite { get; set; }
    }
}
