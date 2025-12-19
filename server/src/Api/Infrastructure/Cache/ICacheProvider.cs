using System;
using System.Threading.Tasks;

namespace Api.Infrastructure.Cache
{
    public interface ICacheProvider
    {
        Task<TValue?> ReadThroughAsync<TValue>(string cacheKey, ICacheOptions cacheOptions, Func<Task<TValue?>> source) where TValue : class;
        void Remove(string cacheKey);
        void Remove(string[] cacheKeys);
    }
}