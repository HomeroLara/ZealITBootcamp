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
    private bool _isBusy;
        
    [ObservableProperty]
    private string _testStringConcatenationResult = "String Concatenation Time: 00:00:00";
    
    [ObservableProperty]
    private string _testStringBuilderResult = "StringBuilder Time: 00:00:00";

    [RelayCommand]
    public void TestStringConcatenation()
    {
        IsBusy = true;
        var stopwatch = Stopwatch.StartNew();
        StringTests.TestStringConcatenation(100000);
        stopwatch.Stop();
        TestStringConcatenationResult = $"String Concatenation Time: {stopwatch.ElapsedMilliseconds} ms";
        IsBusy = false;
    }

    [RelayCommand]
    public void TestStringBuilder()
    {
        IsBusy = true;
        var stopwatch = Stopwatch.StartNew();
        StringTests.TestStringBuilder(100000);
        stopwatch.Stop();
        TestStringBuilderResult = $"StringBuilder Time: {stopwatch.ElapsedMilliseconds} ms";
        IsBusy = false;
    }
}
