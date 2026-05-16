using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data
{
    public class FoodItemDAL : FoodOrderingSystem.Data.Interfaces.IFoodItemDAL
    {
        public bool AddFoodItem(FoodItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrWhiteSpace(item.Name)) throw new ArgumentException("Food item name cannot be empty.", nameof(item.Name));
            if (item.Price < 0) throw new ArgumentException("Food item price cannot be negative.", nameof(item.Price));
            if (item.AdminId <= 0) throw new ArgumentException("Invalid Admin ID.", nameof(item.AdminId));

            string query = @"INSERT INTO [FoodItem] (admin_id, name, description, price, category, image, available) 
                             VALUES (@AdminId, @Name, @Description, @Price, @Category, @Image, @Available)";

            SqlParameter imgParam = new SqlParameter("@Image", SqlDbType.VarBinary);
            imgParam.Value = (object)item.Image ?? DBNull.Value;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AdminId", item.AdminId),
                new SqlParameter("@Name", item.Name),
                new SqlParameter("@Description", (object)item.Description ?? DBNull.Value),
                new SqlParameter("@Price", item.Price),
                new SqlParameter("@Category", item.Category),
                imgParam,
                new SqlParameter("@Available", item.Available)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public List<FoodItem> GetAllFoodItems()
        {
            List<FoodItem> items = new List<FoodItem>();
            string query = "SELECT * FROM [FoodItem]";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                items.Add(new FoodItem
                {
                    FoodId = Convert.ToInt32(row["food_id"]),
                    AdminId = Convert.ToInt32(row["admin_id"]),
                    Name = row["name"].ToString(),
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                    Price = Convert.ToDecimal(row["price"]),
                    Category = row["category"].ToString(),
                    Image = row["image"] != DBNull.Value ? (byte[])row["image"] : null,
                    Available = Convert.ToBoolean(row["available"]),
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    UpdatedAt = Convert.ToDateTime(row["updated_at"])
                });
            }
            return items;
        }

        public bool UpdateFoodItemPrice(int foodId, decimal newPrice)
        {
            if (foodId <= 0) throw new ArgumentException("Food ID must be greater than zero.", nameof(foodId));
            if (newPrice < 0) throw new ArgumentException("Price cannot be negative.", nameof(newPrice));

            string query = "UPDATE [FoodItem] SET price = @Price, updated_at = GETDATE() WHERE food_id = @Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Price", newPrice),
                new SqlParameter("@Id", foodId)
            };
            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public bool DeleteFoodItem(int foodId)
        {
            if (foodId <= 0) throw new ArgumentException("Food ID must be greater than zero.", nameof(foodId));

            string query = "DELETE FROM [FoodItem] WHERE food_id = @Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", foodId)
            };
            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
    }
}
