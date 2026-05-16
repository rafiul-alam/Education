using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Forms;

namespace FoodOrderingSystem.UserControls
{
    public partial class CartUserControl : UserControl
    {
        private CartDAL cartDAL = new CartDAL();
        private int _customerId;
        private int _cartId;
        private DataGridView grid;
        private Label lblTotal;

        public CartUserControl(int customerId, int cartId)
        {
            _customerId = customerId;
            _cartId = cartId;
            InitializeComponent();
            LoadCart();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Padding = new Padding(30, 25, 30, 25);
            this.Font = new Font("Segoe UI", 10f);

            // TITLE
            Label title = new Label
            {
                Text = "MY CART",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // BOTTOM PANEL
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.White
            };

            lblTotal = new Label
            {
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = false,
                Width = 300,
                Height = 44,
                Location = new Point(20, 18),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // CHECKOUT BUTTON
            Button btnCheckout = new Button
            {
                Text = "✔  Proceed to Checkout",
                Width = 210,
                Height = 44,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCheckout.FlatAppearance.BorderSize = 0;

            // CLEAR BUTTON
            Button btnClear = new Button
            {
                Text = "🗑  Clear Cart",
                Width = 140,
                Height = 44,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(239, 68, 68),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClear.FlatAppearance.BorderSize = 1;

            bottomPanel.Resize += (s, e) =>
            {
                btnClear.Location = new Point(bottomPanel.Width - btnClear.Width - 20, 18);
                btnCheckout.Location = new Point(btnClear.Left - btnCheckout.Width - 12, 18);
            };

            btnCheckout.Click += BtnCheckout_Click;
            btnClear.Click += BtnClear_Click;

            bottomPanel.Controls.Add(lblTotal);
            bottomPanel.Controls.Add(btnCheckout);
            bottomPanel.Controls.Add(btnClear);

            // GRID
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                ScrollBars = ScrollBars.Both,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                RowTemplate = { Height = 48 }
            };

            this.Controls.Add(grid);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(title);
        }

        private void LoadCart()
        {
            var items = cartDAL.GetCartItems(_cartId);

            var displayList = items.Select(i => new
            {
                i.Food.Name,
                i.UnitPrice,
                i.Quantity,
                Subtotal = i.UnitPrice * i.Quantity
            }).ToList();

            grid.DataSource = displayList;

            decimal total = items.Sum(i => i.UnitPrice * i.Quantity);
            lblTotal.Text = $"Total: ৳{total:0.00}";
            lblTotal.Tag = total;
        }

        // CLEAR ALL CART
        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all items from cart?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                cartDAL.ClearCart(_cartId);
                LoadCart();
            }
        }

        // CHECKOUT
        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            decimal total = (decimal)(lblTotal.Tag ?? 0m);

            if (total == 0)
            {
                MessageBox.Show("Cart is empty.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var items = cartDAL.GetCartItems(_cartId);
            var checkoutForm = new CheckoutForm(_customerId, items, total);

            if (checkoutForm.ShowDialog() == DialogResult.OK)
                LoadCart();
        }
    }
}