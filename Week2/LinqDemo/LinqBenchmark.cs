using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace LinqDemo;


[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.Net90)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class LinqBenchmark
{
    private IEnumerable<int> _list = Enumerable.Range(1, 1000);
    private IEnumerable<int> _list2 = Enumerable.Range(1, 1000).ToList().Skip(500).Take(100);
    [Benchmark] public bool Any() => _list.Any(i => i == 1000);
    [Benchmark] public bool All() => _list.All(i => i >= 0);
    [Benchmark] public int Count() => _list.Count(i => i == 999);
    [Benchmark] public int First() => _list.First(i => i == 1000);
    [Benchmark] public int Single() => _list.Single(i => i == 555);
    [Benchmark] public int ListSkipTakeElementAt() => _list2.ElementAt(99);
}