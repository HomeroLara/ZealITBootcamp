using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// In-memory cache API
app.MapGet("/inmemorycache", (IMemoryCache memoryCache, ILogger<Program> logger) =>
{
    string cacheKey = "userProfile:123";
    if (!memoryCache.TryGetValue(cacheKey, out UserProfile userProfile))
    {
        logger.LogInformation("Cache miss for key {CacheKey}. Setting new value.", cacheKey);
        userProfile = new UserProfile { Name = "John Doe", Age = 30 };
        memoryCache.Set(cacheKey, userProfile, TimeSpan.FromMinutes(5));
    }
    else
    {
        logger.LogInformation("Cache hit for key {CacheKey}.", cacheKey);
    }
    return userProfile;
})
.WithName("GetInMemoryCache");

// Redis cache API
app.MapGet("/rediscache", (IConnectionMultiplexer connectionMultiplexer, ILogger<Program> logger) =>
{
    var database = connectionMultiplexer.GetDatabase();
    string cacheKey = "userProfile:123";
    var cachedData = database.StringGet(cacheKey);

    UserProfile userProfile;
    if (cachedData.HasValue)
    {
        logger.LogInformation("Cache hit for key {CacheKey} in Redis.", cacheKey);
        userProfile = JsonSerializer.Deserialize<UserProfile>(cachedData);
    }
    else
    {
        logger.LogInformation("Cache miss for key {CacheKey} in Redis. Setting new value.", cacheKey);
        userProfile = new UserProfile { Name = "Jane Smith", Age = 28 };
        database.StringSet(cacheKey, JsonSerializer.Serialize(userProfile), TimeSpan.FromMinutes(5));
    }
    return userProfile;
})
.WithName("GetRedisCache");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class UserProfile
{
    public string Name { get; set; }
    public int Age { get; set; }
}