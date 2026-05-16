
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Forms
{
    public partial class CheckoutForm : Form
    {
        private int _customerId;
        private List<CartItem> _cartItems;
        private decimal _totalAmount;
        private FoodOrderingSystem.Data.Interfaces.IOrderDAL orderDAL = new OrderDAL();

        private TextBox txtAddress = new TextBox();
        private ComboBox cmbPayment = new ComboBox();

        public CheckoutForm(int customerId, List<CartItem> cartItems, decimal totalAmount)
        {
            _customerId = customerId;
            _cartItems = cartItems;
            _totalAmount = totalAmount;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Checkout";
            this.Size = new System.Drawing.Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblTotal = new Label { Text = $"Total to Pay: ৳{_totalAmount:0.00}", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true, Location = new Point(30, 30) };

            Label lblAddr = new Label { Text = "Delivery Address:", Location = new Point(30, 80), AutoSize = true };
            txtAddress.Location = new Point(30, 100); txtAddress.Width = 300; txtAddress.Multiline = true; txtAddress.Height = 50;
            
            // Assume we can default fetch address in future, leave blank for now

            Label lblPay = new Label { Text = "Payment Method:", Location = new Point(30, 170), AutoSize = true };
            cmbPayment.Location = new Point(30, 190); cmbPayment.Width = 300;
            cmbPayment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPayment.Items.AddRange(new string[] { "Cash on Delivery", "bKash", "Nagad" });
            cmbPayment.SelectedIndex = 0;

            Button btnPlace = new Button { Text = "Place Order", Location = new Point(30, 240), Width = 150, Height = 40, BackColor = Color.Green, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            Button btnCancel = new Button { Text = "Cancel", Location = new Point(200, 240), Width = 130, Height = 40, FlatStyle = FlatStyle.Flat };

            btnPlace.Click += BtnPlace_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(lblTotal);
            this.Controls.Add(lblAddr); this.Controls.Add(txtAddress);
            this.Controls.Add(lblPay);  this.Controls.Add(cmbPayment);
            this.Controls.Add(btnPlace); this.Controls.Add(btnCancel);
        }

        private void BtnPlace_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Please enter a delivery address.", "Error");
                return;
            }

            string payMethod = cmbPayment.SelectedItem.ToString();
            string payStatus = payMethod == "Cash on Delivery" ? "Payment Pending" : "Payment Completed";

            Order newOrder = new Order
            {
                CustomerId = _customerId,
                OrderStatus = "Pending",
                TotalAmount = _totalAmount,
                DeliveryAddress = txtAddress.Text
            };

            Payment newPayment = new Payment
            {
                Method = payMethod,
                Status = payStatus,
                Amount = _totalAmount
            };

            if (orderDAL.PlaceOrder(newOrder, newPayment, _cartItems))
            {
                using (var successForm = new OrderSuccessfulForm())
                {
                    successForm.ShowDialog();
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to place order.", "Error");
            }
        }
    }
}
