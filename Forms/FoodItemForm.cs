using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Forms
{
    public partial class FoodItemForm : Form
    {
        private int _adminId;
        private FoodOrderingSystem.Data.Interfaces.IFoodItemDAL foodDAL = new FoodItemDAL();
        private byte[] _imageBytes = null;

        private TextBox txtName = new TextBox();
        private TextBox txtDescription = new TextBox();
        private TextBox txtPrice = new TextBox();
        private TextBox txtCategory = new TextBox();
        private CheckBox chkAvailable = new CheckBox { Text = "Available", Checked = true };
        private PictureBox picPreview = new PictureBox();
        private Button btnPickImage = new Button();

        public FoodItemForm(int adminId)
        {
            _adminId = adminId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Add Food Item";
            this.Size = new Size(440, 580);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(28, 28, 45);

            // Helper to create labels
            Label MakeLabel(string text, int y)
            {
                return new Label
                {
                    Text = text,
                    Location = new Point(30, y),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(200, 200, 220),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    BackColor = Color.Transparent
                };
            }

            void StyleTextBox(TextBox tb, int y, int width = 340)
            {
                tb.Location = new Point(30, y);
                tb.Width = width;
                tb.Height = 30;
                tb.BackColor = Color.FromArgb(40, 40, 65);
                tb.ForeColor = Color.White;
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.Font = new Font("Segoe UI", 10);
            }

            // Name
            this.Controls.Add(MakeLabel("FOOD NAME", 20));
            StyleTextBox(txtName, 42);
            this.Controls.Add(txtName);

            // Description
            this.Controls.Add(MakeLabel("DESCRIPTION", 85));
            StyleTextBox(txtDescription, 107);
            this.Controls.Add(txtDescription);

            // Price
            this.Controls.Add(MakeLabel("PRICE (৳)", 150));
            StyleTextBox(txtPrice, 172, 160);
            this.Controls.Add(txtPrice);

            // Category
            this.Controls.Add(MakeLabel("CATEGORY", 150));
            Label lblCat = MakeLabel("CATEGORY", 150);
            lblCat.Location = new Point(210, 150);
            this.Controls.Add(lblCat);
            StyleTextBox(txtCategory, 172, 160);
            txtCategory.Location = new Point(210, 172);
            this.Controls.Add(txtCategory);

            // Available checkbox
            chkAvailable.Location = new Point(30, 215);
            chkAvailable.ForeColor = Color.FromArgb(200, 200, 220);
            chkAvailable.BackColor = Color.Transparent;
            chkAvailable.Font = new Font("Segoe UI", 10);
            this.Controls.Add(chkAvailable);

            // Image section
            this.Controls.Add(MakeLabel("FOOD IMAGE", 255));

            picPreview.Location = new Point(30, 278);
            picPreview.Size = new Size(180, 130);
            picPreview.BorderStyle = BorderStyle.FixedSingle;
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.BackColor = Color.FromArgb(40, 40, 65);
            this.Controls.Add(picPreview);

            // No image placeholder label
            Label lblNoImg = new Label
            {
                Text = "🍴\nNo Image",
                Location = new Point(30, 278),
                Size = new Size(180, 130),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(100, 100, 130),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10),
                Name = "lblNoImg"
            };
            this.Controls.Add(lblNoImg);

            btnPickImage.Text = "📁  Choose Image";
            btnPickImage.Location = new Point(220, 278);
            btnPickImage.Size = new Size(150, 40);
            btnPickImage.FlatStyle = FlatStyle.Flat;
            btnPickImage.BackColor = Color.FromArgb(50, 50, 80);
            btnPickImage.ForeColor = Color.White;
            btnPickImage.Font = new Font("Segoe UI", 9);
            btnPickImage.FlatAppearance.BorderSize = 0;
            btnPickImage.Cursor = Cursors.Hand;
            btnPickImage.Click += BtnPickImage_Click;
            this.Controls.Add(btnPickImage);

            Label lblHint = new Label
            {
                Text = "JPG, PNG supported\nMax recommended: 1MB",
                Location = new Point(220, 325),
                Size = new Size(150, 40),
                ForeColor = Color.FromArgb(120, 120, 150),
                Font = new Font("Segoe UI", 8),
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblHint);

            // Save button
            Button btnSave = new Button
            {
                Text = "SAVE FOOD ITEM",
                Location = new Point(30, 430),
                Size = new Size(340, 45),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 107, 53),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            // Cancel link
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(30, 485),
                Size = new Size(340, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(150, 150, 170),
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private void BtnPickImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Select Food Image";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _imageBytes = File.ReadAllBytes(ofd.FileName);

                    using (var ms = new MemoryStream(_imageBytes))
                    {
                        picPreview.Image = Image.FromStream(ms);
                    }

                    // Hide the placeholder label
                    foreach (Control c in this.Controls)
                        if (c.Name == "lblNoImg") c.Visible = false;

                    btnPickImage.Text = "✓ Image Selected";
                    btnPickImage.BackColor = Color.FromArgb(40, 120, 70);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtPrice.Text) || string.IsNullOrEmpty(txtCategory.Text))
            {
                MessageBox.Show("Please fill all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Invalid price format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = new FoodItem
            {
                AdminId = _adminId,
                Name = txtName.Text,
                Description = txtDescription.Text,
                Price = price,
                Category = txtCategory.Text,
                Available = chkAvailable.Checked,
                Image = _imageBytes
            };

            if (foodDAL.AddFoodItem(item))
            {
                MessageBox.Show("Food item added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to add food item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
