using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Garbage Collection Demonstration");
        Console.WriteLine($"Initial Memory: {GC.GetTotalMemory(false) / 1024.0:F2} KB");

        // Step 1: Allocate a large number of objects
        Console.WriteLine("\nAllocating memory...");
        CreateObjects();

        Console.WriteLine($"Memory after allocation: {GC.GetTotalMemory(false) / 1024.0:F2} KB");

        // Step 2: Collect garbage in Generation 0
        Console.WriteLine("\nForcing Generation 0 GC...");
        GC.Collect(0);
        GC.WaitForPendingFinalizers();
        Console.WriteLine($"Memory after Gen 0 GC: {GC.GetTotalMemory(false) / 1024.0:F2} KB");

        // Step 3: Collect garbage in Generation 1
        Console.WriteLine("\nForcing Generation 1 GC...");
        GC.Collect(1);
        GC.WaitForPendingFinalizers();
        Console.WriteLine($"Memory after Gen 1 GC: {GC.GetTotalMemory(false) / 1024.0:F2} KB");

        // Step 4: Collect garbage in Generation 2
        Console.WriteLine("\nForcing Full GC (Generation 2)...");
        GC.Collect(2);
        GC.WaitForPendingFinalizers();
        Console.WriteLine($"Memory after Full GC: {GC.GetTotalMemory(false) / 1024.0:F2} KB");

        Console.WriteLine("\nGarbage Collection Stats:");
        Console.WriteLine($"Gen 0 collections: {GC.CollectionCount(0)}");
        Console.WriteLine($"Gen 1 collections: {GC.CollectionCount(1)}");
        Console.WriteLine($"Gen 2 collections: {GC.CollectionCount(2)}");
    }

    static void CreateObjects()
    {
        // Create many small objects that will initially be in Gen 0
        for (int i = 0; i < 100_000; i++)
        {
            var smallObj = new byte[1024]; // 1 KB per object
        }

        // Create a large object that will be allocated on the Large Object Heap (LOH)
        var largeObj = new byte[10 * 1024 * 1024]; // 10 MB object
    }
}
