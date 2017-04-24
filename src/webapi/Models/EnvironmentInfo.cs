using System;


namespace webapi.Models
{
    public class Environment
    {
        public readonly string Version;
        public readonly string HostName;
        public readonly DateTime Timestamp;


        public Environment(
            string version,
            string hostName,
            DateTime timestamp)
        {
            this.Version = version;
            this.HostName = hostName;
            this.Timestamp = timestamp;
        }
    }
}