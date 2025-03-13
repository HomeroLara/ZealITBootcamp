using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ZealITMaui.Models;

namespace ZealITMaui.Contracts;

public interface IFoodService
{
    Task<List<FoodItem>> GetFoodItemsAsync();
}