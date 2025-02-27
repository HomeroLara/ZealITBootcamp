using System.Text.Json;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ExpensiveService>();
builder.Services.AddControllers();
//builder.Services.AddSingleton<Lazy<ExpensiveService>>(sp =>
//    new Lazy<ExpensiveService>(() => new ExpensiveService(sp.GetRequiredService<ILogger<ExpensiveService>>())));


var app = builder.Build();

app.MapGet("/", (ExpensiveService service) => Results.Ok(service.DoWork()));
//app.MapGet("/work", (Lazy<ExpensiveService> lazyService) =>
//{
//    var service = lazyService.Value; //
//    return Results.Ok(service.DoWork());
//});

app.MapGet("/streaming-data", async (ExpensiveService service, HttpResponse response) =>
{
    response.ContentType = "application/json";
    await foreach (var person in service.GetPeopleAsync())
    {
        var json = JsonSerializer.Serialize(person);
        await response.WriteAsync(json);
        await response.Body.FlushAsync(); // Ensure data is sent immediately
    }
});
app.UseRouting();
app.UseEndpoints(endpoint => endpoint.MapControllers());
app.Run();
