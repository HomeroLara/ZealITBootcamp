using System;

using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Cache Invalidation Demo
/// </summary>
public class UserProfileService
{
    private static readonly MemoryCache _cache =  new MemoryCache(new MemoryCacheOptions());
    private const int CacheDurationInMinutes = 5;

    public string GetUserProfile(string userId)
    {
        string cacheKey = $"UserProfile_{userId}";
        string cachedProfile = _cache.Get(cacheKey) as string;

        if (cachedProfile != null)
        {
            Console.WriteLine("Cache Hit");
            return cachedProfile;
        }

        Console.WriteLine("Cache Miss - Fetching from DB");
        string profileFromDb = FetchUserProfileFromDatabase(userId);

        // Time-based expiration: Cache for 5 minutes
        _cache.Set(cacheKey, profileFromDb, DateTimeOffset.Now.AddMinutes(CacheDurationInMinutes));

        return profileFromDb;
    }

    public void UpdateUserProfile(string userId, string newProfileData)
    {
        // Simulate update in DB
        UpdateUserProfileInDatabase(userId, newProfileData);

        // Event-based invalidation: Remove from cache
        string cacheKey = $"UserProfile_{userId}";
        _cache.Remove(cacheKey);
        Console.WriteLine("Cache Invalidated on Update");
    }

    private string FetchUserProfileFromDatabase(string userId)
    {
        // Simulated DB call
        return $"User profile for {userId} at {DateTime.Now}";
    }

    private void UpdateUserProfileInDatabase(string userId, string newData)
    {
        // Simulate DB update
        Console.WriteLine($"DB Updated for {userId}: {newData}");
    }
}