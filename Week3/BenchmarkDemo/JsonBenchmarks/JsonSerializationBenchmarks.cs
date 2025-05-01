using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;

namespace BenchmarkDemo;

[MemoryDiagnoser] // Tracks memory allocation
[Orderer(SummaryOrderPolicy.FastestToSlowest)] // Orders results from fastest to slowest
[RankColumn] // Adds ranking to results
public class JsonSerializationBenchmarks
{
    private readonly SampleData sampleObject;
    private readonly string jsonString_SystemTextJson;
    private readonly string jsonString_Newtonsoft;

    public JsonSerializationBenchmarks()
    {
        sampleObject = new SampleData
        {
            Id = 1,
            Name = "Juan Doe",
            Age = 36,
            Email = "juandoe@example.com",
            Interests = new List<string> { "Coding", "Gaming", "Music" }
        };

        jsonString_SystemTextJson = System.Text.Json.JsonSerializer.Serialize(sampleObject);
        jsonString_Newtonsoft = Newtonsoft.Json.JsonConvert.SerializeObject(sampleObject);
    }

    [Benchmark]
    public string SystemTextJson_Serialize() => System.Text.Json.JsonSerializer.Serialize(sampleObject);

    [Benchmark]
    public string NewtonsoftJson_Serialize() => Newtonsoft.Json.JsonConvert.SerializeObject(sampleObject);

    [Benchmark]
    public SampleData SystemTextJson_Deserialize() => System.Text.Json.JsonSerializer.Deserialize<SampleData>(jsonString_SystemTextJson);

    [Benchmark]
    public SampleData NewtonsoftJson_Deserialize() => Newtonsoft.Json.JsonConvert.DeserializeObject<SampleData>(jsonString_Newtonsoft);
}

public class SampleData
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Email { get; set; }
    public List<string>? Interests { get; set; }
}