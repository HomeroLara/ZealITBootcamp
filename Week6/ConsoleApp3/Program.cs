// See https://aka.ms/new-console-template for more information

using ConsoleApp3;

Console.WriteLine("Hello, World!");

var csvReader = new CsvReader();
await foreach (var row in csvReader.ReadCsvAsync("largefile.csv"))
{
    Console.WriteLine(string.Join(", ", row));
}

// foreach (var row in csvReader.ReadCsv("largefile.csv"))
// {
//     Console.WriteLine(string.Join(", ", row));
// }

Console.ReadLine();