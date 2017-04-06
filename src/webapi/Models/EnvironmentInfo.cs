using System;


namespace webapi.Models
{
    public class Environment
    {
        public readonly string Version;
        public readonly string HostName;


        public Environment(
            string version,
            string hostName)
        {
            this.Version = version;
            this.HostName = hostName;
        }
    }
}