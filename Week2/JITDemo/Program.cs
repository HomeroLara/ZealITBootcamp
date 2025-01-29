using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JITDemo;
using Microsoft.VisualBasic.CompilerServices;
using Utils = JITDemo.Utils;

class Program
{
    static void Main()
    {
        Console.WriteLine("JIT Compilation and Tiered Optimization Demo");

        int size = 10_000_000;
        int[] a = new int[size];
        int[] b = new int[size];
        int[] c = new int[size];

        //Utils.Measure(() => Tier0Test.SumL(size, 10, 10), 1);
        //Utils.Measure(() => Tier0Test.SumL1(size, 10, 10), 1);
        //Utils.Measure(() => Tier1Test.SumL(size, 10, 10), 1);
        
        //Console.WriteLine("Testing Tier0:DivL");
        //Utils.Measure(() => Tier0Test.DivL(size, 10, 10), 1);
        
        //Console.WriteLine("Testing Tier0:DivL1");
        //Utils.Measure(() => Tier0Test.DivL1(size, 10, 10), 1);
        
        ///Console.WriteLine("Testing Tier1:DivL1");
        //Utils.Measure(() => Tier1Test.DivL(size, 10, 10), 1);
        
        //Utils.Measure(() => Tier0Test.SumK(a, 10, 10), 1);
        //Utils.Measure(() => Tier0Test.SumK1(b, 10, 10), 1);
        //Utils.Measure(() => Tier1Test.SumK(b, 10, 10), 1);
        
        //Utils.Measure(() => Tier0Test.SwitchCaseL(size, 2), 1);
       // Utils.Measure(() => Tier0Test.SwitchCaseL1(size, 2), 1);
        //Utils.Measure(() => Tier1Test.SwitchCaseL(size, 2), 1);
        
        //Utils.Measure(() => Tier1Test.SumL(size, 10, 10), 1);
        //Utils.Measure(() => Tier1Test.SumL(size, 10, 10), 1);
        
    //     const int numSteps = 100_000_000; // Increase this for more complexity
    //     const double lowerBound = 0.0;
    //     const double upperBound = 1.0;
    //
    //     // Warm-up phase
    //     Console.WriteLine("Warming up...");
    //     double result = PerformNumericalIntegration(lowerBound, upperBound, numSteps);
    //
    //     // Measure the first run
    //     Stopwatch sw = new Stopwatch();
    //     sw.Start();
    //     result = PerformNumericalIntegration(lowerBound, upperBound, numSteps);
    //     sw.Stop();
    //     Console.WriteLine($"First run (JIT overhead): {sw.ElapsedMilliseconds} ms, Result: {result}");
    //
    //     // Measure the second run
    //     sw.Restart();
    //     result = PerformNumericalIntegration(lowerBound, upperBound, numSteps);
    //     sw.Stop();
    //     Console.WriteLine($"Second run (optimized): {sw.ElapsedMilliseconds} ms, Result: {result}");
    }

    // [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    // static double PerformNumericalIntegration(double lowerBound, double upperBound, int numSteps)
    // {
    //     double stepSize = (upperBound - lowerBound) / numSteps;
    //     double sum = 0.0;
    //
    //     Parallel.For(0, numSteps, i =>
    //     {
    //         double x = lowerBound + i * stepSize;
    //         double y = Math.Exp(-x * x); // Example curve: e^(-x^2)
    //         double area = y * stepSize;
    //         AddToSum(ref sum, area); // Accumulate results safely
    //     });
    //
    //     return sum;
    // }
    //
    // // Thread-safe addition
    // static void AddToSum(ref double sum, double value)
    // {
    //     lock ("sum_lock")
    //     {
    //         sum += value;
    //     }
    // }
}