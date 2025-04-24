using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var filePath = "largefile.csv"; // Replace with your CSV path
        var channel = Channel.CreateUnbounded<string>();

        var producer = ReadCsvAsync(filePath, channel.Writer);
        var consumer = ProcessCsvLinesAsync(channel.Reader);

        await Task.WhenAll(producer, consumer);
    }

    // Producer: Reads the file line-by-line and writes to the channel
    static async Task ReadCsvAsync(string path, ChannelWriter<string> writer)
    {
        try
        {
            using var reader = new StreamReader(path);
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                Console.WriteLine($"Reading line: {line}");
                await writer.WriteAsync(line);
                // Simulate some processing time
                await Task.Delay(100);
            }
        }
        finally
        {
            Console.WriteLine("Finished reading file.");
            writer.Complete();
        }
    }

    // Consumer: Processes each CSV line
    static async Task ProcessCsvLinesAsync(ChannelReader<string> reader)
    {
        await foreach (var line in reader.ReadAllAsync())
        {
            var fields = line.Split(','); // Basic CSV split; consider a CSV parser for robustness
            Console.WriteLine($"Processed Row: {string.Join(" | ", fields)}");

            // Simulate some processing time
            await Task.Delay(200);
        }

        Console.WriteLine("Done processing CSV.");
    }
}