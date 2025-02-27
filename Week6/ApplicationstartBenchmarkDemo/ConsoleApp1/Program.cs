using System;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class ConsoleStartupBenchmark
{
    [Benchmark]
    public void ColdStart()
    {
        var stopwatch = Stopwatch.StartNew();

        // Simulating Cold Start
        var program = new SampleConsoleApp(); 

        stopwatch.Stop();
        Console.WriteLine($"Cold Start Time: {stopwatch.ElapsedMilliseconds} ms");
    }

    [Benchmark]
    public void WarmStart()
    {
        var stopwatch = Stopwatch.StartNew();

        // Simulating Warm Start (Instance Reuse)
        var program = SampleConsoleApp.Instance; 

        stopwatch.Stop();
        Console.WriteLine($"Warm Start Time: {stopwatch.ElapsedMilliseconds} ms");
    }
}

// Simulating a simple console application with a singleton pattern
public class SampleConsoleApp
{
    public static readonly SampleConsoleApp Instance = new SampleConsoleApp();

    public SampleConsoleApp()
    {
        // Simulating initialization delay
        System.Threading.Thread.Sleep(500);
    }
}

class Program
{
    static void Main() => BenchmarkRunner.Run<ConsoleStartupBenchmark>();
}