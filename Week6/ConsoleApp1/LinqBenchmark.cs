using BenchmarkDotNet.Attributes;

namespace ConsoleApp1;

[MemoryDiagnoser]
public class LinqBenchmark
{
    private IEnumerable<int> numbers;

    [GlobalSetup]
    public void Setup()
    {
        numbers = Enumerable.Range(1, 10000);
    }

    [Benchmark]
    //[IterationCount(500)] 
    public void DeferredExecution()
    {
        var filtered = numbers.Where(n => n % 2 == 0); // Deferred
        foreach (var i in filtered)
        {
            DoSomething(i);
        }
        foreach (var i in filtered) // Re-evaluated
        {
            DoSomething(i);
        }
    }

    [Benchmark]
    //[IterationCount(500)] 
    public void MaterializedExecution()
    {
        var filtered = numbers.Where(n => n % 2 == 0).ToList(); // Materialized once
        foreach (var i in filtered)
        {
            DoSomething(i);
        }
        foreach (var i in filtered) // No extra cost
        {
            DoSomething(i);
        }
    }

    private void DoSomething(int i)
    {
        // Simulate light work
        var x = i * 2;
    }
}