namespace WebApplication1;

public class ExpensiveService
{
    private readonly ILogger<ExpensiveService> _logger;

    public ExpensiveService(ILogger<ExpensiveService> logger)
    {
        Task.Delay(10000);
        _logger = logger;
    }

    public async IAsyncEnumerable<Person> GetPeopleAsync()
    {
        for (int i = 1; i <= 3; i++)
        {
            await Task.Delay(1000); // Simulating data fetching delay
            yield return new Person
            {
                Id = i,
                Name = i switch
                {
                    1 => "Alice",
                    2 => "Bob",
                    3 => "Charlie",
                    _ => throw new NotSupportedException()
                },
                Age = 25 + i * 5
            };
        }
    }

    public object? DoWork()
    {
        Task.Delay(1000);
        return new object();
    }
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}