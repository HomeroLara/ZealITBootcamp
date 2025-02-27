using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // Console.WriteLine("Fetching data...");
        // await foreach (var item in GetLargeDataAsync())
        // {
        //     Console.WriteLine(item);
        // }
        
        // string json = """
        //               [
        //                   { "Id": 1, "Name": "Alice", "Age": 25 },
        //                   { "Id": 2, "Name": "Bob", "Age": 30 },
        //                   { "Id": 3, "Name": "Charlie", "Age": 35 }
        //               ]
        //               """;
        //
        // using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        //
        // // ✅ Stream JSON data asynchronously without loading everything into memory
        // await foreach (var person in JsonSerializer.DeserializeAsyncEnumerable<Person>(stream))
        // {
        //     Console.WriteLine($"{person.Id}: {person.Name}, {person.Age} years old");
        // }
        
        using var client = new HttpClient();
        var response = await client.GetAsync("http://localhost:5125/streaming-people", HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

      
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultBufferSize = 128
        };

        using var stream = await response.Content.ReadAsStreamAsync();
        await foreach (var person in JsonSerializer.DeserializeAsyncEnumerable<Person>(stream, options))
        {
            Console.WriteLine($"{person.Id}: {person.Name}, {person.Age} years old");
        }
        
    }

    static async IAsyncEnumerable<int> GetLargeDataAsync()
    {
        for (int i = 1; i <= 100; i++)
        {
            await Task.Delay(500); // Simulating data fetching delay
            yield return i;
        }
    }
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}