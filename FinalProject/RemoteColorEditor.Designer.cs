namespace FinalProject
{
    partial class RemoteColorEditor
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
            this.label3 = new System.Windows.Forms.Label();
            this.ConnectBtn = new Guna.UI2.WinForms.Guna2Button();
            this.DisconnectBtn = new Guna.UI2.WinForms.Guna2Button();
            this.RoomTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 31);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 28);
            this.label3.TabIndex = 9;
            this.label3.Text = "Room id:";
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Animated = true;
            this.ConnectBtn.AnimatedGIF = true;
            this.ConnectBtn.AutoRoundedCorners = true;
            this.ConnectBtn.BorderColor = System.Drawing.Color.DimGray;
            this.ConnectBtn.BorderThickness = 2;
            this.ConnectBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.ConnectBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.ConnectBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.ConnectBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.ConnectBtn.FillColor = System.Drawing.Color.MediumTurquoise;
            this.ConnectBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ConnectBtn.ForeColor = System.Drawing.Color.White;
            this.ConnectBtn.Location = new System.Drawing.Point(30, 85);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(117, 43);
            this.ConnectBtn.TabIndex = 12;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click_1);
            // 
            // DisconnectBtn
            // 
            this.DisconnectBtn.Animated = true;
            this.DisconnectBtn.AnimatedGIF = true;
            this.DisconnectBtn.AutoRoundedCorners = true;
            this.DisconnectBtn.BorderColor = System.Drawing.Color.DimGray;
            this.DisconnectBtn.BorderThickness = 2;
            this.DisconnectBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.DisconnectBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.DisconnectBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.DisconnectBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.DisconnectBtn.FillColor = System.Drawing.Color.MediumTurquoise;
            this.DisconnectBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.DisconnectBtn.ForeColor = System.Drawing.Color.White;
            this.DisconnectBtn.Location = new System.Drawing.Point(290, 85);
            this.DisconnectBtn.Name = "DisconnectBtn";
            this.DisconnectBtn.Size = new System.Drawing.Size(117, 43);
            this.DisconnectBtn.TabIndex = 13;
            this.DisconnectBtn.Text = "Disconnect";
            this.DisconnectBtn.Click += new System.EventHandler(this.DisconnectBtn_Click_1);
            // 
            // RoomTextBox
            // 
            this.RoomTextBox.AutoRoundedCorners = true;
            this.RoomTextBox.BorderColor = System.Drawing.Color.DimGray;
            this.RoomTextBox.BorderRadius = 16;
            this.RoomTextBox.BorderThickness = 2;
            this.RoomTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.RoomTextBox.DefaultText = "";
            this.RoomTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.RoomTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.RoomTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.RoomTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.RoomTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.RoomTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.RoomTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.RoomTextBox.Location = new System.Drawing.Point(130, 31);
            this.RoomTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RoomTextBox.Name = "RoomTextBox";
            this.RoomTextBox.PlaceholderText = "";
            this.RoomTextBox.SelectedText = "";
            this.RoomTextBox.Size = new System.Drawing.Size(277, 35);
            this.RoomTextBox.TabIndex = 15;
            // 
            // RemoteColorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 157);
            this.Controls.Add(this.RoomTextBox);
            this.Controls.Add(this.DisconnectBtn);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.label3);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RemoteColorEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Color Editor (RCE)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2Button ConnectBtn;
        private Guna.UI2.WinForms.Guna2Button DisconnectBtn;
        public Guna.UI2.WinForms.Guna2TextBox RoomTextBox;
    }
}