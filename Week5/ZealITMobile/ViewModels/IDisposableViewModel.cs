using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZealITMobile.Models;
using ZealITMobile.Utlities;

namespace ZealITMobile.ViewModels;

public partial class IDisposableViewModel : ObservableObject, IDisposable
{
    private bool _disposed;
    private StringBuilder _stringBuilder = new StringBuilder();
    private LargeDataModel? _largeDataModel = null;    
    
    [ObservableProperty] 
    private string _usingStatementResult = string.Empty;

    [ObservableProperty]
    private string _createLargeDataModelResult = string.Empty;
    
    [ObservableProperty]
    private string _disposeResult = string.Empty;
    
    public IDisposableViewModel()
    {
        _disposed = false;
    }
    
    [RelayCommand]
    public void CreateLargeDataModel()
    {
        if(_largeDataModel is null)
        {
            _stringBuilder = new StringBuilder();
            
            CreateLargeDataModelResult = string.Empty;
            var beforeMemoryLog = MemoryUtility.GetMemoryReadingAndLog("Memory before LargeDataModel initialization");
            _stringBuilder.AppendLine(beforeMemoryLog.Description);
            _stringBuilder.AppendLine();
            
            _largeDataModel = new LargeDataModel(10);
            var afterMemoryLog = MemoryUtility.GetMemoryReadingAndLog("Memory after LargeDataModel initialization");
            _stringBuilder.AppendLine(afterMemoryLog.Description);
            _stringBuilder.AppendLine();
            
            _stringBuilder.AppendLine($"Memory allocated by LargeDataModel (approx): {(afterMemoryLog.Memory - beforeMemoryLog.Memory) / 1024:N0} KB");
            _stringBuilder.AppendLine();
            CreateLargeDataModelResult = _stringBuilder.ToString();
        }
    }
    
    
    [RelayCommand]
    public void ManualDispose()
    {
        if (!_disposed)
        {
            _stringBuilder = new StringBuilder();
            DisposeResult = string.Empty;
            var memoryBeforeDispose = MemoryUtility.GetMemoryReadingAndLog("Memory before calling ManualDispose");
            _stringBuilder.AppendLine(memoryBeforeDispose.Description);
            _stringBuilder.AppendLine();
        
            // no need for separate disposing block. LargeDataModel handles its own disposal.
            _largeDataModel?.Dispose();
            _largeDataModel = null; // release the strong reference
            _disposed = true;
            
            var memoryAfterDispose = MemoryUtility.GetMemoryReadingAndLog("Memory after calling ManualDispose");
            _stringBuilder.AppendLine(memoryAfterDispose.Description);
            _stringBuilder.AppendLine();
        
            _stringBuilder.AppendLine($"Memory allocated after ManualDispose (approx): {(memoryBeforeDispose.Memory - memoryAfterDispose.Memory) / 1024:N0} KB");
            _stringBuilder.AppendLine();
            DisposeResult = _stringBuilder.ToString();
        }
    }

    /// <summary>
    /// public dispose method. called explicitly to release resources.
    /// ***NOTE: in our case, this will be triggered manually or by Page life cycle events
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        
        // we want to suppress the finalization since we are manually disposing the resources in this object: the GC doesn't need to call the finalizer
        // we're telling the GC that we've manually cleaned up the resources related to this object
        // improves performance.
        // prevents double cleanup 
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            var memoryBeforeDispose = MemoryUtility.GetMemoryReadingAndLog(" -> Memory before calling LargeDataModel.Dispose()");
            // no need for separate disposing block. LargeDataModel handles its own disposal.
            _largeDataModel?.Dispose();
            _largeDataModel = null; // release the strong reference
            _disposed = true;
            var memoryAfterDispose = MemoryUtility.GetMemoryReadingAndLog(" -> Memory after calling LargeDataModel.Dispose())");
            Console.WriteLine($" -> Memory allocated by LargeDataModel (approx): {(memoryBeforeDispose.Memory - memoryAfterDispose.Memory) / 1024:N0} KB");
        }
    }
    
    [RelayCommand]
    public void UsingStatement()
    {
        _stringBuilder = new StringBuilder();
        UsingStatementResult = string.Empty;
        // *** NOTE: the object that is wrapped in a using statement 
        // must implment IDisposable otherwise you'll get a compiler error
        
        // The using statement ensures that the LargeDataModel.Dispose()
        // is called when the block exits, even if an exception occurs.
        
        var memoryBeforeUsing = MemoryUtility.GetMemoryReadingAndLog("Memory before using block");
        _stringBuilder.AppendLine(memoryBeforeUsing.Description);
        _stringBuilder.AppendLine();
        
        using (var largeDataModel = new LargeDataModel(50)) // Allocate 50MB
        {
            var usingBlockMemory = MemoryUtility.GetMemoryReadingAndLog("Memory inside using block, alive");
            _stringBuilder.AppendLine(usingBlockMemory.Description);
            _stringBuilder.AppendLine();
            // do some stuff with largeDataModel
        } // Dispose called automatically 
        
        var memoryAfterUsing = MemoryUtility.GetMemoryReadingAndLog("Memory after using block disposed");
        _stringBuilder.AppendLine(memoryAfterUsing.Description);
        _stringBuilder.AppendLine();
        UsingStatementResult = _stringBuilder.ToString();
        
        // using (var stream = new FileStream("data.txt", FileMode.Open))
        // {
        //     // Read or write operations
        // } // Dispose called automatically
        
        // if we didn't use using, we'd have to explicitly call LargeDataModel.Dispose()
        // like so:
        // LargeDataModel largeDataModel;
        // try
        // {
        //     do some stuff with largeDataModel
        //     largeDataModel = new LargeDataModel(50);
        //     // do some more stuff...
        //     UsingStatementResult = "LargeDataModel used and disposed manually.";
        // }
        // finally
        // {
        //     largeDataModel?.Dispose();  // Must manually call Dispose()
        // }
    }

    // Finalizer is generally not needed here.  LargeDataModel has its own.
    // ~IDisposableViewModel()  // Remove this
    // {
    //     Dispose(false);
    // }
}