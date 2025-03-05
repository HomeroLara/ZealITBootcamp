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

public class Egg() : FoodItem(TimeSpan.FromSeconds(3));
public class Toast() : FoodItem(TimeSpan.FromSeconds(3));
public class Bacon() : FoodItem(TimeSpan.FromSeconds(3));
public class Coffee() : FoodItem(TimeSpan.FromSeconds(3));