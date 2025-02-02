using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Order;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Engines;

namespace BenchmarkDemo;

[MemoryDiagnoser] // tracks memory allocations
[Orderer(SummaryOrderPolicy.FastestToSlowest)] // order the summary of results from fastest to slowest
[RankColumn] // add a rank column to the summary table

public class LinqVsLoopBenchmarks
{
    private List<int> _numbers;
    private readonly Consumer consumer = new Consumer(); // Required for .Consume()

    [GlobalSetup]
    public void Setup()
    {
        var rand = new Random();
        _numbers = Enumerable.Range(1, 10000).Select(_ => rand.Next(1, 10000)).ToList();
    }

    [Benchmark]
    public List<int> LinqWhereToList() => _numbers.Where(x => x % 2 == 0).ToList();

    [Benchmark]
    public int[] LinqWhereSelectToArray() => _numbers.Where(x => x % 2 == 0).Select(x => x * 2).ToArray();

    [Benchmark]
    public List<int> ForEachLoop()
    {
        var result = new List<int>();
        foreach (var num in _numbers)
        {
            if (num % 2 == 0)
                result.Add(num);
        }
        return result;
    }

    [Benchmark]
    public List<int> ForLoop()
    {
        var result = new List<int>();
        for (int i = 0; i < _numbers.Count; i++)
        {
            if (_numbers[i] % 2 == 0)
                result.Add(_numbers[i]);
        }
        return result;
    }
}