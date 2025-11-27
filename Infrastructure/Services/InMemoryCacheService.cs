using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services;

public sealed class InMemoryCacheService(IMemoryCache cache) : ICacheService
{
    public Task<T?> GetAsync<T>(string key)
    {
        cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions();
        if (expiration.HasValue) options.SetAbsoluteExpiration(expiration.Value);

        cache.Set(key, value, options);
        return Task.CompletedTask;
    }

    public bool Exists(string key)
    {
        return cache.TryGetValue(key, out _);
    }
}