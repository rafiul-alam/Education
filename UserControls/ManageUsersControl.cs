using System;
using System.Drawing;
using System.Windows.Forms;
using FoodOrderingSystem.Data;

namespace FoodOrderingSystem.UserControls
{
    public partial class ManageUsersControl : UserControl
    {
        private FoodOrderingSystem.Data.Interfaces.ICustomerDAL customerDAL = new CustomerDAL();
        private DataGridView grid;

        public ManageUsersControl()
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
                Text = "USER MANAGEMENT",
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

            // Total Users label (will be updated after LoadData)
            Label lblCount = new Label
            {
                Name = "lblCount",
                Text = "Total Users: —",
                AutoSize = false,
                Width = 200,
                Height = 40,
                Location = new Point(16, 18),
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Delete button
            Button btnDelete = new Button
            {
                Text = "🗑  Delete User",
                Width = 160,
                Height = 38,
                Location = new Point(230, 19),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);

            // Refresh button
            Button btnRefresh = new Button
            {
                Text = "⟳  Refresh",
                Width = 130,
                Height = 38,
                Location = new Point(402, 19),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(71, 85, 105),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 1;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 232, 240);

            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += (s, e) => LoadData();

            topPanel.Controls.Add(lblCount);
            topPanel.Controls.Add(btnDelete);
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
                SelectionBackColor = Color.FromArgb(254, 226, 226),
                SelectionForeColor = Color.FromArgb(30, 41, 59),
                BackColor = Color.White
            };

            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252),
                Font = new Font("Segoe UI", 10f),
                Padding = new Padding(10, 0, 0, 0),
                ForeColor = Color.FromArgb(30, 41, 59),
                SelectionBackColor = Color.FromArgb(254, 226, 226),
                SelectionForeColor = Color.FromArgb(30, 41, 59)
            };

            grid.RowHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252),
                SelectionBackColor = Color.FromArgb(254, 226, 226)
            };

            // ADD CONTROLS
            this.Controls.Add(grid);
            this.Controls.Add(spacer);
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

                grid.DataSource = customerDAL.GetAllCustomers();

                // Rename columns to readable labels
                if (grid.Columns.Contains("CustomerId"))
                {
                    var col = grid.Columns["CustomerId"];
                    if (col != null) col.HeaderText = "User ID";
                }
                if (grid.Columns.Contains("FullName"))
                {
                    var col = grid.Columns["FullName"];
                    if (col != null) col.HeaderText = "Full Name";
                }
                if (grid.Columns.Contains("Email"))
                {
                    var col = grid.Columns["Email"];
                    if (col != null) col.HeaderText = "Email Address";
                }
                if (grid.Columns.Contains("Phone"))
                {
                    var col = grid.Columns["Phone"];
                    if (col != null) col.HeaderText = "Phone";
                }
                if (grid.Columns.Contains("Address"))
                {
                    var col = grid.Columns["Address"];
                    if (col != null) col.HeaderText = "Address";
                }
                if (grid.Columns.Contains("CreatedAt"))
                {
                    var col = grid.Columns["CreatedAt"];
                    if (col != null) col.HeaderText = "Joined At";
                }
                if (grid.Columns.Contains("Password"))
                {
                    var col = grid.Columns["Password"];
                    if (col != null) col.HeaderText = "Password";
                }
                if (grid.Columns.Contains("Username"))
                {
                    var col = grid.Columns["Username"];
                    if (col != null) col.HeaderText = "Username";
                }

                foreach (DataGridViewColumn col in grid.Columns)
                {
                    if (col != null)
                        col.MinimumWidth = 110;
                }

                // Update count label
                var lbl = this.Controls.Find("lblCount", true);
                if (lbl.Length > 0)
                    lbl[0].Text = $"Total Users: {grid.Rows.Count}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading user data: {ex.Message}");
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a user to delete.", "Notice",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var idCell = grid.SelectedRows[0].Cells["CustomerId"]?.Value;
                if (idCell == null || !int.TryParse(idCell.ToString(), out int id))
                {
                    MessageBox.Show("Invalid user ID.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirm = MessageBox.Show(
                    "Are you sure you want to delete this user?\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    customerDAL.DeleteCustomer(id);
                    LoadData();
                    MessageBox.Show("User deleted successfully.", "Done",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting user: {ex.Message}");
                MessageBox.Show($"Error deleting user: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}