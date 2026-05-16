using System.Collections.Generic;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data.Interfaces
{
    public interface ICustomerDAL
    {
        bool AddCustomer(Customer customer);
        Customer GetCustomerByEmailAndPassword(string email, string password);
        List<Customer> GetAllCustomers();
        bool DeleteCustomer(int customerId);
    }
}
