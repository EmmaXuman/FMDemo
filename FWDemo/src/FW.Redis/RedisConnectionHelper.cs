using FW.Redis.Config;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Redis
{
    public sealed  class RedisConnectionHelper
    {
        private static object _lock = new object();
        private static ConnectionMultiplexer connection;
        private static readonly RedisConfig _redisConfig;

        public RedisConnectionHelper( RedisConfig redisConfig )
        {
            redisConfig = _redisConfig;
        }

        public static ConnectionMultiplexer GetConnection()
        {
            if (connection == null)
            {
                lock (_lock)
                {
                    connection = ConnectionMultiplexer.Connect(_redisConfig.Host);
                }
            }
            return connection;
        }
    }
}
