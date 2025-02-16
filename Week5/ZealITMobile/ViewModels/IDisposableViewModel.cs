using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZealITMobile.Models;

namespace ZealITMobile.ViewModels;

public partial class IDisposableViewModel : ObservableObject, IDisposable
{
    private bool _disposed;
    private LargeDataModel _largeDataModel;

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

    // Finalizer is generally not needed here.  LargeDataModel has its own.
    // ~IDisposableViewModel()  // Remove this
    // {
    //     Dispose(false);
    // }
}