using BenchmarkDotNet.Attributes;

namespace ConsoleApp1;

[MemoryDiagnoser]
public class HotLoopBenchmark
{
    private List<int> numbers;

    [GlobalSetup]
    public void Setup()
    {
        numbers = Enumerable.Range(1, 10_000).ToList();
    }

    /// <summary>
    /// Filters the list of integers to include only even numbers, multiplies each by 2, and computes the sum of the resulting values using LINQ.
    /// Allocates intermediate iterators for Where and Select.
    /// Less control over branching and memory access.
    /// </summary>
    /// <returns>The sum of all even numbers in the list multiplied by 2.</returns>
    [Benchmark]
    public int UsingLinq() => numbers
        .Where(n => n % 2 == 0)
        .Select(n => n * 2)
        .Sum();


    /// <summary>
    /// Iterates through the list of integers using a loop to filter only even numbers, multiplies each by 2, and computes the sum of the resulting values.
    /// No iterator allocation.
    /// No delegate invocation.
    /// Straight-line code that JIT can optimize better.
    /// WHEN TO USE: Inside tight loops or high-frequency methods.
    ///  When every microsecond counts, e.g., in real-time systems or request-intensive APIs.
    ///  When LINQ creates too many temporary objects.
    /// </summary>
    /// <returns>The sum of all even numbers in the list multiplied by 2.</returns>
    [Benchmark]
    public int UsingLoop()
    {
        int sum = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] % 2 == 0)
                sum += numbers[i] * 2;
        }
        return sum;
    }
}