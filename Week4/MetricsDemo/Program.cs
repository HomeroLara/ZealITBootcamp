using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;using Prometheus;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;
using HistogramConfiguration = Prometheus.HistogramConfiguration;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.OpenTelemetry(options =>
    {
        options.ResourceAttributes = new Dictionary<string, object>
        {
            ["deployment.environment"] = "dev",
            ["service.name"] = "MetricsDemo"
        };
        options.Endpoint = "http://localhost:5341/ingest/otlp/v1/logs";
        options.Protocol = OtlpProtocol.HttpProtobuf;
        options.Headers = new Dictionary<string, string>
        {
            ["X-Seq-ApiKey"] = "MQEG33to0nfJUu19cyej"
        };
    })
    .CreateLogger();
builder.Host.UseSerilog();
// builder.Logging.ClearProviders();
// builder.Logging.AddOpenTelemetry(options =>
// {
//     options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MetricsDemo").AddAttributes(
//     [
//         new KeyValuePair<string, object>("deployment.environment", "dev")
//     ]));
//     options.IncludeScopes = true;
//     options.ParseStateValues = true;
//     //options.AddConsoleExporter(); // Optional, to see logs in console
//     options.AddOtlpExporter(o =>
//     {
//         o.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
//         o.Protocol = OtlpExportProtocol.HttpProtobuf;
//         o.Headers = "X-Seq-ApiKey=MQEG33to0nfJUu19cyej";
//     });
// });


var countweatherforecastRequests = Metrics
    .CreateCounter("weatherforecast_count", "Counts the number of weatherforecast requests");

var activeRequests = Metrics
    .CreateGauge("metricsdemo_weather_activerequests", "Number active requests to weather forecast.");

var requestDuration = Metrics
    .CreateHistogram("weatherforecast_duration", "Histogram Records the duration of weatherforecast requests\"",
        new HistogramConfiguration
        {
            LabelNames = new[] {"status"},
            Buckets = Histogram.LinearBuckets(start: 0, width: 100, count: 10)
        });

// Custom ActivitySource for the application
var weatherforecastActivitySource = new ActivitySource("MetricsDemo_Weatherforecast");


var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
var otel = builder.Services.AddOpenTelemetry();

// Configure OpenTelemetry Resources with the application name
otel.ConfigureResource(resource => resource
    .AddService(serviceName: builder.Environment.ApplicationName));

// Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
otel.WithMetrics(metrics => metrics
    // Metrics provider from OpenTelemetry
    .AddAspNetCoreInstrumentation()
    //.AddMeter(weatherforecastMeter.Name)
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
    // tracing.AddSqlClientInstrumentation();
    tracing.AddSource(weatherforecastActivitySource.Name);
    if (tracingOtlpEndpoint != null)
    {
        // tracing.AddOtlpExporter(otlpOptions =>
        // {
        //     otlpOptions.Endpoint = new Uri("http://host.docker.internal:4317");
        //    // otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
        //    otlpOptions.ExportProcessorType = ExportProcessorType.Simple;
        // });
        tracing.AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
            otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        });
        
        tracing.AddJaegerExporter(options =>
        {
            options.Endpoint = new Uri("http://host.docker.internal:14268");
        });
        
        //tracing.AddConsoleExporter();
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

// Configure the Prometheus scraping endpoint
app.UseMetricServer();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Add custom middleware to log request and response payloads
app.Use(async (context, next) =>
{
    // Log request payload
    context.Request.EnableBuffering();
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;
    logger.LogInformation("Request: {Method} {Path} {Body}",
        context.Request.Method,
        context.Request.Path,
        string.IsNullOrEmpty(requestBody) ? "No Body" : requestBody);

    // Capture the response body
    var originalBodyStream = context.Response.Body;
    using var responseBody = new MemoryStream();
    context.Response.Body = responseBody;

    await next();

    // Log response payload
    context.Response.Body.Seek(0, SeekOrigin.Begin);
    var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
    context.Response.Body.Seek(0, SeekOrigin.Begin);
    logger.LogInformation("Response: {StatusCode} {Body}",
        context.Response.StatusCode,
        string.IsNullOrEmpty(responseBodyText) ? "No Body" : responseBodyText);

    await responseBody.CopyToAsync(originalBodyStream);
});


app.MapGet("/weatherforecast", async ([FromServices] ILogger<Program> logger) =>
    {
        using var activity = weatherforecastActivitySource.StartActivity("MetricsDemo.WeatherforecastActivity");
        logger.LogInformation("get weather forecast called");

        // Increment the custom counter
        countweatherforecastRequests.Inc();

        activeRequests.Inc();

        // Record the duration of the request using Activity
        var startTime = activity?.StartTimeUtc ?? DateTime.UtcNow;

        using var client = new HttpClient();
        var response = await client.GetAsync("http://localhost:5183/weatherforecast");
        response.EnsureSuccessStatusCode();
        var forecast = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();

        var endTime = DateTime.UtcNow;
        var duration = (endTime - startTime).TotalMilliseconds;
        requestDuration.Observe(duration);

        // Decrement the active request count
        activeRequests.Dec();

        return forecast ?? Array.Empty<WeatherForecast>();
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}