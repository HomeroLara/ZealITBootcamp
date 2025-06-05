using System.Diagnostics;

namespace ConsoleApp1;

public static class AdaptiveGcDemo
{
    public static void Run()
    {
        Console.WriteLine("Starting memory pressure simulation...");
        var list = new List<byte[]>();
        var sw = Stopwatch.StartNew();

        while (sw.Elapsed.TotalSeconds < 60) // Run for 60 seconds
        {
            for (int i = 0; i < 100; i++)
            {
                // Allocate ~1MB chunks
                list.Add(new byte[1024 * 1024]);
            }

            if (list.Count > 200)
            {
                list.RemoveRange(0, 100); // Free up space
            }

            Thread.Sleep(50); // Slight pause to simulate workload
        }

        Console.WriteLine("Done.");
    }
}