using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data
{
    public class CartDAL
    {
        public int GetOrCreateCart(int customerId)
        {
            if (customerId <= 0) throw new ArgumentException("Customer ID must be greater than zero.", nameof(customerId));

            string selQuery = "SELECT cart_id FROM [Cart] WHERE customer_id = @Id";
            SqlParameter[] selParams =
            {
                new SqlParameter("@Id", customerId)
            };

            var result = DatabaseHelper.ExecuteScalar(selQuery, selParams);
            if (result != null) return Convert.ToInt32(result);

            string insQuery = "INSERT INTO [Cart] (customer_id) OUTPUT INSERTED.cart_id VALUES (@Id)";
            SqlParameter[] insParams =
            {
                new SqlParameter("@Id", customerId)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(insQuery, insParams));
        }

        public void AddToCart(int cartId, int foodId, int quantity, decimal unitPrice)
        {
            if (cartId <= 0) throw new ArgumentException("Cart ID must be greater than zero.", nameof(cartId));
            if (foodId <= 0) throw new ArgumentException("Food ID must be greater than zero.", nameof(foodId));
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

            string chkQuery = @"SELECT cart_item_id, quantity 
                                FROM [CartItem] 
                                WHERE cart_id = @CId AND food_id = @FId";

            SqlParameter[] chkParams =
            {
                new SqlParameter("@CId", cartId),
                new SqlParameter("@FId", foodId)
            };

            var dt = DatabaseHelper.ExecuteQuery(chkQuery, chkParams);

            if (dt.Rows.Count > 0)
            {
                int itemId = Convert.ToInt32(dt.Rows[0]["cart_item_id"]);
                int newQty = Convert.ToInt32(dt.Rows[0]["quantity"]) + quantity;

                string updQuery = "UPDATE [CartItem] SET quantity = @Q WHERE cart_item_id = @Id";
                SqlParameter[] updParams =
                {
                    new SqlParameter("@Q", newQty),
                    new SqlParameter("@Id", itemId)
                };

                DatabaseHelper.ExecuteNonQuery(updQuery, updParams);
            }
            else
            {
                string insQuery = @"INSERT INTO [CartItem] 
                                    (cart_id, food_id, quantity, unit_price) 
                                    VALUES (@CId, @FId, @Qty, @Price)";

                SqlParameter[] insParams =
                {
                    new SqlParameter("@CId", cartId),
                    new SqlParameter("@FId", foodId),
                    new SqlParameter("@Qty", quantity),
                    new SqlParameter("@Price", unitPrice)
                };

                DatabaseHelper.ExecuteNonQuery(insQuery, insParams);
            }
        }

        public List<CartItem> GetCartItems(int cartId)
        {
            if (cartId <= 0) throw new ArgumentException("Cart ID must be greater than zero.", nameof(cartId));

            List<CartItem> items = new List<CartItem>();

            string query = @"SELECT ci.*, f.name AS food_name 
                             FROM [CartItem] ci
                             JOIN [FoodItem] f ON ci.food_id = f.food_id
                             WHERE ci.cart_id = @CId";

            SqlParameter[] param =
            {
                new SqlParameter("@CId", cartId)
            };

            var dt = DatabaseHelper.ExecuteQuery(query, param);

            foreach (DataRow row in dt.Rows)
            {
                items.Add(new CartItem
                {
                    CartItemId = Convert.ToInt32(row["cart_item_id"]),
                    CartId = Convert.ToInt32(row["cart_id"]),
                    FoodId = Convert.ToInt32(row["food_id"]),
                    Quantity = Convert.ToInt32(row["quantity"]),
                    UnitPrice = Convert.ToDecimal(row["unit_price"]),
                    Food = new FoodItem
                    {
                        Name = row["food_name"].ToString()
                    }
                });
            }

            return items;
        }

        public decimal GetCartTotal(int cartId)
        {
            if (cartId <= 0) throw new ArgumentException("Cart ID must be greater than zero.", nameof(cartId));

            string query = @"SELECT SUM(quantity * unit_price) 
                             FROM [CartItem] 
                             WHERE cart_id = @CId";

            SqlParameter[] param =
            {
                new SqlParameter("@CId", cartId)
            };

            var result = DatabaseHelper.ExecuteScalar(query, param);

            if (result == DBNull.Value || result == null)
                return 0;

            return Convert.ToDecimal(result);
        }

        public void ClearCart(int cartId)
        {
            if (cartId <= 0) throw new ArgumentException("Cart ID must be greater than zero.", nameof(cartId));

            string query = "DELETE FROM [CartItem] WHERE cart_id = @CId";

            SqlParameter[] param =
            {
                new SqlParameter("@CId", cartId)
            };

            DatabaseHelper.ExecuteNonQuery(query, param);
        }
    }
}