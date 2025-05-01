using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using BenchmarkDemo.Models; // Assuming Person record is here

namespace BenchmarkDemo;

// Each benchmark runs in a fresh process, from a “cold” start (nothing cached, no JIT warmup carried over)
// This is useful for simulating a real-world scenario where the code is run for the first time
// Good for startup time-sensitive scenarios
// launchCount: 50 means that the benchmark will run 50 times in a row
[SimpleJob(RunStrategy.ColdStart, launchCount: 50)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class LinqVsLoopBenchmarks
{
    private List<Person> _people;

    [Params(10000)] // adjustable dataset size
    public int PersonCount;

    [GlobalSetup]
    public void Setup()
    {
        var rand = new Random();
        _people = new List<Person>(PersonCount);

        for (int i = 0; i < PersonCount; i++)
        {
            _people.Add(new Person(
                Id: i,
                FirstName: $"First{i}",
                LastName: $"Last{i}",
                BirthDate: DateTime.Now.AddDays(-rand.Next(365 * 20, 365 * 60)),
                Salary: (decimal)(rand.NextDouble() * 100_000),
                IsActive: rand.Next(0, 2) == 1,
                Department: "IT",
                Email: $"person{i}@example.com",
                PhoneNumber: $"+1-555-{1000 + i}")
            );
        }
    }

    [Benchmark]
    public List<Person> LinqWhereToList() =>
        _people.Where(p => p.IsActive && p.Salary >= 50000).ToList();

    [Benchmark]
    public Person[] LinqWhereSelectToArray() =>
        _people.Where(p => p.IsActive && p.Salary >= 50000)
               .Select(p => new Person(
                   p.Id,
                   p.FirstName,
                   p.LastName,
                   p.BirthDate,
                   p.Salary,
                   p.IsActive,
                   p.Department,
                   p.Email,
                   p.PhoneNumber))
               .ToArray();

    [Benchmark]
    public List<Person> ForEachLoop()
    {
        var result = new List<Person>();
        foreach (var p in _people)
        {
            if (p.IsActive && p.Salary >= 50000)
                result.Add(p);
        }
        return result;
    }

    [Benchmark]
    public List<Person> ForLoop()
    {
        var result = new List<Person>();
        for (int i = 0; i < _people.Count; i++)
        {
            var p = _people[i];
            if (p.IsActive && p.Salary >= 50000)
                result.Add(p);
        }
        return result;
    }
}
