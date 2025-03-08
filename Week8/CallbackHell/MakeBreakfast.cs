namespace CallbackHell;

public static class MakeBreakfast
{
    public static void Example2()
    {
        var eggs = new Egg();
        eggs.Make().ContinueWith(_ =>
        {
            var bacon = new Bacon();
            bacon.Make().ContinueWith(__ =>
            {
                var toast = new Toast();
                toast.Make().ContinueWith(___ =>
                {
                    var coffee = new Coffee();
                    coffee.Make().ContinueWith(____ =>
                    {
                        Console.WriteLine("Coffee is ready! ...");
                    }, TaskScheduler.FromCurrentSynchronizationContext()); // UI update
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }, TaskScheduler.FromCurrentSynchronizationContext());
        
        // TaskScheduler.FromCurrentSynchronizationContext() is redundant
        // and in most cases it is not needed but it does ensure the
        // continuation of the task runs on the same thread that initiated it
        // (typically the UI thread)
        // TaskScheduler.FromCurrentSynchronizationContext() captures
        // the current UI thread's context
    }
}