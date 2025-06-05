using System;
using Microsoft.Extensions.Caching.Memory;

public class ProductCacheService
{
    // MemoryCache with size limit
    private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions
    {
        SizeLimit = 1024 // Max "size units" in cache (arbitrary units we assign)
    });

    private const int CacheDurationInMinutes = 10;

    /// <summary>
    /// Selectively caches only top products
    /// </summary>
    public void CacheProductIfTop(string productId, string productData, bool isTopProduct)
    {
        if (!isTopProduct)
        {
            Console.WriteLine($"Skip caching {productId} — not a top product.");
            return;
        }

        var cacheKey = $"Product_{productId}";
        _cache.Set(cacheKey, productData, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheDurationInMinutes),
            Size = 1, // Assume each entry takes 1 size unit
            PostEvictionCallbacks =
            {
                new PostEvictionCallbackRegistration
                {
                    EvictionCallback = (key, value, reason, state) =>
                    {
                        Console.WriteLine($"Evicted: {key} due to {reason}");
                    }
                }
            }
        });

        Console.WriteLine($"Cached {productId} (Top product)");
    }

    public string GetProduct(string productId)
    {
        var cacheKey = $"Product_{productId}";
        if (_cache.TryGetValue(cacheKey, out string product))
        {
            Console.WriteLine($"Cache Hit for {productId}");
            return product;
        }

        Console.WriteLine($"Cache Miss for {productId}");
        return null;
    }

    public void CacheProduct_Unbounded(string productId, string productData)
    {
        var cacheKey = $"Product_{productId}";
    
        _cache.Set(cacheKey, productData, new MemoryCacheEntryOptions
        {
            // Still need to set size because SizeLimit is active
            Size = 1
        });

        Console.WriteLine($"❌ Cached {productId} unconditionally (this is bad for memory)");
    }
}
