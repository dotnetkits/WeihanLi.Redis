﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using WeihanLi.Common;

namespace WeihanLi.Redis.UnitTest
{
    public class BaseUnitTest
    {
        static BaseUnitTest()
        {
            var dbIndex = 7;

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddRedisConfig(config =>
            {
                //
                //config.RedisServers = new[]
                //{
                //    new RedisServerConfiguration("127.0.0.1", 6379),
                //};
                config.CachePrefix = "WeihanLi.Redis.UnitTest";
                config.ClientName = "WeihanLi.Redis.UnitTest";

                // config.EnableCompress = false;
                config.DefaultDatabase = dbIndex;
            });

            DependencyResolver.SetDependencyResolver(serviceCollection);

            DependencyResolver.Current.ResolveService<ILoggerFactory>().AddLog4Net();

            // clear keys
            var connection = DependencyResolver.Current.ResolveService<IConnectionMultiplexer>();
            var server = connection.GetServer("127.0.0.1", 6379);
            var db = connection.GetDatabase(dbIndex);
            foreach (var key in server.Keys(dbIndex, "WeihanLi.Redis.UnitTest:*"))
            {
                db.KeyDelete(key);
            }
        }
    }
}
