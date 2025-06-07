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
            this.ChatInputBox = new System.Windows.Forms.TextBox();
            this.SendChatBtn = new System.Windows.Forms.Button();
            this.ChooseImageBtn = new System.Windows.Forms.Button();
            this.DisplayChatArea = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // ChatInputBox
            // 
            this.ChatInputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ChatInputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatInputBox.Location = new System.Drawing.Point(52, 576);
            this.ChatInputBox.MaxLength = 128;
            this.ChatInputBox.Multiline = true;
            this.ChatInputBox.Name = "ChatInputBox";
            this.ChatInputBox.Size = new System.Drawing.Size(412, 59);
            this.ChatInputBox.TabIndex = 0;
            // 
            // SendChatBtn
            // 
            this.SendChatBtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.SendChatBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendChatBtn.Location = new System.Drawing.Point(470, 576);
            this.SendChatBtn.Name = "SendChatBtn";
            this.SendChatBtn.Size = new System.Drawing.Size(60, 59);
            this.SendChatBtn.TabIndex = 1;
            this.SendChatBtn.Text = "Send";
            this.SendChatBtn.UseVisualStyleBackColor = false;
            this.SendChatBtn.Click += new System.EventHandler(this.SendChatBtn_Click);
            // 
            // ChooseImageBtn
            // 
            this.ChooseImageBtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ChooseImageBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChooseImageBtn.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseImageBtn.Location = new System.Drawing.Point(13, 576);
            this.ChooseImageBtn.Name = "ChooseImageBtn";
            this.ChooseImageBtn.Size = new System.Drawing.Size(33, 59);
            this.ChooseImageBtn.TabIndex = 3;
            this.ChooseImageBtn.Text = "📁";
            this.ChooseImageBtn.UseVisualStyleBackColor = false;
            this.ChooseImageBtn.Click += new System.EventHandler(this.ChooseImageBtn_Click);
            // 
            // DisplayChatArea
            // 
            this.DisplayChatArea.AutoScroll = true;
            this.DisplayChatArea.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DisplayChatArea.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.DisplayChatArea.Location = new System.Drawing.Point(13, 13);
            this.DisplayChatArea.Name = "DisplayChatArea";
            this.DisplayChatArea.Size = new System.Drawing.Size(517, 557);
            this.DisplayChatArea.TabIndex = 4;
            this.DisplayChatArea.WrapContents = false;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 648);
            this.Controls.Add(this.DisplayChatArea);
            this.Controls.Add(this.ChooseImageBtn);
            this.Controls.Add(this.SendChatBtn);
            this.Controls.Add(this.ChatInputBox);
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat X area";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ChatInputBox;
        private System.Windows.Forms.Button SendChatBtn;
        private System.Windows.Forms.Button ChooseImageBtn;
        private System.Windows.Forms.FlowLayoutPanel DisplayChatArea;
    }
}