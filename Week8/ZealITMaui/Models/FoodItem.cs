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

    public virtual async Task Cook()
    {
        Console.WriteLine($"Making {Name} ...");
        await Task.Delay(_cookTime);
        Console.WriteLine($"Made {Name} ...");
    }
}

public class Egg(TimeSpan timeSpan) : FoodItem(timeSpan);
public class Toast(TimeSpan timeSpan) : FoodItem(timeSpan);
public class Bacon(TimeSpan timeSpan) : FoodItem(timeSpan);
public class Coffee(TimeSpan timeSpan) : FoodItem(timeSpan);