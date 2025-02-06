using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Engines;

namespace BenchmarkDemo;
[SimpleJob(RunStrategy.ColdStart, launchCount: 50)]
[MemoryDiagnoser] // tracks memory allocations
[Orderer(SummaryOrderPolicy.FastestToSlowest)] // order the summary of results from fastest to slowest
[RankColumn] // add a rank column to the summary table
public class ArrayvsSpanBenchmarks
{
    private int[] _numbers;

    [GlobalSetup] // Runs once before benchmarks to initialize data
    public void Setup()
    {
        // The array is pre - allocated in GlobalSetup, so it does not count as a benchmarked allocation.
        _numbers = Enumerable.Range(1, 1000).ToArray();
    }

    [Benchmark]
    public int SumWithArray()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            sum += _numbers[i];
        }
        return sum;
    }

    [Benchmark]
    public int SumWithSpan()
    {
        int sum = 0;
        Span<int> span = _numbers; // Converts array to Span<int>
        for (int i = 0; i < span.Length; i++)
        {
            sum += span[i];
        }
        return sum;
    }
}
