using System;
using System.Drawing;
using System.Windows.Forms;
using FoodOrderingSystem.Data;

namespace FoodOrderingSystem.UserControls
{
    public partial class ManageOrdersControl : UserControl
    {
        private FoodOrderingSystem.Data.Interfaces.IOrderDAL orderDAL = new OrderDAL();
        private DataGridView grid;
        private ComboBox cmbStatus;

        public ManageOrdersControl()
        {
            InitializeComponent();
            LoadData();
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
                Text = "ORDER MANAGEMENT",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // TOOLBAR PANEL
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = Color.White,
                Padding = new Padding(16, 0, 16, 0)
            };

            topPanel.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(
                    new Pen(Color.FromArgb(226, 232, 240), 1),
                    0, topPanel.Height - 1, topPanel.Width, topPanel.Height - 1);
            };

            Label lblStatus = new Label
            {
                Text = "Order Status:",
                AutoSize = false,
                Width = 110,
                Height = 40,
                Location = new Point(16, 18),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                TextAlign = ContentAlignment.MiddleRight
            };

            cmbStatus = new ComboBox
            {
                Location = new Point(136, 20),
                Width = 180,
                Height = 36,
                Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            cmbStatus.Items.AddRange(new string[]
            {
                "Pending", "Accepted", "Delivering", "Completed", "Cancelled"
            });
            cmbStatus.SelectedIndex = 0;

            Button btnUpdate = new Button
            {
                Text = "✔  Update Status",
                Width = 160,
                Height = 38,
                Location = new Point(330, 19),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);

            Button btnRefresh = new Button
            {
                Text = "⟳  Refresh",
                Width = 120,
                Height = 38,
                Location = new Point(502, 19),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(71, 85, 105),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 1;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 232, 240);

            btnUpdate.Click += BtnUpdate_Click;
            btnRefresh.Click += (s, e) => LoadData();

            topPanel.Controls.Add(lblStatus);
            topPanel.Controls.Add(cmbStatus);
            topPanel.Controls.Add(btnUpdate);
            topPanel.Controls.Add(btnRefresh);

            // SPACER
            Panel spacer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 16,
                BackColor = Color.FromArgb(244, 246, 249)
            };

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
                GridColor = Color.FromArgb(226, 232, 240),
                RowTemplate = { Height = 48 },
                Font = new Font("Segoe UI", 10f),
                ColumnHeadersHeight = 50,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowHeadersWidth = 32
            };

            grid.EnableHeadersVisualStyles = false;

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                SelectionBackColor = Color.FromArgb(30, 41, 59)
            };

            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10f),
                Padding = new Padding(10, 0, 0, 0),
                ForeColor = Color.FromArgb(30, 41, 59),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(30, 41, 59),
                BackColor = Color.White
            };

            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252),
                Font = new Font("Segoe UI", 10f),
                Padding = new Padding(10, 0, 0, 0),
                ForeColor = Color.FromArgb(30, 41, 59),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(30, 41, 59)
            };

            grid.RowHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252),
                SelectionBackColor = Color.FromArgb(219, 234, 254)
            };

            // Register CellFormatting ONCE here (not inside LoadData)
            grid.CellFormatting += Grid_CellFormatting;

            // ADD CONTROLS
            this.Controls.Add(grid);
            this.Controls.Add(spacer);
            this.Controls.Add(topPanel);
            this.Controls.Add(title);
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (grid.Columns[e.ColumnIndex].Name == "OrderStatus" && e.Value != null)
            {
                string status = e.Value.ToString();
                Color bg = status switch
                {
                    "Pending" => Color.FromArgb(255, 243, 205),
                    "Accepted" => Color.FromArgb(219, 234, 254),
                    "Delivering" => Color.FromArgb(254, 240, 138),
                    "Completed" => Color.FromArgb(220, 252, 231),
                    "Cancelled" => Color.FromArgb(254, 226, 226),
                    _ => Color.White
                };
                grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = bg;
            }
        }

        private void LoadData()
        {
            grid.DataSource = orderDAL.GetAllOrders();

            // Rename columns to readable labels
            if (grid.Columns.Contains("OrderId")) grid.Columns["OrderId"].HeaderText = "Order ID";
            if (grid.Columns.Contains("CustomerId")) grid.Columns["CustomerId"].HeaderText = "Customer";
            if (grid.Columns.Contains("OrderStatus")) grid.Columns["OrderStatus"].HeaderText = "Status";
            if (grid.Columns.Contains("TotalAmount")) grid.Columns["TotalAmount"].HeaderText = "Amount (৳)";
            if (grid.Columns.Contains("DeliveryAddress")) grid.Columns["DeliveryAddress"].HeaderText = "Delivery Address";
            if (grid.Columns.Contains("PlacedAt")) grid.Columns["PlacedAt"].HeaderText = "Placed At";
            if (grid.Columns.Contains("CancelledAt")) grid.Columns["CancelledAt"].HeaderText = "Cancelled At";
            if (grid.Columns.Contains("PaymentDetails")) grid.Columns["PaymentDetails"].HeaderText = "Payment";

            // Minimum width so no column is too narrow
            foreach (DataGridViewColumn col in grid.Columns)
                col.MinimumWidth = 110;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order first.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells["OrderId"].Value);
            string status = cmbStatus.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Please select a status.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            orderDAL.UpdateOrderStatus(id, status);
            LoadData();

            MessageBox.Show("Order updated successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}