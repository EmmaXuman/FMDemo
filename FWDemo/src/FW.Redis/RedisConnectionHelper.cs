using FW.Redis.Config;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Redis
{
    public class RedisConnectionHelper
    {
        //定义私有静态字段
        private static RedisConnectionHelper redisInstance;

        private static readonly object _lock = new object();
        private static ConnectionMultiplexer connection;
        private static readonly RedisConfig _redisConfig;

        //定义私有构造函数，外部不可new
        private RedisConnectionHelper()
        { }

        public RedisConnectionHelper( RedisConfig redisConfig )
        {
            redisConfig = _redisConfig;
        }

        public static RedisConnectionHelper GetRedisHelper()
        {
            if (redisInstance == null) 
            {
                lock (_lock)
                {
                    redisInstance = new RedisConnectionHelper();
                }
            }
            return redisInstance;
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
