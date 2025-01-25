using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("JIT Compilation and Tiered Optimization Demo");

        const int numSteps = 100_000_000; // Increase this for more complexity
        const double lowerBound = 0.0;
        const double upperBound = 1.0;

        // Warm-up phase
        Console.WriteLine("Warming up...");
        double result = PerformNumericalIntegration(lowerBound, upperBound, numSteps);

        // Measure the first run
        Stopwatch sw = new Stopwatch();
        sw.Start();
        result = PerformNumericalIntegration(lowerBound, upperBound, numSteps);
        sw.Stop();
        Console.WriteLine($"First run (JIT overhead): {sw.ElapsedMilliseconds} ms, Result: {result}");

        // Measure the second run
        sw.Restart();
        result = PerformNumericalIntegration(lowerBound, upperBound, numSteps);
        sw.Stop();
        Console.WriteLine($"Second run (optimized): {sw.ElapsedMilliseconds} ms, Result: {result}");
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    static double PerformNumericalIntegration(double lowerBound, double upperBound, int numSteps)
    {
        double stepSize = (upperBound - lowerBound) / numSteps;
        double sum = 0.0;

        Parallel.For(0, numSteps, i =>
        {
            double x = lowerBound + i * stepSize;
            double y = Math.Exp(-x * x); // Example curve: e^(-x^2)
            double area = y * stepSize;
            AddToSum(ref sum, area); // Accumulate results safely
        });

        return sum;
    }

    // Thread-safe addition
    static void AddToSum(ref double sum, double value)
    {
        lock ("sum_lock")
        {
            sum += value;
        }
    }
}