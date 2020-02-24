using AutoMapper.Configuration;
using StackExchange.Redis;

namespace BE.Cache
{
    public static class RedisClient
    {
        public static ConnectionMultiplexer Client;

        static RedisClient()
        {
            Client = ConnectionMultiplexer.Connect("localhost:6379");
        }
    }
}