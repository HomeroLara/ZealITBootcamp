using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZealITMaui.Models;

namespace ZealITMaui.ViewModels;

public partial class CallBackHellViewModel: ObservableObject
{
    [ObservableProperty] 
    private ObservableCollection<string> _cookingSteps;
    
    [ObservableProperty]
    private bool _isCooking;

    public CallBackHellViewModel()
    {
        CookingSteps = new ObservableCollection<string>();
    }

    [RelayCommand]
    public void StartCooking()
    {
        if (!IsCooking)
        {
            IsCooking = true;
            CookingSteps.Clear();
            CookingSteps.Add("Starting breakfast...");

            var eggs = new Egg();
            eggs.Cook().ContinueWith(_ =>
            {
                CookingSteps.Add("🍳 Eggs are ready!");

                var bacon = new Bacon();
                bacon.Cook().ContinueWith(__ =>
                {
                    CookingSteps.Add("🥓 Bacon is ready!");

                    var toast = new Toast();
                    toast.Cook().ContinueWith(___ =>
                    {
                        CookingSteps.Add("🍞 Toast is ready!");

                        var coffee = new Coffee();
                        coffee.Cook().ContinueWith(____ =>
                        {
                            CookingSteps.Add("☕ Coffee is ready!");
                            CookingSteps.Add("Breakfast is ready! 🍽️");
                            _isCooking = false;
                        }, TaskScheduler.FromCurrentSynchronizationContext()); // UI update
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}