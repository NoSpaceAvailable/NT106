using System.Windows.Forms;
using System.Drawing;

namespace FinalProject
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.ColorsPanel = new System.Windows.Forms.Panel();
            this.DrawingArea = new System.Windows.Forms.PictureBox();
            this.CursorSizeAdjust = new System.Windows.Forms.TrackBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ToolsPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.OpenChatBtn = new Guna.UI2.WinForms.Guna2Button();
            this.LoginBtn = new Guna.UI2.WinForms.Guna2Button();
            this.RCEBtn = new Guna.UI2.WinForms.Guna2Button();
            this.IPAddressTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.PortTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DrawingArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CursorSizeAdjust)).BeginInit();
            this.SuspendLayout();
            // 
            // ColorsPanel
            // 
            this.ColorsPanel.BackColor = System.Drawing.Color.PeachPuff;
            this.ColorsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColorsPanel.Location = new System.Drawing.Point(20, 19);
            this.ColorsPanel.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.ColorsPanel.Name = "ColorsPanel";
            this.ColorsPanel.Size = new System.Drawing.Size(387, 89);
            this.ColorsPanel.TabIndex = 0;
            // 
            // DrawingArea
            // 
            this.DrawingArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(250)))));
            this.DrawingArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DrawingArea.Location = new System.Drawing.Point(23, 155);
            this.DrawingArea.MinimumSize = new System.Drawing.Size(1545, 722);
            this.DrawingArea.Name = "DrawingArea";
            this.DrawingArea.Size = new System.Drawing.Size(1545, 722);
            this.DrawingArea.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.DrawingArea.TabIndex = 1;
            this.DrawingArea.TabStop = false;
            this.DrawingArea.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawingArea_Paint);
            this.DrawingArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawingArea_MouseDown);
            this.DrawingArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawingArea_MouseMove);
            this.DrawingArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawingArea_MouseUp);
            // 
            // CursorSizeAdjust
            // 
            this.CursorSizeAdjust.AutoSize = false;
            this.CursorSizeAdjust.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.CursorSizeAdjust.Location = new System.Drawing.Point(23, 123);
            this.CursorSizeAdjust.Maximum = 30;
            this.CursorSizeAdjust.Minimum = 1;
            this.CursorSizeAdjust.Name = "CursorSizeAdjust";
            this.CursorSizeAdjust.Size = new System.Drawing.Size(249, 26);
            this.CursorSizeAdjust.TabIndex = 2;
            this.CursorSizeAdjust.TickStyle = System.Windows.Forms.TickStyle.None;
            this.CursorSizeAdjust.Value = 5;
            // 
            // ToolsPanel
            // 
            this.ToolsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(240)))));
            this.ToolsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ToolsPanel.Location = new System.Drawing.Point(467, 19);
            this.ToolsPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Size = new System.Drawing.Size(309, 89);
            this.ToolsPanel.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1210, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 28);
            this.label1.TabIndex = 9;
            this.label1.Text = "Remote IP";
            // 
            // OpenChatBtn
            // 
            this.OpenChatBtn.Animated = true;
            this.OpenChatBtn.AnimatedGIF = true;
            this.OpenChatBtn.AutoRoundedCorners = true;
            this.OpenChatBtn.BorderColor = System.Drawing.Color.DimGray;
            this.OpenChatBtn.BorderThickness = 2;
            this.OpenChatBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.OpenChatBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.OpenChatBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.OpenChatBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.OpenChatBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.OpenChatBtn.ForeColor = System.Drawing.Color.White;
            this.OpenChatBtn.Location = new System.Drawing.Point(1201, 19);
            this.OpenChatBtn.Name = "OpenChatBtn";
            this.OpenChatBtn.Size = new System.Drawing.Size(109, 37);
            this.OpenChatBtn.TabIndex = 11;
            this.OpenChatBtn.Text = "Chat";
            this.OpenChatBtn.Click += new System.EventHandler(this.OpenChatBtn_Click);
            // 
            // LoginBtn
            // 
            this.LoginBtn.Animated = true;
            this.LoginBtn.AnimatedGIF = true;
            this.LoginBtn.AutoRoundedCorners = true;
            this.LoginBtn.BorderColor = System.Drawing.Color.DimGray;
            this.LoginBtn.BorderThickness = 2;
            this.LoginBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.LoginBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.LoginBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.LoginBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.LoginBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LoginBtn.ForeColor = System.Drawing.Color.White;
            this.LoginBtn.Location = new System.Drawing.Point(1335, 19);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Size = new System.Drawing.Size(109, 37);
            this.LoginBtn.TabIndex = 12;
            this.LoginBtn.Text = "Login";
            this.LoginBtn.Click += new System.EventHandler(this.LoginBtn_Click);
            // 
            // RCEBtn
            // 
            this.RCEBtn.Animated = true;
            this.RCEBtn.AnimatedGIF = true;
            this.RCEBtn.AutoRoundedCorners = true;
            this.RCEBtn.BorderColor = System.Drawing.Color.DimGray;
            this.RCEBtn.BorderThickness = 2;
            this.RCEBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.RCEBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.RCEBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.RCEBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.RCEBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.RCEBtn.ForeColor = System.Drawing.Color.White;
            this.RCEBtn.Location = new System.Drawing.Point(1459, 19);
            this.RCEBtn.Name = "RCEBtn";
            this.RCEBtn.Size = new System.Drawing.Size(109, 37);
            this.RCEBtn.TabIndex = 13;
            this.RCEBtn.Text = "RCE";
            this.RCEBtn.Click += new System.EventHandler(this.RCEBtn_Click);
            // 
            // IPAddressTextBox
            // 
            this.IPAddressTextBox.AutoRoundedCorners = true;
            this.IPAddressTextBox.BorderColor = System.Drawing.Color.DimGray;
            this.IPAddressTextBox.BorderRadius = 16;
            this.IPAddressTextBox.BorderThickness = 2;
            this.IPAddressTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.IPAddressTextBox.DefaultText = "";
            this.IPAddressTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.IPAddressTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.IPAddressTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.IPAddressTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.IPAddressTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.IPAddressTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.IPAddressTextBox.ForeColor = System.Drawing.Color.Black;
            this.IPAddressTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.IPAddressTextBox.Location = new System.Drawing.Point(1324, 63);
            this.IPAddressTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IPAddressTextBox.Name = "IPAddressTextBox";
            this.IPAddressTextBox.PlaceholderText = "";
            this.IPAddressTextBox.SelectedText = "";
            this.IPAddressTextBox.Size = new System.Drawing.Size(244, 35);
            this.IPAddressTextBox.TabIndex = 14;
            // 
            // PortTextBox
            // 
            this.PortTextBox.AutoRoundedCorners = true;
            this.PortTextBox.BorderColor = System.Drawing.Color.DimGray;
            this.PortTextBox.BorderRadius = 16;
            this.PortTextBox.BorderThickness = 2;
            this.PortTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.PortTextBox.DefaultText = "";
            this.PortTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.PortTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.PortTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.PortTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.PortTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.PortTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.PortTextBox.ForeColor = System.Drawing.Color.Black;
            this.PortTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.PortTextBox.Location = new System.Drawing.Point(1324, 106);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.PlaceholderText = "";
            this.PortTextBox.SelectedText = "";
            this.PortTextBox.Size = new System.Drawing.Size(244, 34);
            this.PortTextBox.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1188, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 28);
            this.label2.TabIndex = 10;
            this.label2.Text = "Remote port";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(1605, 893);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.IPAddressTextBox);
            this.Controls.Add(this.RCEBtn);
            this.Controls.Add(this.LoginBtn);
            this.Controls.Add(this.OpenChatBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ToolsPanel);
            this.Controls.Add(this.CursorSizeAdjust);
            this.Controls.Add(this.DrawingArea);
            this.Controls.Add(this.ColorsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Advanced Paint";
            ((System.ComponentModel.ISupportInitialize)(this.DrawingArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CursorSizeAdjust)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ColorsPanel;
        private System.Windows.Forms.PictureBox DrawingArea;
        private System.Windows.Forms.TrackBar CursorSizeAdjust;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel ToolsPanel;
        private Label label1;
        private Guna.UI2.WinForms.Guna2Button OpenChatBtn;
        private Guna.UI2.WinForms.Guna2Button LoginBtn;
        private Guna.UI2.WinForms.Guna2Button RCEBtn;
        public Guna.UI2.WinForms.Guna2TextBox IPAddressTextBox;
        public Guna.UI2.WinForms.Guna2TextBox PortTextBox;
        private Label label2;
    }
}
