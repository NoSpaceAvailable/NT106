namespace FinalProject
{
    partial class ChatForm
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
            this.DisplayChatArea = new System.Windows.Forms.FlowLayoutPanel();
            this.ChatInputBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.SendBtn = new Guna.UI2.WinForms.Guna2Button();
            this.ChooseImageBtn = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // DisplayChatArea
            // 
            this.DisplayChatArea.AutoScroll = true;
            this.DisplayChatArea.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DisplayChatArea.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.DisplayChatArea.Location = new System.Drawing.Point(17, 16);
            this.DisplayChatArea.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DisplayChatArea.Name = "DisplayChatArea";
            this.DisplayChatArea.Size = new System.Drawing.Size(689, 686);
            this.DisplayChatArea.TabIndex = 4;
            this.DisplayChatArea.WrapContents = false;
            // 
            // ChatInputBox
            // 
            this.ChatInputBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ChatInputBox.BorderRadius = 10;
            this.ChatInputBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ChatInputBox.DefaultText = "";
            this.ChatInputBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.ChatInputBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ChatInputBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.ChatInputBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.ChatInputBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ChatInputBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ChatInputBox.ForeColor = System.Drawing.Color.Black;
            this.ChatInputBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ChatInputBox.Location = new System.Drawing.Point(70, 709);
            this.ChatInputBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChatInputBox.Name = "ChatInputBox";
            this.ChatInputBox.PlaceholderForeColor = System.Drawing.Color.Transparent;
            this.ChatInputBox.PlaceholderText = "";
            this.ChatInputBox.SelectedText = "";
            this.ChatInputBox.Size = new System.Drawing.Size(550, 73);
            this.ChatInputBox.TabIndex = 5;
            // 
            // SendBtn
            // 
            this.SendBtn.BorderRadius = 12;
            this.SendBtn.BorderThickness = 1;
            this.SendBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.SendBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.SendBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.SendBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.SendBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SendBtn.ForeColor = System.Drawing.Color.White;
            this.SendBtn.Location = new System.Drawing.Point(626, 709);
            this.SendBtn.Name = "SendBtn";
            this.SendBtn.Size = new System.Drawing.Size(80, 73);
            this.SendBtn.TabIndex = 0;
            this.SendBtn.Text = "Send";
            this.SendBtn.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // ChooseImageBtn
            // 
            this.ChooseImageBtn.BorderRadius = 12;
            this.ChooseImageBtn.BorderThickness = 1;
            this.ChooseImageBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.ChooseImageBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.ChooseImageBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.ChooseImageBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.ChooseImageBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ChooseImageBtn.ForeColor = System.Drawing.Color.White;
            this.ChooseImageBtn.Location = new System.Drawing.Point(12, 709);
            this.ChooseImageBtn.Name = "ChooseImageBtn";
            this.ChooseImageBtn.Size = new System.Drawing.Size(52, 73);
            this.ChooseImageBtn.TabIndex = 6;
            this.ChooseImageBtn.Text = "📁";
            this.ChooseImageBtn.Click += new System.EventHandler(this.ChooseImageBtn_Click_1);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 798);
            this.Controls.Add(this.ChooseImageBtn);
            this.Controls.Add(this.SendBtn);
            this.Controls.Add(this.ChatInputBox);
            this.Controls.Add(this.DisplayChatArea);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat X area";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel DisplayChatArea;
        private Guna.UI2.WinForms.Guna2TextBox ChatInputBox;
        private Guna.UI2.WinForms.Guna2Button SendBtn;
        private Guna.UI2.WinForms.Guna2Button ChooseImageBtn;
    }
}