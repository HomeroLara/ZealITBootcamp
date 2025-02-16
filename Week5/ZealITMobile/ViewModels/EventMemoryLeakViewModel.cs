using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ZealITMobile.ViewModels;

public partial class EventMemoryLeakViewModel : ObservableObject, IDisposable
{
    private bool _disposed;
    public event EventHandler SomeEvent;

    public EventMemoryLeakViewModel()
    {
        SomeEvent += OnSomeEvent;
    }

    private void OnSomeEvent(object sender, EventArgs e)
    {
        Debug.WriteLine("ViewModel Event Triggered");
    }

    [RelayCommand]
    public void TriggerEvent()
    {
        SomeEvent?.Invoke(this, EventArgs.Empty);
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
            SomeEvent -= OnSomeEvent; // Unsubscribe to prevent memory leak
            _disposed = true;
        }
    }
    
    // Finalizer is generally not needed here since we are manually disposing
    // ~IDisposableViewModel()  // Remove this
    // {
    //     Dispose(false);
    // }
}