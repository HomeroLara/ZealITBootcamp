using BenchmarkDotNet.Attributes;
using JM.LinqFaster;
using StructLinq;

namespace ConsoleApp1;

[MemoryDiagnoser]
public class LinqAltBenchmark
{
    private List<int> numbers;
    private int[] numberArray;

    [GlobalSetup]
    public void Setup()
    {
        numbers = Enumerable.Range(1, 10_000).ToList();
        numberArray = numbers.ToArray();
    }

    /// <summary>
    /// Filters the elements in a collection to select even numbers,
    /// doubles each selected value, and calculates the sum of the resulting collection.
    /// Issues: 
    /// Not allocation-free.
    /// Slow in tight loops due to enumerator/delegate overhead.
    /// Not vectorized or cache-optimized.
    /// </summary>
    /// <returns>
    /// The sum of the doubled values of even numbers in the collection.
    /// </returns>
    [Benchmark]
    public int StandardLinq() => numbers.Where(n => n % 2 == 0).Select(n => n * 2).Sum();

    /// <summary>
    /// Filters the elements in a collection to select even numbers,
    /// doubles each selected value, and calculates the sum of the resulting collection.
    /// Issues:
    /// Allocation-free, but slower in some cases due to StructLinq overhead.
    /// Optimized for struct enumerables but not fundamentally vectorized.
    /// May not scale effectively for large data sets without additional optimizations.
    /// </summary>
    /// <returns>
    /// The sum of the doubled values of even numbers in the collection using StructLinq.
    /// </returns>
    [Benchmark]
    public int StructLinq() => numbers.ToStructEnumerable().Where(n => n % 2 == 0).Select(n => n * 2).Sum();

    //[Benchmark]
    //public int LinqFaster() => numberArray.WhereSelectF(n => n % 2 == 0, n => n * 2).Sum();
}
