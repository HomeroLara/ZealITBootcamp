using Microsoft.AspNetCore.Mvc;

namespace WebApplication1;

public class StreamingController : ControllerBase
{
    private readonly ILogger<StreamingController> _logger;
    private readonly ExpensiveService _expensiveService;

    public StreamingController(ILogger<StreamingController> logger, ExpensiveService expensiveService)
    {
        _logger = logger;
        _expensiveService = expensiveService;
    }
    
    [HttpGet("streaming-people")]
    public async IAsyncEnumerable<Person> GetPeopleAsync()
    {
        await foreach (var person in _expensiveService.GetPeopleAsync())
        {
            yield return person;
        }
    }
}