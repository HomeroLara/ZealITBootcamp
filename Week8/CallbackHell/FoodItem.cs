namespace CallbackHell;


public class FoodItem
{
    readonly TimeSpan _cookTime;
    public string Name { get; }

    public FoodItem(TimeSpan cookTime)
    {
        _cookTime = cookTime;
        Name = GetType().Name;;
    }

    public async Task Make()
    {
        Console.WriteLine($"Making {Name} ...");
        
        // the await keyword tells the .net runtime to return
        // to the calling method which in this case is the program's 
        // entry point/Main() while this food item is being made in a 
        // background task.
        await Task.Delay(_cookTime);
        Console.WriteLine($"Made {Name} ...");
    }
}

public class Egg() : FoodItem(TimeSpan.FromSeconds(3));
public class Toast() : FoodItem(TimeSpan.FromSeconds(3));
public class Bacon() : FoodItem(TimeSpan.FromSeconds(3));
public class Coffee() : FoodItem(TimeSpan.FromSeconds(3));