using System.Collections.Generic;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data.Interfaces
{
    public interface IFoodItemDAL
    {
        bool AddFoodItem(FoodItem item);
        List<FoodItem> GetAllFoodItems();
        bool UpdateFoodItemPrice(int foodId, decimal newPrice);
        bool DeleteFoodItem(int foodId);
    }
}
