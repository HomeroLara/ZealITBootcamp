using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Engines;
using BenchmarkDemo.Models;

namespace BenchmarkDemo;

[MemoryDiagnoser] // tracks memory allocations
[Orderer(SummaryOrderPolicy.FastestToSlowest)] // order the summary of results from fastest to slowest
[RankColumn] // add a rank column to the summary table
public class IterationBenchmarks
{
    private List<Person> _people;

    [GlobalSetup]
    public void Setup()
    {
        var rand = new Random();
        var departments = new[] { "HR", "IT", "Finance", "Marketing", "Sales" };

        _people = new List<Person>();

        for (int i = 0; i < 10000; i++)
        {
            _people.Add(new Person(
                Id: i,
                FirstName: $"First{i}",
                LastName: $"Last{i}",
                BirthDate: DateTime.Now.AddDays(-rand.Next(365 * 20, 365 * 60)),
                Salary: (decimal)(rand.NextDouble() * 100000),
                IsActive: rand.Next(0, 2) == 1,
                Department: departments[rand.Next(departments.Length)],
                Email: $"someone_{i}@gmail.com",
                PhoneNumber: $"123-456-789{i % 10}"
            ));
        }
    }
    
    [Benchmark(Baseline = true)]
    public decimal ForLoop_TotalSalary()
    {
        decimal total = 0;
        for (int i = 0; i < _people.Count; i++)
        {
            total += _people[i].Salary;
        }
        return total;
    }

    [Benchmark]
    public decimal ForEachLoop_TotalSalary()
    {
        decimal total = 0;
        foreach (var person in _people)
        {
            total += person.Salary;
        }
        return total;
    }

    [Benchmark]
    public decimal LinqSum_TotalSalary()
    {
        return _people.Sum(p => p.Salary);
    }

    [Benchmark]
    public List<string> Linq_Select_ToList_FirstNames()
    {
        return _people.Select(p => p.FirstName).ToList();
    }

    [Benchmark]
    public List<string> ForEach_FirstNames()
    {
        var names = new List<string>(_people.Count);
        foreach (var person in _people)
        {
            names.Add(person.FirstName);
        }
        return names;
    }

    [Benchmark]
    public List<string> ForLoop_FirstNames()
    {
        var names = new List<string>(_people.Count);
        for (int i = 0; i < _people.Count; i++)
        {
            names.Add(_people[i].FirstName);
        }
        return names;
    }
}