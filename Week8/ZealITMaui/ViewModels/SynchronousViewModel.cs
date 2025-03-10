using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZealITMaui.Models;

namespace ZealITMaui.ViewModels;

/*
 This approach runs everything sequentially in a blocking manner. 
 Each task must complete before the next one starts.
   
   Pros ✅
   ✔ Simple and easy to understand
   ✔ Predictable execution order
   
   Cons ❌
   ❌ Blocks the main thread, making it unresponsive
   ❌ Inefficient—long operations (e.g., I/O, API calls) freeze the application
 */
public partial class SynchronousViewModel: ObservableObject
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
    public void StartCooking()
    {
        if (!IsCooking)
        {
            IsCooking = true;
            CookingSteps.Clear();
            CookingSteps.Add("Starting breakfast...");

            CookingSteps.Add("🍳 Making Eggs ...");
            var eggs = new Egg(TimeSpan.FromSeconds(5));
            eggs.Cook();
            CookingSteps.Add("✅ Eggs are ready!");
            
            CookingSteps.Add("🥓 Making-Bacon-Pancakes ...");
            var bacon = new Bacon(TimeSpan.FromSeconds(4));
            bacon.Cook();
            CookingSteps.Add("✅ Making-Bacon-Pancakes is ready!");
            
            CookingSteps.Add("🍞 Making Toast ...");
            var toast = new Toast(TimeSpan.FromSeconds(1));
            toast.Cook();
            CookingSteps.Add("✅ Toast is ready!");
            
            CookingSteps.Add("☕  Making Coffee ...");
            var coffee = new Coffee(TimeSpan.FromSeconds(3));
            coffee.Cook();
            CookingSteps.Add("✅ Coffee is ready!");
            
            CookingSteps.Add("Download Image ...");
            GetImage();
            CookingSteps.Add("✅ Image is downloaded!");
            CookingSteps.Add("Breakfast is ready! 🍽️");
        }
    }
    
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
}