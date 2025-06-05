using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

public class ProductService
{
    private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private const int CacheDurationInMinutes = 5;

    /// <summary>
    /// Cache-Aside Pattern Example
    /// </summary>
    public string GetProductDetails(string productId)
    {
        string cacheKey = $"Product_{productId}";
        string product = _cache.Get(cacheKey) as string;

        if (product != null)
        {
            Console.WriteLine("Cache Hit");
            return product;
        }

        Console.WriteLine("Cache Miss - Fetching from DB");
        product = FetchProductFromDatabase(productId);

        _cache.Set(cacheKey, product, DateTimeOffset.Now.AddMinutes(CacheDurationInMinutes));
        return product;
    }

    private string FetchProductFromDatabase(string productId)
    {
        // Simulate DB fetch
        return $"Product {productId} - Price: $100 (fetched at {DateTime.Now})";
    }

    private void UpdateProductInDatabase(string productId, string newData)
    {
        Console.WriteLine($"DB Updated for {productId}: {newData}");
    }

    /// <summary>
    /// Write-Through Pattern Example
    /// </summary>
    public void UpdateProduct_WriteThrough(string productId, string updatedProductData)
    {
        UpdateProductInDatabase(productId, updatedProductData);

        string cacheKey = $"Product_{productId}";
        _cache.Set(cacheKey, updatedProductData, DateTimeOffset.Now.AddMinutes(CacheDurationInMinutes));

        Console.WriteLine("Write-Through: DB and Cache updated");
    }

    /// <summary>
    /// Write-Behind Pattern Example
    /// </summary>
    public async Task UpdateProduct_WriteBehindAsync(string productId, string updatedProductData)
    {
        string cacheKey = $"Product_{productId}";
        _cache.Set(cacheKey, updatedProductData, DateTimeOffset.Now.AddMinutes(CacheDurationInMinutes));

        Console.WriteLine("Write-Behind: Cache updated, DB will update shortly...");

        await Task.Delay(2000); // simulate async lag
        UpdateProductInDatabase(productId, updatedProductData);
    }

    /// <summary>
    /// ❌ Pitfall: Over-Updating Cache for Every Minor Change
    /// </summary>
    public void UpdateProduct_TooFrequentCacheUpdates(string productId, string updatedProductData)
    {
        UpdateProductInDatabase(productId, updatedProductData);

        string cacheKey = $"Product_{productId}";
        _cache.Set(cacheKey, updatedProductData, DateTimeOffset.Now.AddMinutes(CacheDurationInMinutes));

        Console.WriteLine("BAD: Updated cache even for minor change");
    }

    /// <summary>
    /// ✅ Fix: Only Update Cache When Change is Significant
    /// </summary>
    public void UpdateProduct_SmartCacheUpdate(string productId, string updatedProductData, string originalData)
    {
        if (!IsSignificantChange(originalData, updatedProductData))
        {
            Console.WriteLine("Minor change detected - DB updated, cache untouched");
            UpdateProductInDatabase(productId, updatedProductData);
            return;
        }

        UpdateProductInDatabase(productId, updatedProductData);

        string cacheKey = $"Product_{productId}";
        _cache.Set(cacheKey, updatedProductData, DateTimeOffset.Now.AddMinutes(CacheDurationInMinutes));

        Console.WriteLine("Smart Update: DB and cache updated only on significant change");
    }

    private bool IsSignificantChange(string oldData, string newData)
    {
        // Dummy check: in real scenario, parse and compare meaningful fields
        return !oldData.Equals(newData, StringComparison.OrdinalIgnoreCase);
    }
}
