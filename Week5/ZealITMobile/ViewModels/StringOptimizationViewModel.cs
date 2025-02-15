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
    private string _testResult;

    [RelayCommand]
    public void TestStringConcatenation()
    {
        IsBusy = true;
        var stopwatch = Stopwatch.StartNew();
        StringTests.TestStringConcatenation(100000);
        stopwatch.Stop();
        TestResult = $"String Concatenation Time: {stopwatch.ElapsedMilliseconds} ms";
        IsBusy = false;
    }

    [RelayCommand]
    public void TestStringBuilder()
    {
        IsBusy = true;
        var stopwatch = Stopwatch.StartNew();
        StringTests.TestStringBuilder(100000);
        stopwatch.Stop();
        TestResult = $"StringBuilder Time: {stopwatch.ElapsedMilliseconds} ms";
        IsBusy = false;
    }
}

public partial class ObjectManagementViewModel : ObservableObject
{
    private WeakReference<LargeObject> _weakReference;

    [RelayCommand]
    public void CreateLargeObject()
    {
        var obj = new LargeObject();
        _weakReference = new WeakReference<LargeObject>(obj);
    }

    [ObservableProperty]
    private string _objectStatus;

    [RelayCommand]
    public void CheckIfAlive()
    {
        ObjectStatus = _weakReference != null && _weakReference.TryGetTarget(out _)
            ? "Object is still alive"
            : "Object has been garbage collected";
    }
}

public class LargeObject
{
    private byte[] _data = new byte[10 * 1024 * 1024]; // 10MB object
}

public class DisposableResource : IDisposable
{
    private IntPtr _unmanagedResource;
    private bool _disposed;

    public DisposableResource()
    {
        _unmanagedResource = Marshal.AllocHGlobal(100);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Marshal.FreeHGlobal(_unmanagedResource);
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}