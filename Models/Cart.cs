using System;
using System.Collections.Generic;

namespace FoodOrderingSystem.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
