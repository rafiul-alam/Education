using System.Collections.Generic;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data.Interfaces
{
    public interface IOrderDAL
    {
        bool PlaceOrder(Order order, Payment payment, List<CartItem> cartItems);
        List<Order> GetAllOrders();
        List<Order> GetOrdersByCustomer(int customerId);
        bool UpdateOrderStatus(int orderId, string status);
    }
}
