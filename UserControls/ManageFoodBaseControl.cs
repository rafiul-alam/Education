using System;
using System.Drawing;
using System.Windows.Forms;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Forms;

namespace FoodOrderingSystem.UserControls
{
    public partial class ManageFoodBaseControl : UserControl
    {
        private FoodOrderingSystem.Data.Interfaces.IFoodItemDAL foodDAL = new FoodItemDAL();
        private int _adminId;
        private DataGridView grid;

        // Colors
        private readonly Color BgPage = Color.FromArgb(244, 246, 249);
        private readonly Color BgCard = Color.White;
        private readonly Color BtnAdd = Color.FromArgb(29, 158, 117);
        private readonly Color BtnAddHov = Color.FromArgb(15, 110, 86);
        private readonly Color BtnRefresh = Color.FromArgb(226, 232, 240);
        private readonly Color BtnRefHov = Color.FromArgb(203, 213, 225);
        private readonly Color BtnDelete = Color.FromArgb(226, 75, 74);
        private readonly Color BtnDelHov = Color.FromArgb(163, 45, 45);
        private readonly Color BtnPrice = Color.FromArgb(186, 117, 23);
        private readonly Color BtnPriceHov = Color.FromArgb(133, 79, 11);
        private readonly Color GridHeader = Color.FromArgb(248, 250, 252);
        private readonly Color GridBorder = Color.FromArgb(226, 232, 240);
        private readonly Color TextPrimary = Color.FromArgb(26, 26, 26);
        private readonly Color TextMuted = Color.FromArgb(100, 116, 139);

        public ManageFoodBaseControl(int adminId)
        {
            _adminId = adminId;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = BgPage;
            this.Padding = new Padding(24);
            this.Font = new Font("Segoe UI", 9.5f);

            // TITLE
            Panel titleRow = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = BgPage
            };

            Label title = new Label
            {
                Text = "Manage Food Items",
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            titleRow.Controls.Add(title);

            // TOOLBAR
            Panel toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = BgPage
            };

            Button btnAdd = MakeButton("Add Food", BtnAdd, BtnAddHov, Color.White, 0);
            Button btnRefresh = MakeButton("Refresh", BtnRefresh, BtnRefHov, TextPrimary, 140);
            Button btnDelete = MakeButton("Delete", BtnDelete, BtnDelHov, Color.White, 280);
            Button btnPrice = MakeButton("Update Price", BtnPrice, BtnPriceHov, Color.White, 420);

            btnAdd.Click += BtnAdd_Click;
            btnRefresh.Click += (s, e) => LoadData();
            btnDelete.Click += BtnDelete_Click;
            btnPrice.Click += BtnUpdatePrice_Click;

            toolbar.Controls.Add(btnAdd);
            toolbar.Controls.Add(btnRefresh);
            toolbar.Controls.Add(btnDelete);
            toolbar.Controls.Add(btnPrice);

            // CARD
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BgCard
            };

            // GRID
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = BgCard,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 90 }
            };

            grid.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Image",
                HeaderText = "Image",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 100
            });

            grid.Columns.Add("FoodId", "ID");
            grid.Columns.Add("Name", "Name");
            grid.Columns.Add("Price", "Price");
            grid.Columns.Add("Category", "Category");
            grid.Columns.Add("Available", "Available");

            card.Controls.Add(grid);

            this.Controls.Add(card);
            this.Controls.Add(toolbar);
            this.Controls.Add(titleRow);
        }

        // BUTTON STYLE
        private Button MakeButton(string text, Color bg, Color hover, Color fg, int x)
        {
            Button b = new Button
            {
                Text = text,
                Width = 130,
                Height = 36,
                Location = new Point(x, 8),
                BackColor = bg,
                ForeColor = fg,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = hover;

            return b;
        }

        // LOAD DATA (FIXED IMAGE HERE)
        private void LoadData()
        {
            grid.Rows.Clear();

            var foods = foodDAL.GetAllFoodItems();

            foreach (var food in foods)
            {
                Image img = null;

                // ✔ FIX: convert byte[] to Image
                if (food.Image != null)
                {
                    using (var ms = new System.IO.MemoryStream(food.Image))
                    {
                        img = Image.FromStream(ms);
                    }
                }

                grid.Rows.Add(
                    img,
                    food.FoodId,
                    food.Name,
                    food.Price,
                    food.Category,
                    food.Available
                );
            }
        }

        // ADD
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new FoodItemForm(_adminId);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        // DELETE
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) return;

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells["FoodId"].Value);

            foodDAL.DeleteFoodItem(id);
            LoadData();
        }

        // UPDATE PRICE
        private void BtnUpdatePrice_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) return;

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells["FoodId"].Value);
            decimal current = Convert.ToDecimal(grid.SelectedRows[0].Cells["Price"].Value);

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter new price:",
                "Update Price",
                current.ToString()
            );

            if (decimal.TryParse(input, out decimal newPrice))
            {
                foodDAL.UpdateFoodItemPrice(id, newPrice);
                LoadData();
            }
        }
    }
}