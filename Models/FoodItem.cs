using System;

namespace FoodOrderingSystem.Models
{
    public class FoodItem
    {
        public int FoodId { get; set; }
        public int AdminId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private decimal _price;
        public decimal Price 
        { 
            get => _price; 
            set 
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public string Category { get; set; }
        public byte[] Image { get; set; }
        public bool Available { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
