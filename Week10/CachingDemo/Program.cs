using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.Json;
using CachingDemo;
using StackExchange.Redis;

class Program
{
    static async Task Main()
    {
        
        // cache
        var service = new UserProfileService();
        var profile1 = service.GetUserProfile("123");  // Cache Miss
        var profile2 = service.GetUserProfile("123");  // Cache Hit
        service.UpdateUserProfile("123", "New profile info");  // Cache Invalidated
        var profile3 = service.GetUserProfile("123");  // Cache Miss again

        
        // var productService = new ProductService();
        //
        // Console.WriteLine(productService.GetProductDetails("101"));
        // Console.WriteLine(productService.GetProductDetails("101")); // Cache hit
        //
        // productService.UpdateProduct_WriteThrough("101", "Product 101 - Price: $120");
        //
        // await productService.UpdateProduct_WriteBehindAsync("102", "Product 102 - Price: $90");
        //
        // productService.UpdateProduct_TooFrequentCacheUpdates("103", "Product 103 - Name: Added dot");
        //
        // productService.UpdateProduct_SmartCacheUpdate("103", "Product 103 - Price: $130", "Product 103 - Price: $120");

        
        // var productCacheService = new ProductCacheService();
        //
        // productCacheService.CacheProductIfTop("101", "Product 101 - Popular", isTopProduct: true);   // cached
        // productCacheService.CacheProductIfTop("102", "Product 102 - Rarely viewed", isTopProduct: false); //  skipped
        //
        // Console.WriteLine(productCacheService.GetProduct("101"));
        // Console.WriteLine(productCacheService.GetProduct("102")); // will miss
        //
        // productCacheService.CacheProduct_Unbounded("999", "Product 999 - Never accessed"); // Poor practice

        // var cacheService = new DynamicCacheService(initialSizeLimit: 3);
        //
        // // Fill the cache
        // cacheService.AddToCache("item1", "value1");
        // cacheService.AddToCache("item2", "value2");
        // cacheService.AddToCache("item3", "value3");
        //
        // // This will evict one due to LRU
        // cacheService.AddToCache("item4", "value4");
        //
        // cacheService.GetFromCache("item1");
        // cacheService.GetFromCache("item2");
        //
        // // Dynamically increase cache size
        // cacheService.AdjustCacheSize(newSizeLimit: 5);
        //
        // cacheService.AddToCache("item5", "value5");
        // cacheService.AddToCache("item6", "value6");
        //
        // // Static cache size example (bad)
        // var staticCache = DynamicCacheService.CreateStaticSizeCache();
        
        //var secureService = new SecureCacheService();

        // Encrypt & cache sensitive info
        //secureService.CacheSensitiveData("user123", "SSN: 123-45-6789");

        // Authorized fetch
        //string ssn = secureService.GetSensitiveData("user123", "user123");
        //Console.WriteLine($"Fetched: {ssn}");

        // Unauthorized fetch
        //string unauthorized = secureService.GetSensitiveData("user123", "hacker456");

        // Insecure cache usage (bad practice)
        //secureService.CachePlainSensitiveData_Insecure("user123", "Credit Card: 4111-1111-1111-1111");


        //In memory cache
        
        // Set up the MemoryCache instance
        // var cache = new MemoryCache(new MemoryCacheOptions());
        //
        // // Sample data to cache
        // string cacheKey = "userProfile:123";
        // var userProfile = new { Name = "John Doe", Age = 30 };
        //
        // // Cache the data
        // cache.Set(cacheKey, userProfile, TimeSpan.FromMinutes(5)); // Expiry in 5 minutes
        //
        // // Retrieve the data from cache
        // var cachedUserProfile = cache.Get(cacheKey);
        //
        // if (cachedUserProfile != null)
        // {
        //     Console.WriteLine($"User Profile Retrieved: {((dynamic)cachedUserProfile).Name}, Age: {((dynamic)cachedUserProfile).Age}");
        // }
        // else
        // {
        //     Console.WriteLine("Data not found in cache.");
        // }
        
        
        //redis
        // Connect to Redis server (change with your Redis instance configuration)
        // var connection = ConnectionMultiplexer.Connect("localhost");
        // var database = connection.GetDatabase();
        //
        // // Sample data to cache
        // string cacheKey = "userProfile:123";
        // var userProfile = new UserProfile { Name = "Jane Smith", Age = 28 };
        //
        // // Cache the data in Redis with an expiration time of 5 minutes
        // database.StringSet(cacheKey, JsonSerializer.Serialize(userProfile), TimeSpan.FromMinutes(5));
        //
        // // Retrieve the data from Redis
        // var cachedData = database.StringGet(cacheKey);
        //
        // if (cachedData.HasValue)
        // {
        //     var retrievedProfile = JsonSerializer.Deserialize<UserProfile>(cachedData);
        //     Console.WriteLine($"User Profile Retrieved: {retrievedProfile.Name}, Age: {retrievedProfile.Age}");
        // }
        // else
        // {
        //     Console.WriteLine("Data not found in Redis cache.");
        // }
        //
        Console.ReadLine();
    }
}

public class UserProfile
{
    public string Name { get; set; }
    public int Age { get; set; }
}