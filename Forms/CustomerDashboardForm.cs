using System;
using System.Windows.Forms;
using FoodOrderingSystem.UserControls;
using FoodOrderingSystem.Data;

namespace FoodOrderingSystem.Forms
{
    public partial class CustomerDashboardForm : Form
    {
        private int _customerId;
        private int _cartId;
        private Panel mainPanel;
        private CartDAL cartDAL = new CartDAL();

        public CustomerDashboardForm(int customerId)
        {
            _customerId = customerId;
            _cartId = cartDAL.GetOrCreateCart(_customerId);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Customer Dashboard";
            this.Size = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += (s, e) => Application.Exit();

            Panel navPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = System.Drawing.Color.FromArgb(41, 53, 65)
            };

            Button btnMenu = new Button { Text = "Food Menu", Dock = DockStyle.Top, Height = 50, ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat };
            Button btnCart = new Button { Text = "My Cart", Dock = DockStyle.Top, Height = 50, ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat };
            Button btnOrders = new Button { Text = "My Orders", Dock = DockStyle.Top, Height = 50, ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat };
            Button btnLogout = new Button { Text = "Logout", Dock = DockStyle.Bottom, Height = 50, ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat };

            btnMenu.Click += (s, e) => LoadControl(new MenuUserControl(_cartId));
            btnCart.Click += (s, e) => LoadControl(new CartUserControl(_customerId, _cartId));
            btnOrders.Click += (s, e) => LoadControl(new OrderHistoryUserControl(_customerId));
            btnLogout.Click += (s, e) => {
                new LoginForm().Show();
                this.Hide();
            };

            navPanel.Controls.Add(btnOrders);
            navPanel.Controls.Add(btnCart);
            navPanel.Controls.Add(btnMenu);
            navPanel.Controls.Add(btnLogout);

            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White
            };

            this.Controls.Add(mainPanel);
            this.Controls.Add(navPanel);

            LoadControl(new MenuUserControl(_cartId));
        }

        public void LoadControl(UserControl control)
        {
            mainPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(control);
        }
    }
}
