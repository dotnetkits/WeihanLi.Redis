﻿using System;
using System.Net;

namespace WeihanLi.Redis.Internals
{
    internal static class Helpers
    {
        public static string GetCachePrefix(RedisDataType redisDataType)
        {
            switch (redisDataType)
            {
                case RedisDataType.Cache:
                    return RedisConstants.CachePrefix;

                case RedisDataType.Counter:
                    return RedisConstants.CounterPrefix;

                case RedisDataType.Firewall:
                    return RedisConstants.FirewallPrefix;

                case RedisDataType.RedLock:
                    return RedisConstants.RedLockPrefix;

                case RedisDataType.Hash:
                    return RedisConstants.HashPrefix;

                case RedisDataType.Dictionary:
                    return RedisConstants.DictionaryPrefix;

                case RedisDataType.List:
                    return RedisConstants.ListPrefix;

                case RedisDataType.Set:
                    return RedisConstants.SetPrefix;

                case RedisDataType.SortedSet:
                    return RedisConstants.SortedSetPrefix;

                case RedisDataType.Rank:
                    return RedisConstants.RankPrefix;

                default:
                    throw new ArgumentOutOfRangeException(nameof(redisDataType), redisDataType, null);
            }
        }

        public static EndPoint ConvertToEndPoint(string host, int port)
        {
            if (IPAddress.TryParse(host, out var address))
            {
                return new IPEndPoint(address, port);
            }
            return new DnsEndPoint(host, port);
        }
    }
}
