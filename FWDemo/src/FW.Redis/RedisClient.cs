using FW.Common.Extensions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FW.Redis
{
    public class RedisClient
    {
        private readonly ConnectionMultiplexer _connection;

        public RedisClient()
        {
            _connection = RedisConnectionHelper.GetConnection();
        }

        private IDatabase _db => _connection.GetDatabase(0);

        public string Get( string key )
        {
            return _db.StringGet(key);
        }

        public async Task<string> GetAsync( string key )
        {
            return await _db.StringGetAsync(key);
        }

        public T Get<T>( string key )
        {
            var result = _db.StringGet(key);
            if (result.IsNull)
                return default(T);
            return result.ToJsonString().GetDeserializeObject<T>();
        }

        public async Task<T> GetTAsync<T>( string key )
        {
            var result = await _db.StringGetAsync(key);
            if (result.IsNull)
                return default;
            return result.ToString().GetDeserializeObject<T>();
        }
        /// <summary>
        /// 保存key value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间 默认为永久</param>
        public void Set( string key, string value, TimeSpan? expiry = default(TimeSpan?) )
        {
            _db.StringSet(key, value, expiry);
        }

        public async Task SetAsync(string key,string value,TimeSpan? expiry=default)
        {
            await _db.StringSetAsync(key,value,expiry);
        }
        /// <summary>
        /// 保存key value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间 默认为永久</param>
        public void Set<T>( string key, T value, TimeSpan? expiry = default )
        {
            var jsonValue = value.ToJsonString();
            _db.StringSet(key, jsonValue, expiry);
        }

        public async Task SetAsync<T>( string key, T value, TimeSpan? expiry = default )
        {
            var jsonValue = value.ToJsonString();
            await _db.StringSetAsync(key, jsonValue, expiry);
        }
        /// <summary>
        /// 移除key对应的值
        /// </summary>
        /// <param name="key"></param>
        public void Remove( string key )
        {
            _db.KeyDelete(key);
        }

        public async Task RemoveAsync( string key )
        {
           await  _db.KeyDeleteAsync(key);
        }
        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">需要增加的数字 默认为1</param>
        /// <returns>增加后的值</returns>
        public double Increment( string key, double val = 1 )
        {
            return _db.StringIncrement(key,val);
        }

        public async Task<double> IncrementAsync( string key, double val = 1 )
        {
            return await _db.StringIncrementAsync(key, val);
        }
        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns>减少后的值</returns>
        public double Decrement( string key, double val = 1 )
        {
            return _db.StringDecrement(key, val);
        }
    }
}
