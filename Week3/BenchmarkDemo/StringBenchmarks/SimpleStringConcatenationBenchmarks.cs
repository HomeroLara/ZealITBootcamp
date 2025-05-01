using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDemo.Models; 

namespace BenchmarkDemo;

[MemoryDiagnoser] // tracks memory allocations
[Orderer(SummaryOrderPolicy.FastestToSlowest)] // order the summary of results from fastest to slowest
[RankColumn] // add a rank column to the summary table
public class SimpleStringConcatenationBenchmarks
{
    private List<Person> _people;

    [Params(10000)] 
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
    public string UsingPlusOperator()
    {
        var result = string.Empty;
        foreach (var p in _people)
        {
            result += p.Id + "," + p.FirstName + "," + p.LastName + "," + p.BirthDate.ToString("yyyy-MM-dd") + "," +
                      p.Salary + "," + p.IsActive + "," + p.Department + "," + p.Email + "," + p.PhoneNumber + Environment.NewLine;
        }
        return result;
    }

    [Benchmark]
    public string UsingStringConcat()
    {
        var result = string.Empty;
        foreach (var p in _people)
        {
            result = string.Concat(result,
                p.Id, ",", p.FirstName, ",", p.LastName, ",", p.BirthDate.ToString("yyyy-MM-dd"), ",",
                p.Salary, ",", p.IsActive, ",", p.Department, ",", p.Email, ",", p.PhoneNumber, Environment.NewLine);
        }
        return result;
    }

    [Benchmark]
    public string UsingStringFormat()
    {
        // var result = string.Empty;
        var sb = new StringBuilder();
        for (int i = 0; i < _people.Count - 1; i++)    
        {
            // result += string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}{9}",
            //     _people[i].Id, 
            //     _people[i].FirstName, 
            //     _people[i].LastName, 
            //     _people[i].BirthDate.ToString("yyyy-MM-dd"),
            //     _people[i].Salary, 
            //     _people[i].IsActive, 
            //     _people[i].Department, 
            //     _people[i].Email, 
            //     _people[i].PhoneNumber, 
            //     Environment.NewLine);
            
            sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8}{9}",
                _people[i].Id, 
                _people[i].FirstName, 
                _people[i].LastName, 
                _people[i].BirthDate.ToString("yyyy-MM-dd"),
                _people[i].Salary, 
                _people[i].IsActive, 
                _people[i].Department, 
                _people[i].Email, 
                _people[i].PhoneNumber, 
                Environment.NewLine);
        }
        // return result;
        return sb.ToString();
    }

    [Benchmark]
    // wins both performance and memory efficiency 
    public string UsingStringBuilder()
    {
        var sb = new StringBuilder();
        foreach (var p in _people)
        {
            sb.Append(p.Id).Append(',')
              .Append(p.FirstName).Append(',')
              .Append(p.LastName).Append(',')
              .Append(p.BirthDate.ToString("yyyy-MM-dd")).Append(',')
              .Append(p.Salary).Append(',')
              .Append(p.IsActive).Append(',')
              .Append(p.Department).Append(',')
              .Append(p.Email).Append(',')
              .Append(p.PhoneNumber).AppendLine();
        }
        return sb.ToString();
    }

    [Benchmark]
    public string UsingInterpolation()
    {
        string result = string.Empty;
        foreach (var p in _people)
        {
            result += $"{p.Id},{p.FirstName},{p.LastName},{p.BirthDate:yyyy-MM-dd},{p.Salary},{p.IsActive},{p.Department},{p.Email},{p.PhoneNumber}{Environment.NewLine}";
        }
        return result;
    }
}