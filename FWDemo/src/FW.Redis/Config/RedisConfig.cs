using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Redis.Config
{
    public class RedisConfig
    {
        public const string Config = "Redis:Redis_Default";

        public string Host { get; set; }

        public string DefaultDb { get; set; }
        public string InstanceName { get; set; }
    }
}
