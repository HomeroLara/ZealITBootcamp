using BenchmarkDotNet.Attributes;

namespace ConsoleApp1;

[MemoryDiagnoser]
public class CountByBenchmark
{
    private List<int> _data;

    [GlobalSetup]
    public void Setup()
    {
        // Simulate 1 million items with repeated keys
        _data = Enumerable.Range(0, 1_000_000)
            .Select(i => i % 1000)
            .ToList();
    }

    [Benchmark(Baseline = true)]
    public Dictionary<int, int> GroupByAndCount()
    {
        return _data
            .GroupBy(x => x)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    [Benchmark]
    public Dictionary<int, int> CountBy() =>
        _data.CountBy(x => x).ToDictionary();
}