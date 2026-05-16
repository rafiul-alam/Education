using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.UserControls
{
    public partial class MenuUserControl : UserControl
    {
        private FoodOrderingSystem.Data.Interfaces.IFoodItemDAL foodDAL = new FoodItemDAL();
        private CartDAL cartDAL = new CartDAL();
        private int _cartId;
        private FlowLayoutPanel flowPanel;
        private Label lblTitle;
        private Panel topPanel;

        public MenuUserControl(int cartId)
        {
            _cartId = cartId;
            InitializeComponent();
            LoadMenu();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(15, 15, 25);
            this.Padding = new Padding(0);

            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(28, 28, 45),
                Padding = new Padding(20, 0, 0, 0)
            };

            lblTitle = new Label
            {
                Text = "🍽️  Our Menu",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 107, 53),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            topPanel.Controls.Add(lblTitle);

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(15, 15, 25),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };

            this.Controls.Add(flowPanel);
            this.Controls.Add(topPanel);
        }

        private void LoadMenu()
        {
            flowPanel.Controls.Clear();
            var foods = foodDAL.GetAllFoodItems();

            foreach (var food in foods)
            {
                if (!food.Available) continue;
                flowPanel.Controls.Add(CreateFoodCard(food));
            }
        }

        private Panel CreateFoodCard(FoodItem food)
        {
            Panel card = new Panel
            {
                Size = new Size(210, 370),
                Margin = new Padding(12),
                BackColor = Color.FromArgb(28, 28, 45)
            };

            PictureBox pic = new PictureBox
            {
                Size = new Size(210, 130),
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(40, 40, 65)
            };

            if (food.Image != null && food.Image.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream(food.Image))
                        pic.Image = Image.FromStream(ms);
                }
                catch { pic.Image = null; }
            }

            if (pic.Image == null)
            {
                Label noImg = new Label
                {
                    Text = "🍴",
                    Font = new Font("Segoe UI", 30),
                    ForeColor = Color.Gray,
                    Size = new Size(210, 130),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(40, 40, 65)
                };
                card.Controls.Add(noImg);
            }
            else
            {
                card.Controls.Add(pic);
            }

            Label lblCat = new Label
            {
                Text = food.Category.ToUpper(),
                Font = new Font("Segoe UI", 7, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 107, 53),
                BackColor = Color.FromArgb(60, 35, 20),
                AutoSize = true,
                Location = new Point(8, 138),
                Padding = new Padding(4, 2, 4, 2)
            };

            Label lblName = new Label
            {
                Text = food.Name,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(190, 40),
                Location = new Point(8, 162),
                AutoEllipsis = true
            };

            Label lblPrice = new Label
            {
                Text = $"৳ {food.Price:0.00}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 220, 120),
                AutoSize = true,
                Location = new Point(8, 205)
            };

            Label qtyLabel = new Label
            {
                Text = "Qty:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(8, 238),
                AutoSize = true
            };

            NumericUpDown qtyBox = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 20,
                Value = 1,
                Location = new Point(50, 235),
                Width = 80,
                Height = 28,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                ForeColor = Color.Black,
                TextAlign = HorizontalAlignment.Center
            };

            Button btnAdd = new Button
            {
                Text = "+ Add to Cart",
                Size = new Size(190, 38),
                Location = new Point(8, 278),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 107, 53),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAdd.FlatAppearance.BorderSize = 0;

            btnAdd.Click += (s, e) =>
            {
                int qty = (int)qtyBox.Value;
                cartDAL.AddToCart(_cartId, food.FoodId, qty, food.Price);

                btnAdd.Text = "✓ Added!";
                btnAdd.BackColor = Color.FromArgb(40, 160, 90);

                var timer = new System.Windows.Forms.Timer { Interval = 1500 };
                timer.Tick += (ts, te) =>
                {
                    btnAdd.Text = "+ Add to Cart";
                    btnAdd.BackColor = Color.FromArgb(255, 107, 53);
                    timer.Stop();
                };
                timer.Start();
            };

            card.Controls.Add(lblCat);
            card.Controls.Add(lblName);
            card.Controls.Add(lblPrice);
            card.Controls.Add(qtyLabel);
            card.Controls.Add(qtyBox);
            card.Controls.Add(btnAdd);

            return card;
        }
    }
}