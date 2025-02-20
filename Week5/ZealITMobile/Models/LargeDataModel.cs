namespace ZealITMobile.Models;

/// <summary>
/// implementing IDisposable ensures proper resource cleanup
/// by manually releasing resources. this is accomplished in the Dispose().
/// without IDisposable this object would hang around until a GC cycle runs
/// and determines there are no strong references. with IDisposable we are forced
/// to implment the Dispose method which makes it easier for us manually dispose of this object
/// </summary>
public class LargeDataModel : IDisposable
{
    // flag to track whether the object has been disposed.
    private bool _disposed;
    
    // the resource we want to dispose
    // ***NOTE: while a byte[] is technically managed, we treat it
    // ***      like an unmanaged resource due to its size(potential)
    // ***      and therefore we want a prompt release as soon as it is not needed
    private byte[] _data;
    
    // stores the size of the data.
    // needed if using ArrayPool.
    public int Size { get; }

    public LargeDataModel(int sizeInMB)
    {
        Size = sizeInMB * 1024 * 1024;
        _data = new byte[Size];
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

    
    /// <summary>
    /// protected virtual dispose method.  Handles resource cleanup.
    /// </summary>
    /// <param name="disposing">True if called from Dispose(), false if from the finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose of any *other* managed resources here (if any).
                // Example:
                // if (_someOtherDisposableObject != null) {
                //     _someOtherDisposableObject.Dispose();
                //     _someOtherDisposableObject = null; // Important: Set to null
                // }
            }
            _data = null; // release the reference
            _disposed = true;
        }
    }
    
    /// <summary>
    /// The garbage collector runs finalizers at an unspecified time.
    /// </summary>
    ~LargeDataModel()
    {
        Dispose(false);
    }
}
