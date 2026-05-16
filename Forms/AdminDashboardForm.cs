using System;
using System.Drawing;
using System.Windows.Forms;
using FoodOrderingSystem.UserControls;

namespace FoodOrderingSystem.Forms
{
    public partial class AdminDashboardForm : Form
    {
        private int _adminId;
        private Panel mainPanel;
        private Button _activeNavButton = null;

        // Nav colors
        private readonly Color NavBg = Color.FromArgb(18, 28, 38);
        private readonly Color NavHover = Color.FromArgb(30, 45, 60);
        private readonly Color NavActive = Color.FromArgb(29, 158, 117);  // teal accent
        private readonly Color NavText = Color.FromArgb(200, 210, 220);
        private readonly Color NavTextActive = Color.White;
        private readonly Color HeaderBg = Color.FromArgb(24, 36, 48);
        private readonly Color ContentBg = Color.FromArgb(244, 246, 249);

        public AdminDashboardForm(int adminId)
        {
            _adminId = adminId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Food Ordering System — Admin";
            this.Size = new Size(1100, 720);
            this.MinimumSize = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ContentBg;
            this.FormClosed += (s, e) => Application.Exit();
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // ── Sidebar ─────────────────────────────────────────────
            Panel navPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 210,
                BackColor = NavBg
            };

            // Logo / App name at top of sidebar
            Panel logoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = HeaderBg,
                Padding = new Padding(16, 0, 0, 0)
            };
            Label lblLogo = new Label
            {
                Text = "🍽  FoodAdmin",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            logoPanel.Controls.Add(lblLogo);

            // Admin info strip
            Panel adminStrip = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.FromArgb(22, 33, 44),
                Padding = new Padding(16, 0, 0, 0)
            };
            Label lblAdmin = new Label
            {
                Text = $"Admin ID: {_adminId}",
                ForeColor = Color.FromArgb(120, 160, 180),
                Font = new Font("Segoe UI", 8.5f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            adminStrip.Controls.Add(lblAdmin);

            // Nav buttons
            Button btnManageFood = CreateNavButton("🍛   Manage Food");
            Button btnManageOrders = CreateNavButton("📋   Manage Orders");
            Button btnManageUsers = CreateNavButton("👥   Manage Users");

            btnManageFood.Click += (s, e) => { SetActive(btnManageFood); LoadControl(new ManageFoodBaseControl(_adminId)); };
            btnManageOrders.Click += (s, e) => { SetActive(btnManageOrders); LoadControl(new ManageOrdersControl()); };
            btnManageUsers.Click += (s, e) => { SetActive(btnManageUsers); LoadControl(new ManageUsersControl()); };

            // Separator before logout
            Panel sep = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Color.FromArgb(40, 55, 70)
            };

            // Logout at bottom
            Button btnLogout = new Button
            {
                Text = "⏻   Logout",
                Dock = DockStyle.Bottom,
                Height = 50,
                ForeColor = Color.FromArgb(220, 90, 90),
                BackColor = NavBg,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 30, 30);
            btnLogout.Click += (s, e) =>
            {
                var result = MessageBox.Show(
                    "Are you sure you want to logout?",
                    "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    new LoginForm().Show();
                    this.Hide();
                }
            };

            navPanel.Controls.Add(btnManageUsers);
            navPanel.Controls.Add(btnManageOrders);
            navPanel.Controls.Add(btnManageFood);
            navPanel.Controls.Add(adminStrip);
            navPanel.Controls.Add(logoPanel);
            navPanel.Controls.Add(sep);
            navPanel.Controls.Add(btnLogout);

            // ── Top header bar ───────────────────────────────────────
            Panel headerBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(20, 0, 20, 0)
            };
            // bottom border line on header
            headerBar.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(
                    new Pen(Color.FromArgb(226, 232, 240)),
                    0, headerBar.Height - 1, headerBar.Width, headerBar.Height - 1);
            };

            Label lblPageTitle = new Label
            {
                Name = "lblPageTitle",
                Text = "Manage Food",
                ForeColor = Color.FromArgb(26, 26, 26),
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Label lblDateTime = new Label
            {
                Text = DateTime.Now.ToString("dddd, dd MMM yyyy"),
                ForeColor = Color.FromArgb(100, 116, 139),
                Font = new Font("Segoe UI", 9f),
                Dock = DockStyle.Right,
                Width = 220,
                TextAlign = ContentAlignment.MiddleRight
            };
            headerBar.Controls.Add(lblPageTitle);
            headerBar.Controls.Add(lblDateTime);

            // ── Main content area ────────────────────────────────────
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ContentBg,
                Padding = new Padding(20)
            };

            // Add in correct z-order
            this.Controls.Add(mainPanel);
            this.Controls.Add(headerBar);
            this.Controls.Add(navPanel);

            // Default: highlight Manage Food and load it
            SetActive(btnManageFood);
            LoadControl(new ManageFoodBaseControl(_adminId));
        }

        // ── Helpers ─────────────────────────────────────────────────

        private Button CreateNavButton(string text)
        {
            Button btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 50,
                ForeColor = NavText,
                BackColor = NavBg,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = NavHover;
            return btn;
        }

        private void SetActive(Button btn)
        {
            // Reset previous active button
            if (_activeNavButton != null)
            {
                _activeNavButton.BackColor = NavBg;
                _activeNavButton.ForeColor = NavText;
                _activeNavButton.FlatAppearance.MouseOverBackColor = NavHover;
            }

            // Highlight new active button
            btn.BackColor = NavActive;
            btn.ForeColor = NavTextActive;
            btn.FlatAppearance.MouseOverBackColor = NavActive;
            _activeNavButton = btn;

            // Update header page title
            var lbl = this.Controls.Find("lblPageTitle", true);
            if (lbl.Length > 0)
            {
                // Strip the emoji prefix for the title
                string raw = btn.Text.Trim();
                int spaceIdx = raw.IndexOf("   ");
                lbl[0].Text = spaceIdx >= 0 ? raw.Substring(spaceIdx).Trim() : raw;
            }
        }

        private void LoadControl(UserControl control)
        {
            mainPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(control);
            control.BringToFront();
        }
    }
}