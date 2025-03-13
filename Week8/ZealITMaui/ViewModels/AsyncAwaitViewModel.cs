using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZealITMaui.Contracts;
using ZealITMaui.Models;

namespace ZealITMaui.ViewModels;

/*
    The async/await pattern simplifies asynchronous programming, making it more readable and maintainable.

    Pros ✅
    ✔ No blocking—operations run asynchronously
    ✔ Readable and maintainable (no deep nesting)
    ✔ Exception handling is easier (try/catch)

    Cons ❌
    ❌ Requires understanding of Task and await
    ❌ Some overhead for async state machines
 */
public partial class AsyncAwaitViewModel: ObservableObject
{
    [ObservableProperty] 
    private ObservableCollection<string> _cookingSteps;
    
    [ObservableProperty]
    private bool _isCooking;
    
    [ObservableProperty]
    private string _breakfastStatus;
    
    [ObservableProperty]
    private ImageSource _downloadImage;
    
    [ObservableProperty]
    private bool _isDownloading;
    
    private readonly IFoodService _foodService;

    public AsyncAwaitViewModel(IFoodService foodService)
    {
        CookingSteps = new ObservableCollection<string>();
        _foodService = foodService;
    }

    [RelayCommand]
    public async Task StartCookingAsync()
    {
        if (!IsCooking)
        {
            IsCooking = true;
            CookingSteps.Clear();
            CookingSteps.Add("Starting breakfast...");

            CookingSteps.Add("🍳 Making Eggs ...");
            var eggs = new Egg(TimeSpan.FromSeconds(7));
            await eggs.CookAsync();
            CookingSteps.Add("✅ Eggs are ready!");
            
            CookingSteps.Add("🥓 Making-Bacon-Pancakes ...");
            var bacon = new Bacon(TimeSpan.FromSeconds(4));
            await bacon.CookAsync();
            CookingSteps.Add("✅ Making-Bacon-Pancakes is ready!");
            
            CookingSteps.Add("🍞 Making Toast ...");
            var toast = new Toast(TimeSpan.FromSeconds(1));
            await toast.CookAsync();
            CookingSteps.Add("✅ Toast is ready!");
            
            CookingSteps.Add("☕  Making Coffee ...");
            var coffee = new Coffee(TimeSpan.FromSeconds(3));
            await coffee.CookAsync();
            CookingSteps.Add("✅ Coffee is ready!");
            
            CookingSteps.Add("Download Image ...");
            await GetImage();
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