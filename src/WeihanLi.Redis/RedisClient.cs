﻿using System;
using System.Linq;
using StackExchange.Redis;
using WeihanLi.Common.Log;
using WeihanLi.Extensions;
using WeihanLi.Redis.Internals;

namespace WeihanLi.Redis
{
    public interface IRedisClient
    {
    }

    internal abstract class BaseRedisClient
    {
        private static readonly ConnectionMultiplexer Connection;

        /// <summary>
        /// 随机数生成器
        /// </summary>
        protected readonly Random Random = new Random();

        public IRedisWrapper Wrapper { get; }

        /// <summary>
        /// logger
        /// </summary>
        protected ILogHelper Logger { get; }

        static BaseRedisClient()
        {
            var configurationOptions = new ConfigurationOptions
            {
                Password = RedisManager.RedisConfiguration.Password,
                DefaultDatabase = RedisManager.RedisConfiguration.DefaultDatabase,
                ConnectRetry = RedisManager.RedisConfiguration.ConnectRetry,
                ConnectTimeout = RedisManager.RedisConfiguration.ConnectTimeout,
                AllowAdmin = RedisManager.RedisConfiguration.AllowAdmin,
                Ssl = RedisManager.RedisConfiguration.Ssl,
                Proxy = RedisManager.RedisConfiguration.Proxy,
                AbortOnConnectFail = RedisManager.RedisConfiguration.AbortOnConnectFail,
                SyncTimeout = RedisManager.RedisConfiguration.SyncTimeout
            };
            configurationOptions.EndPoints.AddRange(RedisManager.RedisConfiguration.RedisServers.Select(s => Helpers.ConvertToEndPoint(s.Host, s.Port)).ToArray());
            Connection = ConnectionMultiplexer.Connect(configurationOptions);
        }

        #region GetRandomCacheExpiry

        protected TimeSpan GetRandomCacheExpiry() => GetRandomCacheExpiry(RedisManager.RedisConfiguration.MaxRandomCacheExpiry);

        protected TimeSpan GetRandomCacheExpiry(int max) => TimeSpan.FromSeconds(Random.Next(max));

        protected TimeSpan GetRandomCacheExpiry(int min, int max) => TimeSpan.FromSeconds(Random.Next(min, max));

        #endregion GetRandomCacheExpiry

        protected BaseRedisClient(ILogHelper logger, IRedisWrapper redisWrapper)
        {
            Logger = logger;
            Wrapper = redisWrapper;
            Wrapper.Database = Connection.GetDatabase();
            Wrapper.Subscriber = Connection.GetSubscriber();
        }
    }
}
