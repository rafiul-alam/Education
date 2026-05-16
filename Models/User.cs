using System;

namespace FoodOrderingSystem.Models
{
    public abstract class User
    {
        private string _email;
        private string _password;
        private string _name;

        public int Id { get; set; } 

        public string Name
        {
            get { return _name; }
            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                _name = value; 
            }
        }

        public string Email
        {
            get { return _email; }
            set 
            { 
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@") || !value.Contains("."))
                    throw new ArgumentException("Invalid email format.");
                _email = value; 
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password cannot be empty.");
                _password = value; 
            }
        }
                    
        public DateTime CreatedAt { get; set; }
    }
}
