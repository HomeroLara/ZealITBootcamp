using System;
using System.Collections.Generic;
using System.Threading;
using GCDemo;

class Program
{
    static async Task Main()
    {
        Run();

        Console.WriteLine("\nLeaving 'Run'...");

        GC.Collect();
        GC.WaitForPendingFinalizers();
        // Console.WriteLine("Starting GC demonstration...\n");
        //
        // // Print initial memory info
        // PrintGCInfo();
        //
        // // Step 1: Allocate short-lived objects (Gen 0)
        // AllocateShortLivedObjects();
        // PrintGCInfo();
        //
        // // Step 2: Allocate long-lived objects (Gen 1 and Gen 2)
        // AllocateLongLivedObjects();
        // PrintGCInfo();
        //
        // // Step 3: Trigger explicit garbage collection
        // Console.WriteLine("\nForcing GC Collection...");
        // GC.Collect();
        // GC.WaitForPendingFinalizers();
        // PrintGCInfo();
        //
        // // Step 4: Force Full GC (including Gen 2)
        // Console.WriteLine("\nForcing Full GC Collection (Gen 2)...");
        // GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        // GC.WaitForPendingFinalizers();
        // PrintGCInfo();
        //
        // Console.WriteLine("\nGC demonstration complete!");
    }

    static void ShortLives (Person parent)
    {
        Person fred = new Person
        {
            Name = "Fred",
            ChildOne = new Person { Name = "Bamm-Bamm" }
        };

        parent.ChildTwo = fred.ChildOne;
    }

    static void Run()
    {
        Person wilma = new Person 
        { 
            Name = "Wilma",
            ChildOne = new Person { Name = "Pebbles"}
        };

        ShortLives(wilma);

        Console.WriteLine("Leaving 'ShortLives'...");

        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
    
    static void Run1()
    {
        //using
       var pmn = new PureManagedClass();
       pmn.StartWriting();
    }
    
    static void Run2()
    {
        //using
        var mc = new MixedClass();
        mc.StartWriting();
    }
    
    static void CreateObjects()
    {
        for (int i = 0; i < 5; i++)
        {
            var obj = new MyClass(); // Store reference in a local variable
            obj = null; // Explicitly remove reference to make it eligible for GC
        }
    }
    
    static void AllocateShortLivedObjects()
    {
        Console.WriteLine("Allocating short-lived objects (Gen 0)...");

        for (int i = 0; i < 10000; i++)
        {
            byte[] temp = new byte[1024]; // Allocate 1 KB
        }

        Console.WriteLine("Short-lived objects created.");
    }

    static void AllocateLongLivedObjects()
    {
        Console.WriteLine("Allocating long-lived objects (Gen 1 and Gen 2)...");

        List<byte[]> longLivedObjects = new List<byte[]>();

        for (int i = 0; i < 100; i++)
        {
            byte[] largeObject = new byte[1024 * 1024]; // Allocate 1 MB (Large Object Heap)
            longLivedObjects.Add(largeObject);
        }

        Console.WriteLine("Long-lived objects created.");
    }

    static void PrintGCInfo()
    {
        Console.WriteLine("\n--- Garbage Collection Info ---");
        Console.WriteLine($"Generation 0: {GC.CollectionCount(0)} collections");
        Console.WriteLine($"Generation 1: {GC.CollectionCount(1)} collections");
        Console.WriteLine($"Generation 2: {GC.CollectionCount(2)} collections");
        Console.WriteLine($"Total Memory: {GC.GetTotalMemory(false) / 1024.0:F2} KB");
        Console.WriteLine("--------------------------------\n");
    }
}
