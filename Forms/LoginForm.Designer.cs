namespace FoodOrderingSystem.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.rbCustomer = new System.Windows.Forms.RadioButton();
            this.rbAdmin = new System.Windows.Forms.RadioButton();
            this.btnLogin = new System.Windows.Forms.Button();
            this.linkRegister = new System.Windows.Forms.LinkLabel();
            this.pnlCard = new System.Windows.Forms.Panel();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();

            // === FORM ===
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 560);
            this.BackColor = System.Drawing.Color.FromArgb(15, 15, 25);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.Text = "Food Ordering System";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.Add(this.pnlCard);

            // === CARD PANEL ===
            this.pnlCard.BackColor = System.Drawing.Color.FromArgb(28, 28, 45);
            this.pnlCard.Location = new System.Drawing.Point(40, 60);
            this.pnlCard.Size = new System.Drawing.Size(400, 440);
            this.pnlCard.Controls.Add(this.lblTitle);
            this.pnlCard.Controls.Add(this.lblSubtitle);
            this.pnlCard.Controls.Add(this.lblEmail);
            this.pnlCard.Controls.Add(this.txtEmail);
            this.pnlCard.Controls.Add(this.lblPassword);
            this.pnlCard.Controls.Add(this.txtPassword);
            this.pnlCard.Controls.Add(this.rbCustomer);
            this.pnlCard.Controls.Add(this.rbAdmin);
            this.pnlCard.Controls.Add(this.btnLogin);
            this.pnlCard.Controls.Add(this.linkRegister);

            // === TITLE ===
            this.lblTitle.AutoSize = false;
            this.lblTitle.Size = new System.Drawing.Size(400, 45);
            this.lblTitle.Location = new System.Drawing.Point(0, 30);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(255, 107, 53);
            this.lblTitle.Text = "🍔 Food Order System";
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;

            // === SUBTITLE ===
            this.lblSubtitle.AutoSize = false;
            this.lblSubtitle.Size = new System.Drawing.Size(400, 25);
            this.lblSubtitle.Location = new System.Drawing.Point(0, 75);
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(150, 150, 170);
            this.lblSubtitle.Text = "Sign in to your account";
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;

            // === EMAIL LABEL ===
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(40, 120);
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(200, 200, 220);
            this.lblEmail.Text = "EMAIL ADDRESS";
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;

            // === EMAIL TEXTBOX ===
            this.txtEmail.Location = new System.Drawing.Point(40, 145);
            this.txtEmail.Size = new System.Drawing.Size(320, 32);
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(40, 40, 65);
            this.txtEmail.ForeColor = System.Drawing.Color.White;
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Name = "txtEmail";

            // === PASSWORD LABEL ===
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(40, 195);
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(200, 200, 220);
            this.lblPassword.Text = "PASSWORD";
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;

            // === PASSWORD TEXTBOX ===
            this.txtPassword.Location = new System.Drawing.Point(40, 220);
            this.txtPassword.Size = new System.Drawing.Size(320, 32);
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(40, 40, 65);
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Name = "txtPassword";

            // === RADIO CUSTOMER ===
            this.rbCustomer.AutoSize = true;
            this.rbCustomer.Checked = true;
            this.rbCustomer.Location = new System.Drawing.Point(40, 275);
            this.rbCustomer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbCustomer.ForeColor = System.Drawing.Color.FromArgb(200, 200, 220);
            this.rbCustomer.BackColor = System.Drawing.Color.Transparent;
            this.rbCustomer.Text = "Customer";
            this.rbCustomer.TabStop = true;
            this.rbCustomer.Name = "rbCustomer";
            this.rbCustomer.UseVisualStyleBackColor = false;

            // === RADIO ADMIN ===
            this.rbAdmin.AutoSize = true;
            this.rbAdmin.Location = new System.Drawing.Point(200, 275);
            this.rbAdmin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbAdmin.ForeColor = System.Drawing.Color.FromArgb(200, 200, 220);
            this.rbAdmin.BackColor = System.Drawing.Color.Transparent;
            this.rbAdmin.Text = "Admin";
            this.rbAdmin.Name = "rbAdmin";
            this.rbAdmin.UseVisualStyleBackColor = false;

            // === LOGIN BUTTON ===
            this.btnLogin.Location = new System.Drawing.Point(40, 315);
            this.btnLogin.Size = new System.Drawing.Size(320, 45);
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(255, 107, 53);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Text = "LOGIN";
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // === REGISTER LINK ===
            this.linkRegister.AutoSize = false;
            this.linkRegister.Size = new System.Drawing.Size(320, 30);
            this.linkRegister.Location = new System.Drawing.Point(40, 375);
            this.linkRegister.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkRegister.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.linkRegister.LinkColor = System.Drawing.Color.FromArgb(255, 107, 53);
            this.linkRegister.ActiveLinkColor = System.Drawing.Color.White;
            this.linkRegister.BackColor = System.Drawing.Color.Transparent;
            this.linkRegister.Text = "Not a user? Register here";
            this.linkRegister.Name = "linkRegister";
            this.linkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRegister_LinkClicked);

            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.RadioButton rbCustomer;
        private System.Windows.Forms.RadioButton rbAdmin;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel linkRegister;
        private System.Windows.Forms.Panel pnlCard;
    }
}
