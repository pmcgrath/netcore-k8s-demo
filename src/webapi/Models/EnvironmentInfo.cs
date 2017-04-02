using System;


namespace webapi.Models
{
    public class Environment
    {
        public readonly string Version;
        public readonly string MachineName;


        public Environment(
            string version,
            string machineName)
        {
            this.Version = version;
            this.MachineName = machineName;
        }
    }
}