using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data
{
    public class CustomerDAL : FoodOrderingSystem.Data.Interfaces.ICustomerDAL
    {
        public bool AddCustomer(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (string.IsNullOrWhiteSpace(customer.Name)) throw new ArgumentException("Name cannot be null or empty.", nameof(customer.Name));
            if (string.IsNullOrWhiteSpace(customer.Email)) throw new ArgumentException("Email cannot be null or empty.", nameof(customer.Email));
            if (string.IsNullOrWhiteSpace(customer.Password)) throw new ArgumentException("Password cannot be null or empty.", nameof(customer.Password));

            string query = "INSERT INTO [Customer] (name, email, password, phone, address) VALUES (@Name, @Email, @Password, @Phone, @Address)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", customer.Name),
                new SqlParameter("@Email", customer.Email),
                new SqlParameter("@Password", customer.Password),
                new SqlParameter("@Phone", customer.Phone),
                new SqlParameter("@Address", customer.Address)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public Customer GetCustomerByEmailAndPassword(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            string query = "SELECT * FROM [Customer] WHERE email = @Email AND password = @Password AND deleted_at IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", password)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Customer
                {
                    CustomerId = Convert.ToInt32(row["customer_id"]),
                    Name = row["name"].ToString(),
                    Email = row["email"].ToString(),
                    Password = row["password"].ToString(),
                    Phone = row["phone"].ToString(),
                    Address = row["address"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["created_at"])
                };
            }
            return null;
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            string query = "SELECT * FROM [Customer] WHERE deleted_at IS NULL";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                customers.Add(new Customer
                {
                    CustomerId = Convert.ToInt32(row["customer_id"]),
                    Name = row["name"].ToString(),
                    Email = row["email"].ToString(),
                    Phone = row["phone"].ToString(),
                    Address = row["address"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["created_at"])
                });
            }
            return customers;
        }

        public bool DeleteCustomer(int customerId)
        {
            if (customerId <= 0) throw new ArgumentException("Customer ID must be greater than zero.", nameof(customerId));

            // Soft delete
            string query = "UPDATE [Customer] SET deleted_at = GETDATE() WHERE customer_id = @Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", customerId)
            };
            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
    }
}
