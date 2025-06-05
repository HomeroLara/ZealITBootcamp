namespace ConsoleApp3;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class CsvReader
{
    public async IAsyncEnumerable<string[]> ReadCsvAsync(string filePath, char delimiter = ',')
    {
        using var reader = new StreamReader(filePath);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line != null)
            {
                yield return line.Split(delimiter);
            }
        }
    }
    
    public IEnumerable<string[]> ReadCsv(string filePath, char delimiter = ',')
    {
        using var reader = new StreamReader(filePath);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line != null)
            {
                yield return line.Split(delimiter);
            }
        }
    }
}