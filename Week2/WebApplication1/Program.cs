using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IService1, Service1>();
builder.Services.AddSingleton<IService2, Service2>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (IService1 service1) =>
    {
        service1.DoSomething();
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

// Add a new endpoint to demonstrate JIT compilation
app.MapGet("/jitdemo", () =>
    {
        if (OperatingSystem.IsWindows())
            Console.WriteLine("Running on Windows");
        else if (OperatingSystem.IsLinux())
            Console.WriteLine("Running on Linux");
        else if (OperatingSystem.IsMacOS())
            Console.WriteLine("Running on macOS");
        

        
        const int numSteps = 100_000_000; // Increase this for more complexity
        const double lowerBound = 0.0;
        const double upperBound = 1.0;

        // Warm-up phase
        Console.WriteLine("Warming up...");
        for (int i = 0; i < 5; i++)
        {
            PerformNumericalIntegration(lowerBound, upperBound, numSteps);
        }

        // Force garbage collection
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Measure the first run
        Stopwatch sw = new Stopwatch();
        sw.Start();
        double result = PerformNumericalIntegration(lowerBound, upperBound, numSteps);
        sw.Stop();
        var firstRunTime = sw.ElapsedMilliseconds;

        // Force garbage collection again
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Measure the second run
        sw.Restart();
        result = PerformNumericalIntegration(lowerBound, upperBound, numSteps);
        sw.Stop();
        var secondRunTime = sw.ElapsedMilliseconds;

        return new
        {
            FirstRunTime = firstRunTime,
            SecondRunTime = secondRunTime,
            Result = result
        };
    })
    .WithName("GetJitDemo")
    .WithOpenApi();


app.Run();

static double PerformNumericalIntegration(double lowerBound, double upperBound, int numSteps)
{
    double stepSize = (upperBound - lowerBound) / numSteps;
    double sum = 0.0;

    Parallel.For(0, numSteps, i =>
    {
        double x = lowerBound + i * stepSize;
        double y = Math.Exp(-x * x); // Example curve: e^(-x^2)
        double area = y * stepSize;
        AddToSum(ref sum, area); // Accumulate results safely
    });

    return sum;
}

// Thread-safe addition
static void AddToSum(ref double sum, double value)
{
    lock ("sum_lock")
    {
        sum += value;
    }
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}