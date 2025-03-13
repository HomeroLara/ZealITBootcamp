namespace CallbackHell;


public class FoodItem
{
    readonly TimeSpan _cookTime;
    public string Name { get; }

    public FoodItem(TimeSpan cookTime)
    {
        _cookTime = cookTime;
        Name = GetType().Name;
    }

    public async Task CookAsync()
    {
        Console.WriteLine($"Current Thread Id {Thread.CurrentThread.ManagedThreadId}...");
        Console.WriteLine($"Cooking {Name}(s) ...");
        // the await keyword tells the .net runtime to return
        // to the calling method which in this case is the program's 
        // entry point/Main() while this food item is being made in a 
        // background task.
        await Task.Delay(_cookTime);
        
        // since there is no SynchronizationContext (like in a console application or a background thread),
        // the remaining code runs on a thread pool thread, which may have a different thread ID.
        Console.WriteLine($"Current Thread Id {Thread.CurrentThread.ManagedThreadId}...");
        
        Console.WriteLine($"{Name}(s) cooked ...");
    }
}

public class Eggs(TimeSpan cookTime) : FoodItem(cookTime);
public class Toast(TimeSpan cookTime) : FoodItem(cookTime);
public class Bacon(TimeSpan cookTime) : FoodItem(cookTime);
public class Coffee(TimeSpan cookTime) : FoodItem(cookTime);