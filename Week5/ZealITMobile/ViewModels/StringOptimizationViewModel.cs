using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.InteropServices;
using ZealITMobile.Utlities;

namespace ZealITMobile.ViewModels;

public partial class StringOptimizationViewModel : ObservableObject
{
    [ObservableProperty] 
    private bool _stringConcatIsBusy;
    
    [ObservableProperty]
    private bool _sbIsBusy;
        
    [ObservableProperty]
    private string _testStringConcatenationResult = "String Concatenation Time: 00:00:00";
    
    [ObservableProperty]
    private string _testStringBuilderResult = "StringBuilder Time: 00:00:00";

    [RelayCommand]
    public async Task TestStringConcatenation()
    {
        // long before = GC.GetTotalMemory(true);
        // Console.WriteLine($"Approx memory used by ViewModel before string concat: {before} bytes");
        StringConcatIsBusy = true;
        TestStringConcatenationResult = "Calculating string concatenation...";
        await Task.Delay(100); // Small delay to allow UI to update
        await Task.Run(() =>  {
            var stopwatch = Stopwatch.StartNew();
            StringTests.TestStringConcatenation(100000);
            stopwatch.Stop();
            TestStringConcatenationResult = $"String Concatenation Time: {stopwatch.ElapsedMilliseconds} ms";
            StringConcatIsBusy = false;
        });
        // long after = GC.GetTotalMemory(true);
        // Console.WriteLine($"Approx memory used by ViewModel: {after - before} bytes");
    }

    [RelayCommand]
    public async Task TestStringBuilder()
    {
        SbIsBusy = true;
        TestStringBuilderResult = "Calculating string builder...";
        await Task.Delay(100); // Small delay to allow UI to update
        await Task.Delay(100); // Small delay to allow UI to update
        await Task.Run(() =>
        {
            var stopwatch = Stopwatch.StartNew();
            StringTests.TestStringBuilder(100000);
            stopwatch.Stop();
            TestStringBuilderResult = $"StringBuilder Time: {stopwatch.ElapsedMilliseconds} ms";
            SbIsBusy = false;
        });
    }
}
