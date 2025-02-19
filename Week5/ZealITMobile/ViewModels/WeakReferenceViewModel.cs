using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZealITMobile.Models;

namespace ZealITMobile.ViewModels;

public partial class WeakReferenceViewModel: ObservableObject
{
    [ObservableProperty]
    private string message;
    private List<LargeDataModel> largeObjects;

    public WeakReferenceViewModel()
    {
        // Initialize the list of large objects.  Important to keep this as a member
        // variable to keep the objects in memory as long as possible.
        largeObjects = new List<LargeDataModel>();
        for (int i = 0; i < 100; i++) 
        {
            // Create several
            largeObjects.Add(new LargeDataModel(2)); 
        }
    }
    [RelayCommand]
    private void CollectGarbage()
    {
        largeObjects.Clear();
        
        // rarely need to call these explicitly
        // it's being done here for demo purposes &
        // to verify that finalizers are running as expected.
        // forcing finalization with GC.WaitForPendingFinalizers()
        // will disrupt application flow
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Message = "Garbage Collected";
    }

    [RelayCommand]
    private async Task AccessObjects()
    {
        Message = "Accessing Objects...";
        await Task.Delay(100); // Small delay to allow UI to update

        // Use WeakReferences to hold references to the large objects
        var weakReferences = new List<WeakReference<LargeDataModel>>();

        foreach (var obj in largeObjects)
        {
            weakReferences.Add(new WeakReference<LargeDataModel>(obj));
        }

        int stillAlive = 0;
        int collected = 0;

        foreach (var weakReference in weakReferences)
        {
            // safe and correct and safe way to retrieve the target object 
            if (weakReference.TryGetTarget(out LargeDataModel target))
            {
                // Object is still alive, do something with it.  For example
                // Access a member variable to keep the object alive a little longer.
                _ = target.Size;
                stillAlive++;
            }
            else
            {
                collected++;
            }
        }

        Message = $"Still Alive: {stillAlive}, Collected: {collected}";
    }
}