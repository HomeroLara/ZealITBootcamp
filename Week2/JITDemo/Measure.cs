using System.Diagnostics;

namespace JITDemo;

public static class Utils
{
    public static void Measure(Action action, int iterations)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));
        if (iterations <= 0)
            throw new ArgumentOutOfRangeException(nameof(iterations), "Iterations must be greater than zero.");

        // Warm-up
        //action();

        // Measure
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            action();
        }
        stopwatch.Stop();

        Console.WriteLine($"Total time for {iterations} iterations: {stopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine($"Average time per iteration: {stopwatch.ElapsedMilliseconds / (double)iterations} ms");
    }
}