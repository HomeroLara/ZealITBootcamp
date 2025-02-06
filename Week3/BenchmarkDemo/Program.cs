using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Exporters.Json;

namespace BenchmarkDemo;

class Program
{
    static void Main(string[] args)
    {
        var summaryConfig = DetailedConfig();
        // BenchmarkRunner.Run<SimpleStringConcatenationBenchmarks>(summaryConfig);
        // BenchmarkRunner.Run<LinqVsLoopBenchmarks>(summaryConfig);
        // BenchmarkRunner.Run<JsonSerializationBenchmarks>(summaryConfig);
        BenchmarkRunner.Run<Md5VsSha256>(summaryConfig);
    }

    /// <summary>
    /// Configure Benchmark.Net to generate reports in Markdown, CSV, and JSON, 
    /// which makes the output more readable and shareable.
    /// After running the benchmark (dotnet run --configuration Release), you will get:
    /// A Markdown report: BenchmarkDotNet.Artifacts/results/*.md
    /// A CSV report: BenchmarkDotNet.Artifacts/results/*.csv
    /// A JSON report: BenchmarkDotNet.Artifacts/results/*.json
    /// </summary>
    /// <returns></returns>
    private static IConfig DetailedConfig()
    {
        return DefaultConfig.Instance
            .AddLogger(ConsoleLogger.Default)  // Default console logger
            .AddExporter(MarkdownExporter.GitHub) // Export as Markdown for GitHub
            .AddExporter(CsvExporter.Default) // Export as CSV
            .AddExporter(JsonExporter.Full); // Export as JSON
    }

    /// <summary>
    /// Enable Pretty Console Output (ASCII Table)
    /// </summary>
    /// <returns></returns>
    private static IConfig AsciiTableFormatConfig()
    {
        return DefaultConfig.Instance
            .AddExporter(DefaultExporters.AsciiDoc);
    }

    /// <summary>
    /// Enables graphical output, Benchmark.Net can generate R-based plots:
    /// </summary>
    /// <returns></returns>
    private static IConfig GraphFormatConfig()
    {
        return DefaultConfig.Instance
    .AddExporter(RPlotExporter.Default);
    }
}
