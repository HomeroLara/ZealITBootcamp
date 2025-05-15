using System.Diagnostics;
using ZealITMobile.Models;

namespace ZealITMobile.Utlities;

public static class MemoryUtility
{
    /// <summary>
    /// This method is designed to provide a way to measure the approximate
    /// managed memory usage of a .NET application, at a specific point in its execution,
    /// log this information, and optionally try to get a more stable reading by forcing garbage collection.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="forceFullCollection"></param>
    /// <returns></returns>
    public static MemoryLog GetMemoryReadingAndLog(string description, bool forceFullCollection = true)
    {
        if (forceFullCollection)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect(); 
        }
        
        // GC.GetTotalMemory(true), you are signaling to the method that it should try to ensure a full
        // garbage collection occurs before the memory is measured.
        // This can help to get a more accurate reading of the memory usage at that point in time.
        var memory = GC.GetTotalMemory(forceFullCollection);
        var logMessage = $"{description}: {memory / 1024:N0} KB";
        
        var memoryLog = new MemoryLog
        {
            Memory = memory,
            Description = logMessage
        };
        
        Console.WriteLine(logMessage);
        Debug.WriteLine(logMessage);
        
        return memoryLog;
    }
}