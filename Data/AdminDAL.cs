using System;
using System.Data;
using Microsoft.Data.SqlClient;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data
{
    public class AdminDAL : FoodOrderingSystem.Data.Interfaces.IAdminDAL
    {
        public Admin GetAdminByEmailAndPassword(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            string query = "SELECT * FROM [Admin] WHERE email = @Email AND password = @Password";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", password) 

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Admin
                {
                    AdminId = Convert.ToInt32(row["admin_id"]),
                    Name = row["name"].ToString(),
                    Email = row["email"].ToString(),
                    Password = row["password"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["created_at"])
                };
            }
            return null;
        }

        public void EnsureDefaultAdmin()
        {
            string query = "IF NOT EXISTS(SELECT 1 FROM [Admin]) BEGIN INSERT INTO [Admin] (name, email, password) VALUES ('Super Admin', 'admin@admin.com', 'admin123') END";
            DatabaseHelper.ExecuteNonQuery(query);
        }
    }
}
