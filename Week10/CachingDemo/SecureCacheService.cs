using Microsoft.Extensions.Caching.Memory;

namespace CachingDemo;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class CryptoHelper
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF"); // 32 chars = 32 bytes (AES-256)
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("ABCDEF0123456789"); // 16 chars = 16 bytes


    public static string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var memStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using var writer = new StreamWriter(cryptoStream);
        writer.Write(plainText);
        writer.Flush();
        cryptoStream.FlushFinalBlock();

        return Convert.ToBase64String(memStream.ToArray());
    }

    public static string Decrypt(string encrypted)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var memStream = new MemoryStream(Convert.FromBase64String(encrypted));
        using var cryptoStream = new CryptoStream(memStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream);

        return reader.ReadToEnd();
    }
}


public class SecureCacheService
{
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());

    /// <summary>
    /// Securely cache encrypted data for a user
    /// </summary>
    public void CacheSensitiveData(string userId, string data)
    {
        var encryptedData = CryptoHelper.Encrypt(data);
        var key = $"UserSensitive_{userId}";

        _cache.Set(key, encryptedData, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            Size = 1
        });

        Console.WriteLine("🔐 Sensitive data encrypted and cached.");
    }

    /// <summary>
    /// Fetch sensitive data with access control check
    /// </summary>
    public string GetSensitiveData(string userId, string requestingUserId)
    {
        if (userId != requestingUserId)
        {
            Console.WriteLine("🚫 Unauthorized access attempt detected.");
            return null;
        }

        var key = $"UserSensitive_{userId}";

        if (_cache.TryGetValue(key, out string encryptedData))
        {
            var decrypted = CryptoHelper.Decrypt(encryptedData);
            Console.WriteLine("✅ Decrypted sensitive data retrieved.");
            return decrypted;
        }

        Console.WriteLine("❌ Sensitive data not found.");
        return null;
    }

    /// <summary>
    /// ❌ Bad example: Caches plain sensitive data with no protection
    /// </summary>
    public void CachePlainSensitiveData_Insecure(string userId, string data)
    {
        var key = $"PlainSensitive_{userId}";
        _cache.Set(key, data);

        Console.WriteLine("❌ Insecure: Sensitive data cached in plain text.");
    }
}
