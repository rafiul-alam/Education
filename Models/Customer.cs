using System;

namespace FoodOrderingSystem.Models
{
    public class Customer : User
    {
        public int CustomerId 
        { 
            get => Id; 
            set => Id = value; 
        }

        private string _phone;
        public string Phone 
        { 
            get => _phone; 
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Phone cannot be empty.");
                _phone = value;
            }
        }

        private string _address;
        public string Address 
        { 
            get => _address; 
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Address cannot be empty.");
                _address = value;
            }
        }

        public DateTime? DeletedAt { get; set; }
    }
}
