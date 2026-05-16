using System;
using System.Windows.Forms;
using FoodOrderingSystem.Data;

namespace FoodOrderingSystem.Forms
{
    public partial class LoginForm : Form
    {
        private FoodOrderingSystem.Data.Interfaces.IAdminDAL adminDAL = new AdminDAL();
        private FoodOrderingSystem.Data.Interfaces.ICustomerDAL customerDAL = new CustomerDAL();

        public LoginForm()
        {
            InitializeComponent();
            adminDAL.EnsureDefaultAdmin(); // Create default admin if not exists
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter email and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rbAdmin.Checked)
            {
                var admin = adminDAL.GetAdminByEmailAndPassword(email, password);
                if (admin != null)
                {
                    MessageBox.Show("Login Successful as Admin!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Launch Admin Dashboard (handle later)
                    var adminForm = new AdminDashboardForm(admin.AdminId);
                    adminForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid admin credentials.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var customer = customerDAL.GetCustomerByEmailAndPassword(email, password);
                if (customer != null)
                {
                    MessageBox.Show("Login Successful as Customer!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Launch Customer Dashboard (handle later)
                    var customerForm = new CustomerDashboardForm(customer.CustomerId);
                    customerForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid customer credentials.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var regForm = new CustomerRegistrationForm();
            regForm.ShowDialog();
        }
    }
}
