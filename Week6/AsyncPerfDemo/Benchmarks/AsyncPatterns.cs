using BenchmarkDotNet.Attributes;

namespace AsyncPerfDemo.Benchmarks;

[MemoryDiagnoser]
public class AsyncPatterns
{
    private readonly string testFilePath = "test.txt";

    public AsyncPatterns()
    {
        // Ensure file exists
        //File.WriteAllText(testFilePath, "Sample text content for benchmarking...");
    }

    [Benchmark]
    public string Sync_ReadFile()
    {
        return File.ReadAllText(testFilePath);
    }

    [Benchmark]
    public async Task<string> ProperAsync_ReadFile()
    {
        return await File.ReadAllTextAsync(testFilePath).ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<string> BadAsync_WrappedInTaskRun()
    {
        return await Task.Run(() => File.ReadAllText(testFilePath));
    }

    [Benchmark]
    public Task<string> TrivialAsync_WithFromResult()
    {
        return Task.FromResult("Hello, world!");
    }

    [Benchmark]
    public async Task ContextCaptured()
    {
        await Task.Delay(50); // Captures synchronization context
    }

    [Benchmark]
    public async Task NoContextCaptured()
    {
        await Task.Delay(50).ConfigureAwait(false);
    }
}