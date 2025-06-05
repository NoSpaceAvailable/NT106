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
            this.ConnectBtn = new System.Windows.Forms.Button();
            this.DisconnectBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.RoomTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConnectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectBtn.Location = new System.Drawing.Point(10, 64);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(88, 35);
            this.ConnectBtn.TabIndex = 4;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // DisconnectBtn
            // 
            this.DisconnectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DisconnectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisconnectBtn.Location = new System.Drawing.Point(197, 64);
            this.DisconnectBtn.Name = "DisconnectBtn";
            this.DisconnectBtn.Size = new System.Drawing.Size(92, 35);
            this.DisconnectBtn.TabIndex = 7;
            this.DisconnectBtn.Text = "Disconnect";
            this.DisconnectBtn.UseVisualStyleBackColor = true;
            this.DisconnectBtn.Click += new System.EventHandler(this.DisconnectBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Room id:";
            // 
            // RoomTextBox
            // 
            this.RoomTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RoomTextBox.Location = new System.Drawing.Point(82, 26);
            this.RoomTextBox.MaxLength = 5;
            this.RoomTextBox.Multiline = true;
            this.RoomTextBox.Name = "RoomTextBox";
            this.RoomTextBox.Size = new System.Drawing.Size(208, 23);
            this.RoomTextBox.TabIndex = 8;
            // 
            // RemoteColorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 116);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RoomTextBox);
            this.Controls.Add(this.DisconnectBtn);
            this.Controls.Add(this.ConnectBtn);
            this.Name = "RemoteColorEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Color Editor (RCE)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ConnectBtn;
        private System.Windows.Forms.Button DisconnectBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RoomTextBox;
    }
}