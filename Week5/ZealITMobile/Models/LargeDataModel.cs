namespace ZealITMobile.Models;

public class LargeDataModel : IDisposable
{
    private bool _disposed;
    private byte[] _data;

    public LargeDataModel(int sizeInMB)
    {
        _data = new byte[sizeInMB * 1024 * 1024];
    }

    ~LargeDataModel()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        
        // prevent the garbage collector from calling the finalizer (~Finalizer)
        // on an object after it has been manually disposed.
        // prevents double cleanup 
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose any other managed resources, if needed
            }
            _data = null;
            _disposed = true;
        }
    }
}
