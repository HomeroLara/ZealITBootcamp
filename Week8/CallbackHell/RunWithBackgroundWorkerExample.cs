using System.ComponentModel;

namespace CallbackHell;

public static class RunWithBackgroundWorkerExample
{
    // Method demonstrating the use of BackgroundWorker
    public static void RunWithBackgroundWorker()
    {
        BackgroundWorker worker = new BackgroundWorker(); // Create a new BackgroundWorker instance
        
        // Event handler for the background task
        worker.DoWork += (sender, e) =>
        {
            Console.WriteLine("[BackgroundWorker] Working in the background...");
            Thread.Sleep(2000); // Simulate a time-consuming task
            e.Result = "Task Completed"; // Store the result of the task
        };
        
        // Event handler for when the task completes
        worker.RunWorkerCompleted += (sender, e) =>
        {
            Console.WriteLine($"[BackgroundWorker] Completed: {e.Result}");
        };
        
        worker.RunWorkerAsync(); // Start the background operation asynchronously
    }
}