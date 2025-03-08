namespace CallbackHell;

public static class RunWithContinueWithExample
{
    // Method demonstrating Task-Based Asynchronous Pattern (TAP) using Task.ContinueWith()
    public static void RunTaskWithContinueWith()
    {
        Task.Run(() =>
        {
            Console.WriteLine("[Task] Running async operation...");
            Thread.Sleep(2000); // Simulate a time-consuming task
            return "Task Completed"; // Return the result of the task
        })
        .ContinueWith(task =>
        {
            // This runs after the initial Task completes
            Console.WriteLine($"[Task.ContinueWith] Completed: {task.Result}");
        });
    }
}