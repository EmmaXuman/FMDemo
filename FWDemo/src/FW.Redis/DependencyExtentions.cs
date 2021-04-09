using FW.Redis.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace FW.Redis
{
    public static class DependencyExtentions
    {
        public static IServiceCollection AddRedisService(this IServiceCollection services,IConfiguration configuration)
        {
            RedisConfig redisConfig = new RedisConfig();
            configuration.GetSection(RedisConfig.Config).Bind(
            redisConfig);
            services.AddSingleton(redisConfig);
            return services;
        }
    }
}
