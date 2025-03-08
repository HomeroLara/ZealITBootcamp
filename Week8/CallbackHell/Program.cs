// See https://aka.ms/new-console-template for more information

using CallbackHell;
using System;
using System.ComponentModel;
using System.Threading;

Console.WriteLine("Starting BackgroundWorker example...");
RunWithBackgroundWorkerExample.RunWithBackgroundWorker(); // Calls the method demonstrating BackgroundWorker
        
Console.WriteLine("\nStarting Task.ContinueWith() example...");
RunWithContinueWithExample.RunTaskWithContinueWith(); // Calls the method demonstrating Task.ContinueWith()
        
Console.WriteLine("\nMain method complete. Press any key to exit.");
Console.ReadKey(); // Keeps the console open until a key is pressed

// Console.WriteLine("Breakfast will be ready soon!");

// var makingBreakfast = new MakeBreakfast();
// // makingBreakfast.Example1();
// makingBreakfast.Example2();
// Console.WriteLine("Breakfast is ready!");
// Console.ReadKey();

// BackgroundWorker worker = new BackgroundWorker();
// worker.DoWork += Worker_DoWork;
// worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
//
// worker.RunWorkerAsync(5); // Start async operation
//
// Console.WriteLine("Main thread continues running...");
// Console.ReadLine(); // Keep the console open
// static void Worker_DoWork(object sender, DoWorkEventArgs e)
// {
//     int x = (int)e.Argument;
//     Thread.Sleep(3000); // Simulate long-running work
//     e.Result = x * x;
// }
//
// static void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
// {
//     Console.WriteLine($"Operation result: {e.Result}");
// }