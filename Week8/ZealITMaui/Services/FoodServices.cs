using ZealITMaui.Contracts;
using ZealITMaui.Models;

namespace ZealITMaui.Services;

public class FoodServices: IFoodService
{
    public Task<List<FoodItem>> GetFoodItemsAsync()
    {
        var foodItems = new List<FoodItem>();
        foodItems.Add(new Egg(TimeSpan.FromSeconds(5)));
        foodItems.Add(new Bacon(TimeSpan.FromSeconds(2)));
        foodItems.Add(new Toast(TimeSpan.FromSeconds(1)));
        foodItems.Add(new Coffee(TimeSpan.FromSeconds(2)));
        
        return Task.FromResult(foodItems);
    }
}