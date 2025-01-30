namespace WebApplication1.Services;

public class Service1 : IService1
{
    private readonly ILogger<Service1> _logger;

    public Service1(ILogger<Service1> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation("Hello world from Serivce1");
    }
}