using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

namespace BenchmarkDemo;

[SimpleJob(RunStrategy.ColdStart, launchCount:20)]
[MemoryDiagnoser] // tracks memory allocations
[Orderer(SummaryOrderPolicy.FastestToSlowest)] // order the summary of results from fastest to slowest
[RankColumn] // add a rank column to the summary table
public class SimpleStringConcatenationBenchmarks
{
    private const string _firstName = "Jaun";
    private const string _lastName = "Doe";

    [Benchmark]
    public string FullName_UsingPlusOperator() => _firstName + " " + _lastName;

    [Benchmark]
    public string FullName_UsingStringFormat() => string.Format("{0} {1}", _firstName, _lastName);

    [Benchmark]
    public string FullName_UsingStringConcatMethod() => string.Concat(_firstName, " ", _lastName);

    [Benchmark]
    public string FullName_UsingStringBuilder()
    {
        var sb = new StringBuilder();
        sb.Append(_firstName);
        sb.Append(" ");
        sb.Append(_lastName);
        return sb.ToString();
    }

    [Benchmark]
    public string UsingStringJoin() => string.Join(" ", _firstName, _lastName);

    [Benchmark]
    public string UsingInterpolation() => $"{_firstName} {_lastName}";

}