using System;
using System.Drawing;
using System.Windows.Forms;
using FoodOrderingSystem.Data;

namespace FoodOrderingSystem.UserControls
{
    public partial class OrderHistoryUserControl : UserControl
    {
        private FoodOrderingSystem.Data.Interfaces.IOrderDAL orderDAL = new OrderDAL();
        private int _customerId;
        private DataGridView grid;

        public OrderHistoryUserControl(int customerId)
        {
            _customerId = customerId;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(15);
            this.BackColor = Color.WhiteSmoke;

            // ===== TITLE =====
            Label title = new Label
            {
                Text = "My Orders",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 10, 0, 10)
            };

            // ===== TOP PANEL =====
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // ===== BUTTONS =====
            Button btnCancel = new Button
            {
                Text = "Cancel Selected Order",
                Width = 220,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancel.FlatAppearance.BorderColor = Color.Red;

            Button btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 120,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            FlowLayoutPanel buttonLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                WrapContents = false,
                Padding = new Padding(5)
            };

            buttonLayout.Controls.Add(btnCancel);
            buttonLayout.Controls.Add(btnRefresh);

            topPanel.Controls.Add(buttonLayout);

            // ===== DATA GRID VIEW (FIXED TEXT CUT ISSUE) =====
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,

                ColumnHeadersHeight = 50,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,

                RowTemplate = { Height = 40 },

                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10),
                    Padding = new Padding(6),
                    WrapMode = DataGridViewTriState.True
                },

                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    WrapMode = DataGridViewTriState.True
                }
            };

            // ===== EVENTS =====
            btnCancel.Click += BtnCancel_Click;
            btnRefresh.Click += (s, e) => LoadData();

            // ===== ADD CONTROLS =====
            this.Controls.Add(grid);
            this.Controls.Add(topPanel);
            this.Controls.Add(title);
        }

        private void LoadData()
        {
            try
            {
                if (grid == null)
                {
                    return;
                }

                grid.DataSource = orderDAL.GetOrdersByCustomer(_customerId);

                // extra safety for status column visibility
                if (grid.Columns.Contains("OrderStatus"))
                {
                    var column = grid.Columns["OrderStatus"];
                    if (column != null)
                    {
                        column.MinimumWidth = 130;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading order history: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count > 0)
            {
                var statusValue = grid.SelectedRows[0].Cells["OrderStatus"].Value;
                string status = statusValue?.ToString() ?? "Unknown";

                if (status != "Pending")
                {
                    MessageBox.Show("You can only cancel Pending orders.", "Warning");
                    return;
                }

                if (grid.SelectedRows[0].Cells["OrderId"].Value is int id)
                {
                    if (MessageBox.Show("Are you sure you want to cancel this order?", "Confirm",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        orderDAL.UpdateOrderStatus(id, "Cancelled");
                        LoadData();
                        MessageBox.Show("Order cancelled.", "Success");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Order ID.", "Error");
                }
            }
        }
    }
}