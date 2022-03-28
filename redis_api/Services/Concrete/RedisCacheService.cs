using Microsoft.Extensions.Configuration;
using redis_api.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace redis_api.Services.Concrete
{
    public class RedisCacheService : ICacheService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        public RedisCacheService(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("RedisConfiguration:ConnectionString")?.Value;
            ConfigurationOptions options = new ConfigurationOptions
            {
                EndPoints =
                {
                    connectionString
                },
                AbortOnConnectFail = false, //redise bağlanmadığı durumda
                AsyncTimeout = 10000, //async isteklerde redise 10sn den geç yanıt verilirse timouta düşür
                ConnectTimeout = 10000 //normal isteklerde redise 10 sn den geç yanıt gelirse timeouta düşür
            };
            _connectionMultiplexer = ConnectionMultiplexer.Connect(options);
        }
        public T Get<T>(string key) where T : class
        {
            string value = _connectionMultiplexer.GetDatabase().StringGet(key);
            return value.ToObject<T>();
        }

        public string Get(string key)
        {
            return _connectionMultiplexer.GetDatabase().StringGet(key);
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            string value = await _connectionMultiplexer.GetDatabase().StringGetAsync(key);
            return value.ToObject<T>();
        }

        public void Remove(string key)
        {
            _connectionMultiplexer.GetDatabase().KeyDelete(key);
        }

        public void Set(string key, string value)
        {
            _connectionMultiplexer.GetDatabase().StringSet(key, value);
        }

        public void Set<T>(string key, T value) where T : class
        {
            _connectionMultiplexer.GetDatabase().StringSet(key, value.ToJson());
        }

        public void Set(string key, object value, TimeSpan expiration)
        {
            _connectionMultiplexer.GetDatabase().StringSet(key, value.ToJson(), expiration);
        }

        public Task SetAsync(string key, object value)
        {
            return _connectionMultiplexer.GetDatabase().StringSetAsync(key, value.ToJson());
        }

        public Task SetAsync(string key, object value, TimeSpan expiration)
        {
            return _connectionMultiplexer.GetDatabase().StringSetAsync(key, value.ToJson(), expiration);
        }
    }
}
