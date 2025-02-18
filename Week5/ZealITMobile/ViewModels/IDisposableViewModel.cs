using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZealITMobile.Models;

namespace ZealITMobile.ViewModels;

public partial class IDisposableViewModel : ObservableObject, IDisposable
{
    private bool _disposed;
    private LargeDataModel _largeDataModel;

    [ObservableProperty] 
    private string _usingStatementResult;

    [RelayCommand]
    public void CreateLargeDataModel()
    {
        _largeDataModel = new LargeDataModel(10);
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
            // no need for separate disposing block. LargeDataModel handles its own disposal.
            _largeDataModel?.Dispose();
            _largeDataModel = null; // release the reference
            _disposed = true;
        }
    }
    
    [RelayCommand]
    public void UsingStatement()
    {
        // *** NOTE: the object that is wrapped in a using statement 
        // must implment IDisposable otherwise you'll get a compiler error
        
        // The using statement ensures that the LargeDataModel.Dispose()
        // is called when the block exits, even if an exception occurs.
        using (var largeDataModel = new LargeDataModel(50)) // Allocate 50MB
        {
            // do some stuff with largeDataModel
            UsingStatementResult = "LargeDataModel used and disposed automatically.";
            // do some more stuff....
        }
        
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