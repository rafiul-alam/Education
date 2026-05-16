using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodOrderingSystem.Forms
{
    public class OrderSuccessfulForm : Form
    {
        public OrderSuccessfulForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Order Status";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            Label lblSuccess = new Label
            {
                Text = "ORDER SUCCESSFUL",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.Green,
                AutoSize = true,
                Location = new Point(60, 80)
            };

            Label lblThanks = new Label
            {
                Text = "Thank you for your order!",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(100, 130)
            };

            Button btnBack = new Button
            {
                Text = "Back to Menu",
                Location = new Point(125, 180),
                Width = 130,
                Height = 40,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnBack.Click += (s, e) => this.Close();

            this.Controls.Add(lblSuccess);
            this.Controls.Add(lblThanks);
            this.Controls.Add(btnBack);
        }
    }
}
