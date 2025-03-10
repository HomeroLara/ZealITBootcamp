namespace ZealITMaui.Models;

public class FoodItem
{
    readonly TimeSpan _cookTime;
    public string Name { get; }

    public FoodItem(TimeSpan cookTime)
    {
        _cookTime = cookTime;
        Name = GetType().Name;;
    }

    public virtual async Task CookAsync()
    {
        Console.WriteLine($"Making {Name} ...");
        await Task.Delay(_cookTime);
        Console.WriteLine($"Made {Name} ...");
    }
    
    public virtual void Cook()
    {
        Console.WriteLine($"Making {Name} ...");
        Task.Delay(_cookTime).Wait(); // simulate blocking
        Console.WriteLine($"✅ Made {Name} ...");
    }
}

public class Egg(TimeSpan timeSpan) : FoodItem(timeSpan);
public class Toast(TimeSpan timeSpan) : FoodItem(timeSpan);
public class Bacon(TimeSpan timeSpan) : FoodItem(timeSpan);
public class Coffee(TimeSpan timeSpan) : FoodItem(timeSpan);