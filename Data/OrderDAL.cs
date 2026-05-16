using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data
{
    public class OrderDAL : FoodOrderingSystem.Data.Interfaces.IOrderDAL
    {
        public bool PlaceOrder(Order order, Payment payment, List<CartItem> cartItems)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (payment == null) throw new ArgumentNullException(nameof(payment));
            if (cartItems == null || cartItems.Count == 0) throw new ArgumentException("Cart items cannot be null or empty.", nameof(cartItems));

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insert Order
                        string orderQuery = @"INSERT INTO [Order] (customer_id, order_status, total_amount, delivery_address, placed_at) 
                                              OUTPUT INSERTED.order_id
                                              VALUES (@CustomerId, @OrderStatus, @TotalAmount, @DeliveryAddress, @PlacedAt)";
                        SqlCommand orderCmd = new SqlCommand(orderQuery, conn, transaction);
                        orderCmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                        orderCmd.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                        orderCmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                        orderCmd.Parameters.AddWithValue("@DeliveryAddress", order.DeliveryAddress);
                        orderCmd.Parameters.AddWithValue("@PlacedAt", DateTime.Now);

                        int orderId = (int)orderCmd.ExecuteScalar();

                        // 2. Insert Payment
                        string paymentQuery = @"INSERT INTO [Payment] (order_id, method, status, amount, transaction_ref, paid_at)
                                                VALUES (@OrderId, @Method, @Status, @Amount, @TransactionRef, @PaidAt)";
                        SqlCommand paymentCmd = new SqlCommand(paymentQuery, conn, transaction);
                        paymentCmd.Parameters.AddWithValue("@OrderId", orderId);
                        paymentCmd.Parameters.AddWithValue("@Method", payment.Method);
                        paymentCmd.Parameters.AddWithValue("@Status", payment.Status);
                        paymentCmd.Parameters.AddWithValue("@Amount", payment.Amount);
                        paymentCmd.Parameters.AddWithValue("@TransactionRef", (object)payment.TransactionRef ?? DBNull.Value);
                        paymentCmd.Parameters.AddWithValue("@PaidAt", (object)payment.PaidAt ?? DBNull.Value);
                        paymentCmd.ExecuteNonQuery();

                        // 3. Insert Order Items
                        foreach (var item in cartItems)
                        {
                            string itemQuery = @"INSERT INTO [OrderItem] (order_id, food_id, quantity, unit_price, subtotal)
                                                 VALUES (@OrderId, @FoodId, @Quantity, @UnitPrice, @Subtotal)";
                            SqlCommand itemCmd = new SqlCommand(itemQuery, conn, transaction);
                            itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                            itemCmd.Parameters.AddWithValue("@FoodId", item.FoodId);
                            itemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            itemCmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                            itemCmd.Parameters.AddWithValue("@Subtotal", item.Quantity * item.UnitPrice);
                            itemCmd.ExecuteNonQuery();
                        }

                        // 4. Clear User's Cart
                
                        string cartQuery = @"DELETE FROM [CartItem] WHERE cart_id IN (SELECT cart_id FROM [Cart] WHERE customer_id = @CustomerId)";
                        SqlCommand cartCmd = new SqlCommand(cartQuery, conn, transaction);
                        cartCmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                        cartCmd.ExecuteNonQuery();

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error placing order: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            string query = "SELECT * FROM [Order]";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                orders.Add(new Order
                {
                    OrderId = Convert.ToInt32(row["order_id"]),
                    CustomerId = Convert.ToInt32(row["customer_id"]),
                    OrderStatus = row["order_status"].ToString(),
                    TotalAmount = Convert.ToDecimal(row["total_amount"]),
                    DeliveryAddress = row["delivery_address"].ToString(),
                    PlacedAt = Convert.ToDateTime(row["placed_at"]),
                    CancelledAt = row["cancelled_at"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["cancelled_at"]) : null
                });
            }
            return orders;
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            if (customerId <= 0) throw new ArgumentException("Customer ID must be greater than zero.", nameof(customerId));

            List<Order> orders = new List<Order>();
            string query = "SELECT * FROM [Order] WHERE customer_id = @Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", customerId)
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                orders.Add(new Order
                {
                    OrderId = Convert.ToInt32(row["order_id"]),
                    CustomerId = Convert.ToInt32(row["customer_id"]),
                    OrderStatus = row["order_status"].ToString(),
                    TotalAmount = Convert.ToDecimal(row["total_amount"]),
                    DeliveryAddress = row["delivery_address"].ToString(),
                    PlacedAt = Convert.ToDateTime(row["placed_at"]),
                    CancelledAt = row["cancelled_at"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["cancelled_at"]) : null
                });
            }
            return orders;
        }

        public bool UpdateOrderStatus(int orderId, string status)
        {
            if (orderId <= 0) throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status cannot be null or empty.", nameof(status));

            string query = "UPDATE [Order] SET order_status = @Status WHERE order_id = @Id";
            if (status == "Cancelled")
            {
                query = "UPDATE [Order] SET order_status = @Status, cancelled_at = GETDATE() WHERE order_id = @Id";
            }
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Status", status),
                new SqlParameter("@Id", orderId)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
