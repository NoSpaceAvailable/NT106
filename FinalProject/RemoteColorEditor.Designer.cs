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
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.ConnectBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DisconnectBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.RoomTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // IPTextBox
            // 
            this.IPTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IPTextBox.Location = new System.Drawing.Point(116, 22);
            this.IPTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.IPTextBox.MaxLength = 15;
            this.IPTextBox.Multiline = true;
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(277, 28);
            this.IPTextBox.TabIndex = 2;
            this.IPTextBox.Text = "127.0.0.1";
            // 
            // PortTextBox
            // 
            this.PortTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PortTextBox.Location = new System.Drawing.Point(116, 60);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PortTextBox.MaxLength = 5;
            this.PortTextBox.Multiline = true;
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(277, 28);
            this.PortTextBox.TabIndex = 3;
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConnectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectBtn.Location = new System.Drawing.Point(20, 143);
            this.ConnectBtn.Margin = new System.Windows.Forms.Padding(4);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(117, 43);
            this.ConnectBtn.TabIndex = 4;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "IP address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(60, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port:";
            // 
            // DisconnectBtn
            // 
            this.DisconnectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DisconnectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisconnectBtn.Location = new System.Drawing.Point(270, 143);
            this.DisconnectBtn.Margin = new System.Windows.Forms.Padding(4);
            this.DisconnectBtn.Name = "DisconnectBtn";
            this.DisconnectBtn.Size = new System.Drawing.Size(123, 43);
            this.DisconnectBtn.TabIndex = 7;
            this.DisconnectBtn.Text = "Disconnect";
            this.DisconnectBtn.UseVisualStyleBackColor = true;
            this.DisconnectBtn.Click += new System.EventHandler(this.DisconnectBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(32, 96);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Room id:";
            // 
            // RoomTextBox
            // 
            this.RoomTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RoomTextBox.Location = new System.Drawing.Point(116, 96);
            this.RoomTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.RoomTextBox.MaxLength = 5;
            this.RoomTextBox.Multiline = true;
            this.RoomTextBox.Name = "RoomTextBox";
            this.RoomTextBox.Size = new System.Drawing.Size(277, 28);
            this.RoomTextBox.TabIndex = 8;
            // 
            // RemoteColorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 214);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RoomTextBox);
            this.Controls.Add(this.DisconnectBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.IPTextBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RemoteColorEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Color Editor (RCE)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Button ConnectBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DisconnectBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RoomTextBox;
    }
}