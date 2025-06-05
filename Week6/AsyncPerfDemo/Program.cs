// See https://aka.ms/new-console-template for more information

using AsyncPerfDemo.Benchmarks;
using BenchmarkDotNet.Running;

Console.WriteLine("Hello, World!");
BenchmarkRunner.Run<AsyncPatterns>();