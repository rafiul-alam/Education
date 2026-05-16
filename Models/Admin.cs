using System;

namespace FoodOrderingSystem.Models
{
    public class Admin : User
    {
        public int AdminId 
        { 
            get => Id; 
            set => Id = value; 
        }
    }
}
