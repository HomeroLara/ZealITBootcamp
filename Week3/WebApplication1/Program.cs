using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeScopes = true;
    options.ParseStateValues = true;
    options.AddConsoleExporter(); // Optional, to see logs in console
});

// Custom metrics for the application
var  weatherforecastMeter = new Meter("webAppication1.demo", "1.0.0");
var countweatherforecastRequests = weatherforecastMeter.CreateCounter<int>(
    "webAppication1.weatherforecast.count", 
    description: "Counts the number of weatherforecast requests in webAppication1"
);
// Custom ActivitySource for the application
var weatherforecastActivitySource = new ActivitySource("webAppication1.Weatherforecast");



var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
var otel = builder.Services.AddOpenTelemetry();

// Configure OpenTelemetry Resources with the application name
otel.ConfigureResource(resource => resource
    .AddService(serviceName: builder.Environment.ApplicationName));

// Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
otel.WithMetrics(metrics => metrics
    // Metrics provider from OpenTelemetry
    .AddAspNetCoreInstrumentation()
    .AddMeter(weatherforecastMeter.Name)
    // Metrics provides by ASP.NET Core in .NET 8
    .AddMeter("Microsoft.AspNetCore.Hosting")
    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
    // Metrics provided by System.Net libraries
    .AddMeter("System.Net.Http")
    .AddMeter("System.Net.NameResolution")
    .AddPrometheusExporter());

// Add Tracing for ASP.NET Core and our custom ActivitySource and export to Jaeger
otel.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();
    tracing.AddSource(weatherforecastActivitySource.Name);
    if (tracingOtlpEndpoint != null)
    {
        tracing.AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
            otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        });

        tracing.AddJaegerExporter(options =>
        {
            options.Endpoint = new Uri("http://host.docker.internal:14268");
        });
        
        tracing.AddConsoleExporter();
    }
    else
    {
        tracing.AddConsoleExporter();
    }
});


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
// Configure the Prometheus scraping endpoint
app.MapPrometheusScrapingEndpoint();

// Add custom middleware to log request and response payloads
app.Use(async (context, next) =>
{
    // Log request payload
    context.Request.EnableBuffering();
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;
    Log.Information("Request: {Method} {Path} {Body}", context.Request.Method, context.Request.Path, string.IsNullOrEmpty(requestBody) ? "No Body" : requestBody);

    // Capture the response body
    var originalBodyStream = context.Response.Body;
    using var responseBody = new MemoryStream();
    context.Response.Body = responseBody;

    await next();

    // Log response payload
    context.Response.Body.Seek(0, SeekOrigin.Begin);
    var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
    context.Response.Body.Seek(0, SeekOrigin.Begin);
    Log.Information("Response: {StatusCode} {Body}", context.Response.StatusCode, string.IsNullOrEmpty(responseBodyText) ? "No Body" : responseBodyText);

    await responseBody.CopyToAsync(originalBodyStream);
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", ([FromServices] ILogger<Program> logger) =>
    {
        logger.LogInformation("get weather forecast called");
        // Create a new Activity scoped to the method
        using var activity = weatherforecastActivitySource.StartActivity("WebApplication1.WeatherforecastActivity");
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();

        // Increment the custom counter
        countweatherforecastRequests.Add(1);
        // Add a tag to the Activity
        activity?.SetTag("weatherforecast", "Hello World!");
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}