namespace FinalProject
{
    partial class Authentication
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RegisterOrLoginLinkLabel = new System.Windows.Forms.LinkLabel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ConfirmPasswordLabel = new System.Windows.Forms.Label();
            this.ErrorTextLabel = new System.Windows.Forms.Label();
            this.UsernameBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.PasswordBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.ConfirmPasswordTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.LoginOrRegisterBtn = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // RegisterOrLoginLinkLabel
            // 
            this.RegisterOrLoginLinkLabel.Location = new System.Drawing.Point(81, 444);
            this.RegisterOrLoginLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RegisterOrLoginLinkLabel.Name = "RegisterOrLoginLinkLabel";
            this.RegisterOrLoginLinkLabel.Size = new System.Drawing.Size(255, 21);
            this.RegisterOrLoginLinkLabel.TabIndex = 0;
            this.RegisterOrLoginLinkLabel.TabStop = true;
            this.RegisterOrLoginLinkLabel.Text = "Don\'t have an account? Register now";
            this.RegisterOrLoginLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RegisterLinkLabel_LinkClicked);
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(131, 106);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(144, 29);
            this.TitleLabel.TabIndex = 3;
            this.TitleLabel.Text = "Chat X login";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(85, 244);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Password";
            // 
            // ConfirmPasswordLabel
            // 
            this.ConfirmPasswordLabel.AutoSize = true;
            this.ConfirmPasswordLabel.Location = new System.Drawing.Point(85, 317);
            this.ConfirmPasswordLabel.Name = "ConfirmPasswordLabel";
            this.ConfirmPasswordLabel.Size = new System.Drawing.Size(114, 16);
            this.ConfirmPasswordLabel.TabIndex = 7;
            this.ConfirmPasswordLabel.Text = "Confirm password";
            this.ConfirmPasswordLabel.Visible = false;
            // 
            // ErrorTextLabel
            // 
            this.ErrorTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorTextLabel.AutoSize = true;
            this.ErrorTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorTextLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrorTextLabel.Location = new System.Drawing.Point(50, 480);
            this.ErrorTextLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ErrorTextLabel.Name = "ErrorTextLabel";
            this.ErrorTextLabel.Size = new System.Drawing.Size(298, 18);
            this.ErrorTextLabel.TabIndex = 8;
            this.ErrorTextLabel.Text = "Length of username must be in range (5, 20)";
            this.ErrorTextLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ErrorTextLabel.Visible = false;
            // 
            // UsernameBox
            // 
            this.UsernameBox.BorderColor = System.Drawing.Color.Gray;
            this.UsernameBox.BorderRadius = 10;
            this.UsernameBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.UsernameBox.DefaultText = "";
            this.UsernameBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.UsernameBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.UsernameBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.UsernameBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.UsernameBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.UsernameBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.UsernameBox.ForeColor = System.Drawing.Color.Black;
            this.UsernameBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.UsernameBox.Location = new System.Drawing.Point(88, 191);
            this.UsernameBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.PlaceholderText = "";
            this.UsernameBox.SelectedText = "";
            this.UsernameBox.Size = new System.Drawing.Size(232, 34);
            this.UsernameBox.TabIndex = 9;
            // 
            // PasswordBox
            // 
            this.PasswordBox.BorderColor = System.Drawing.Color.Gray;
            this.PasswordBox.BorderRadius = 10;
            this.PasswordBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.PasswordBox.DefaultText = "";
            this.PasswordBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.PasswordBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.PasswordBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.PasswordBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.PasswordBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.PasswordBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.PasswordBox.ForeColor = System.Drawing.Color.Black;
            this.PasswordBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.PasswordBox.Location = new System.Drawing.Point(88, 264);
            this.PasswordBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.PasswordChar = '*';
            this.PasswordBox.PlaceholderText = "";
            this.PasswordBox.SelectedText = "";
            this.PasswordBox.Size = new System.Drawing.Size(232, 34);
            this.PasswordBox.TabIndex = 10;
            // 
            // ConfirmPasswordTextBox
            // 
            this.ConfirmPasswordTextBox.BorderColor = System.Drawing.Color.Gray;
            this.ConfirmPasswordTextBox.BorderRadius = 10;
            this.ConfirmPasswordTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ConfirmPasswordTextBox.DefaultText = "";
            this.ConfirmPasswordTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.ConfirmPasswordTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ConfirmPasswordTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.ConfirmPasswordTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.ConfirmPasswordTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ConfirmPasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ConfirmPasswordTextBox.ForeColor = System.Drawing.Color.Black;
            this.ConfirmPasswordTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ConfirmPasswordTextBox.Location = new System.Drawing.Point(88, 337);
            this.ConfirmPasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ConfirmPasswordTextBox.Name = "ConfirmPasswordTextBox";
            this.ConfirmPasswordTextBox.PasswordChar = '*';
            this.ConfirmPasswordTextBox.PlaceholderText = "";
            this.ConfirmPasswordTextBox.SelectedText = "";
            this.ConfirmPasswordTextBox.Size = new System.Drawing.Size(232, 34);
            this.ConfirmPasswordTextBox.TabIndex = 11;
            this.ConfirmPasswordTextBox.Visible = false;
            // 
            // LoginOrRegisterBtn
            // 
            this.LoginOrRegisterBtn.BorderRadius = 12;
            this.LoginOrRegisterBtn.BorderThickness = 1;
            this.LoginOrRegisterBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.LoginOrRegisterBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.LoginOrRegisterBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.LoginOrRegisterBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.LoginOrRegisterBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LoginOrRegisterBtn.ForeColor = System.Drawing.Color.White;
            this.LoginOrRegisterBtn.Location = new System.Drawing.Point(136, 395);
            this.LoginOrRegisterBtn.Name = "LoginOrRegisterBtn";
            this.LoginOrRegisterBtn.Size = new System.Drawing.Size(139, 32);
            this.LoginOrRegisterBtn.TabIndex = 12;
            this.LoginOrRegisterBtn.Text = "Login";
            this.LoginOrRegisterBtn.Click += new System.EventHandler(this.LoginOrRegisterBtn_Click_1);
            // 
            // Authentication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(419, 554);
            this.Controls.Add(this.LoginOrRegisterBtn);
            this.Controls.Add(this.ConfirmPasswordTextBox);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.UsernameBox);
            this.Controls.Add(this.ErrorTextLabel);
            this.Controls.Add(this.ConfirmPasswordLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.RegisterOrLoginLinkLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Authentication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authentication";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel RegisterOrLoginLinkLabel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ConfirmPasswordLabel;
        private System.Windows.Forms.Label ErrorTextLabel;
        private Guna.UI2.WinForms.Guna2TextBox UsernameBox;
        private Guna.UI2.WinForms.Guna2TextBox PasswordBox;
        private Guna.UI2.WinForms.Guna2TextBox ConfirmPasswordTextBox;
        private Guna.UI2.WinForms.Guna2Button LoginOrRegisterBtn;
    }
}