using System;
using BenchmarkDotNet.Running;
using ConsoleApp1;
using StructLinq;

class Program
{
    static void Main()
    {
        //Console.WriteLine($"PID: {Environment.ProcessId}");
        //AdaptiveGcDemo.Run();
        BenchmarkRunner.Run<CountByBenchmark>();
        //BenchmarkRunner.Run<ConsoleStartupBenchmark>();
    }
}