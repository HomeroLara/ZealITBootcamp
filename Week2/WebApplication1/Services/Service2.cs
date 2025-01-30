namespace WebApplication1.Services;

public class Service2 : IService2
{
    private readonly ILogger<Service2> _logger;

    public Service2(ILogger<Service2> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation("Hello world from Serivce2");
    }
}