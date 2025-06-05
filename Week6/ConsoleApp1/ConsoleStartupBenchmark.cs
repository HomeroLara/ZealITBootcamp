using System.Diagnostics;
using BenchmarkDotNet.Attributes;

public class ConsoleStartupBenchmark
{
    [Benchmark]
    [IterationCount(100)]  // Limits the number of runs
    public void ColdStart()
    {
        var stopwatch = Stopwatch.StartNew();

        // Simulating Cold Start
        var program = new SampleConsoleApp(); 

        stopwatch.Stop();
        Console.WriteLine($"Cold Start Time: {stopwatch.ElapsedMilliseconds} ms");
    }

    [Benchmark]
    [IterationCount(100)]  // Limits the number of runs
    public void WarmStart()
    {
        var stopwatch = Stopwatch.StartNew();

        // Simulating Warm Start (Instance Reuse)
        var program = SampleConsoleApp.Instance; 

        stopwatch.Stop();
        Console.WriteLine($"Warm Start Time: {stopwatch.ElapsedMilliseconds} ms");
    }
}