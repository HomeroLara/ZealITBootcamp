namespace GCDemo;

class MyClass : IDisposable
{
    public MyClass()
    {
        MyStaticEventClass.MyEvent += HandleEvent;
    }

    private void HandleEvent(object sender, EventArgs e)
    {
        Console.WriteLine("Event triggered");
    }

    // Properly unsubscribe to prevent memory leaks
    public void Dispose()
    {
        MyStaticEventClass.MyEvent -= HandleEvent;
        Console.WriteLine("Unsubscribed from event");
    }

    ~MyClass()
    {
        Dispose(); // Ensures cleanup in case Dispose() is not called
        Console.WriteLine("Finalizer called!");
    }
    
    public async Task DoWorkAsync()
    {
        await Task.Delay(1000); // Captures "this", preventing GC
        Console.WriteLine("Task completed");
    }
}

static class MyStaticEventClass
{
    public static event EventHandler MyEvent;
}