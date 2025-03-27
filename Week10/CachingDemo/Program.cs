using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.Json;
using StackExchange.Redis;

class Program
{
    static void Main()
    {
        
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
        var connection = ConnectionMultiplexer.Connect("localhost");
        var database = connection.GetDatabase();

        // Sample data to cache
        string cacheKey = "userProfile:123";
        var userProfile = new UserProfile { Name = "Jane Smith", Age = 28 };

        // Cache the data in Redis with an expiration time of 5 minutes
        database.StringSet(cacheKey, JsonSerializer.Serialize(userProfile), TimeSpan.FromMinutes(5));

        // Retrieve the data from Redis
        var cachedData = database.StringGet(cacheKey);

        if (cachedData.HasValue)
        {
            var retrievedProfile = JsonSerializer.Deserialize<UserProfile>(cachedData);
            Console.WriteLine($"User Profile Retrieved: {retrievedProfile.Name}, Age: {retrievedProfile.Age}");
        }
        else
        {
            Console.WriteLine("Data not found in Redis cache.");
        }
        
    }
}

public class UserProfile
{
    public string Name { get; set; }
    public int Age { get; set; }
}