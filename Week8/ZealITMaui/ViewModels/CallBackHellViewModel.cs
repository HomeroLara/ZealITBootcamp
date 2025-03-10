using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZealITMaui.Models;

namespace ZealITMaui.ViewModels;

/*
    Task Continuation using .ContinueWith() (Pre-Async/Await)
    Explanation:
    Before async/await, we used Task Continuations (ContinueWith()) to run operations asynchronously. 
    Each task triggers the next after it finishes.
   
    Pros ✅
    ✔ Asynchronous no UI blocking
    ✔ No thread freezing

    Cons ❌
    ❌ Callback Hell—deeply nested code is hard to read
    ❌ No built-in exception propagation
 */
public partial class CallBackHellViewModel: ObservableObject
{
    [ObservableProperty] 
    private ObservableCollection<string> _cookingSteps = new ObservableCollection<string>();
    
    [ObservableProperty]
    private bool _isCooking;
    
    [ObservableProperty]
    private string _breakfastStatus;
    
    [ObservableProperty]
    private ImageSource _downloadImage;
    
    [ObservableProperty]
    private bool _isDownloading;

    [RelayCommand]
    private async Task GetImage()
    {
        try
        {
            IsDownloading = true;

            using HttpClient client = new();
            var imageBytes = await client.GetByteArrayAsync("https://static.bokeh.org/logos/logo.png");

            // Convert byte array to an ImageSource
            DownloadImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            IsDownloading = false;
        }
    }

    [RelayCommand]
    public void StartCooking()
    {
        // prior to C# 5.0 the Task Parallel Library (TPL)'s ContinueWith()
        // was used for asynchronous programming. It also provided a way to execute
        // code after a task completed.
        // c# 5.0 introduced 'async/await'
        
        // several drawbacks:
        // leads to callback hell
        // made error handling very difficult
        // hard to read
        if (!IsCooking)
        {
            IsCooking = true;
            CookingSteps.Clear();
            CookingSteps.Add("Starting breakfast...");

            var eggs = new Egg(TimeSpan.FromSeconds(20));
            
            CookingSteps.Add("🍳 Making Eggs ...");
            eggs.CookAsync().ContinueWith(_ =>
            {
                CookingSteps.Add("✅ Eggs are ready!");

                var bacon = new Bacon(TimeSpan.FromSeconds(4));
                CookingSteps.Add("🥓 Making-Bacon-Pancakes ...");
                bacon.CookAsync().ContinueWith(__ =>
                {
                    CookingSteps.Add("✅ Making-Bacon-Pancakes is ready!");

                    var toast = new Toast(TimeSpan.FromSeconds(1));
                    CookingSteps.Add("🍞 Making Toast ...");
                    toast.CookAsync().ContinueWith(___ =>
                    {
                        CookingSteps.Add("✅ Toast is ready!");

                        var coffee = new Coffee(TimeSpan.FromSeconds(4));
                        CookingSteps.Add("☕  Making Coffee ...");
                        coffee.CookAsync().ContinueWith(____ =>
                        {
                            CookingSteps.Add("✅ Coffee is ready!");
                            CookingSteps.Add("Breakfast is ready! 🍽️");
                            IsCooking = false;

                            CookingSteps.Add("Download Image ...");
                            GetImage().ContinueWith(____ =>
                            {
                                CookingSteps.Add("✅ Image is downloaded!");
                                CookingSteps.Add("Breakfast is ready! 🍽️");
                                IsCooking = false;
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                        }, TaskScheduler.FromCurrentSynchronizationContext()); // UI update
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }, TaskScheduler.FromCurrentSynchronizationContext());
            
            // TaskScheduler.FromCurrentSynchronizationContext() is redundant
            // and in most cases it is not needed but it does ensure the
            // continuation of the task runs on the same thread that initiated it
            // (typically the UI thread)
            // TaskScheduler.FromCurrentSynchronizationContext() captures
            // the current UI thread's context
        }
    }
}