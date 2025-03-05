using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ZealITMaui.ViewModels;

public partial class BreakfastViewModel: ObservableObject
{
    [ObservableProperty] 
    private ObservableCollection<string> _cookingSteps;
    
    [ObservableProperty]
    private bool _isCooking;

    public BreakfastViewModel()
    {
        CookingSteps = new ObservableCollection<string>();
    }

    [RelayCommand]
    public async Task StartCooking()
    {
        if (!_isCooking)
        {
            _isCooking = true;
            CookingSteps.Clear();
                    
            CookingSteps.Add("Starting breakfast...");

            var eggsTask = FryEggs();
            var makeBacon = MakeBacon();
            var toastTask = ToastBread();
            var coffeeTask = BrewCoffee();

            await Task.WhenAll(eggsTask, toastTask, coffeeTask);

            CookingSteps.Add("Breakfast is ready! 🍽️");
            IsCooking = false;
        }
    }
    
    private async Task FryEggs()
    {
        CookingSteps.Add("🍳 Frying eggs...");
        await Task.Delay(3000);
        CookingSteps.Add("✅ Eggs are ready!");
    }
    
    private async Task MakeBacon()
    {
        CookingSteps.Add("🥓 Making Bacon ...");
        await Task.Delay(3000);
        CookingSteps.Add("✅ Becon is ready!");
    }

    private async Task ToastBread()
    {
        CookingSteps.Add("🍞 Toasting bread...");
        await Task.Delay(2000);
        CookingSteps.Add("✅ Toast is ready!");
    }

    private async Task BrewCoffee()
    {
        CookingSteps.Add("☕ Brewing coffee...");
        await Task.Delay(5000);
        CookingSteps.Add("✅ Coffee is ready!");
    }
}