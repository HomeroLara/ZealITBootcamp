// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using LinqDemo;

Console.WriteLine("Hello, World!");

//BenchmarkRunner.Run<LinqBenchmark>();

var totalWatchTime = new List<(long trainingId, long sessionDurationInSeconds)>();

var completedTrainings = new List<CompletedTraining>()
{
    new(1, 1, 400),
    new(1, 2, 300),
    new(2, 1, 200),
    new(2, 2, 100),
    new(3, 1, 500),
    new(3, 2, 600),
};

Console.WriteLine($"Completed trainings start: {DateTime.Now}");
foreach (var completedTraining in completedTrainings)
{
    totalWatchTime.Add((completedTraining.trainingId, completedTraining.sessionDurationInSeconds));
}


Console.WriteLine($"Completed trainings end: {DateTime.Now}");

Console.WriteLine($"Completed trainings Aggregate By Start: {DateTime.Now}");
var courseWatchTime = totalWatchTime.AggregateBy(x => x.trainingId, _ => 0m,
    (second, item) => decimal.Add(second, item.sessionDurationInSeconds));

Console.WriteLine($"Completed trainings Aggregate By End: {DateTime.Now}");
foreach (var pair in courseWatchTime)
{
    Console.WriteLine($"TrainingId: {pair.Key}, Total Watch Time: {pair.Value}");
}

foreach (var pair in completedTrainings.CountBy(x => x.sessionId))
{
    Console.WriteLine($"TrainingId: {pair.Key}, Total: {pair.Value}");
}

Console.WriteLine($"Completed trainings: {DateTime.Now}");

record CompletedTraining(long trainingId, long sessionId, long sessionDurationInSeconds);