using System.Collections.Generic;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data.Interfaces
{
    public interface IAdminDAL
    {
        Admin GetAdminByEmailAndPassword(string email, string password);
        void EnsureDefaultAdmin();
    }
}
