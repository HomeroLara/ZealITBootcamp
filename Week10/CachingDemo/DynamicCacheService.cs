using System;
using Microsoft.Extensions.Caching.Memory;

public class DynamicCacheService
{
    private MemoryCache _cache;
    private int _currentSizeLimit;

    public DynamicCacheService(int initialSizeLimit)
    {
        _currentSizeLimit = initialSizeLimit;
        InitializeCache(_currentSizeLimit);
    }

    private void InitializeCache(int sizeLimit)
    {
        _cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = sizeLimit
        });

        Console.WriteLine($"Initialized cache with size limit: {_currentSizeLimit}");
    }

    /// <summary>
    /// Dynamically adjusts the size limit
    /// </summary>
    public void AdjustCacheSize(int newSizeLimit)
    {
        Console.WriteLine($"Adjusting cache size from {_currentSizeLimit} to {newSizeLimit}");

        _currentSizeLimit = newSizeLimit;

        // Dispose old cache and reinitialize with new limit
        _cache.Dispose();
        InitializeCache(newSizeLimit);
    }

    /// <summary>
    /// Adds data to the cache with eviction logging
    /// </summary>
    public void AddToCache(string key, string value)
    {
        _cache.Set(key, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            Size = 1, // Simulated unit size
            PostEvictionCallbacks =
            {
                new PostEvictionCallbackRegistration
                {
                    EvictionCallback = (k, v, reason, state) =>
                    {
                        Console.WriteLine($"Evicted {k}: Reason = {reason}");
                    }
                }
            }
        });

        Console.WriteLine($"Cached: {key}");
    }

    public string GetFromCache(string key)
    {
        if (_cache.TryGetValue(key, out string value))
        {
            Console.WriteLine($"Cache Hit: {key}");
            return value;
        }

        Console.WriteLine($"Cache Miss: {key}");
        return null;
    }

    /// <summary>
    /// ❌ Pitfall: Cache initialized with static size
    /// </summary>
    public static MemoryCache CreateStaticSizeCache()
    {
        Console.WriteLine("Static size cache with size limit = 2");
        return new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 2
        });
    }
}
