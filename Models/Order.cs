using System;
using System.Collections.Generic;

namespace FoodOrderingSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment PaymentDetails { get; set; }
    }
}
