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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.OpenChatBtn = new System.Windows.Forms.Button();
            this.IPAddressTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DrawingArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CursorSizeAdjust)).BeginInit();
            this.SuspendLayout();
            // 
            // ColorsPanel
            // 
            this.ColorsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(235)))));
            this.ColorsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColorsPanel.Location = new System.Drawing.Point(30, 29);
            this.ColorsPanel.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this.ColorsPanel.Name = "ColorsPanel";
            this.ColorsPanel.Size = new System.Drawing.Size(892, 154);
            this.ColorsPanel.TabIndex = 0;
            // 
            // DrawingArea
            // 
            this.DrawingArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(250)))));
            this.DrawingArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DrawingArea.Location = new System.Drawing.Point(-6, 240);
            this.DrawingArea.Margin = new System.Windows.Forms.Padding(4);
            this.DrawingArea.MinimumSize = new System.Drawing.Size(2340, 1444);
            this.DrawingArea.Name = "DrawingArea";
            this.DrawingArea.Size = new System.Drawing.Size(2340, 1444);
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
            this.CursorSizeAdjust.Location = new System.Drawing.Point(34, 192);
            this.CursorSizeAdjust.Margin = new System.Windows.Forms.Padding(4);
            this.CursorSizeAdjust.Maximum = 30;
            this.CursorSizeAdjust.Minimum = 1;
            this.CursorSizeAdjust.Name = "CursorSizeAdjust";
            this.CursorSizeAdjust.Size = new System.Drawing.Size(374, 40);
            this.CursorSizeAdjust.TabIndex = 2;
            this.CursorSizeAdjust.TickStyle = System.Windows.Forms.TickStyle.None;
            this.CursorSizeAdjust.Value = 5;
            // 
            // ToolsPanel
            // 
            this.ToolsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(240)))));
            this.ToolsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ToolsPanel.Location = new System.Drawing.Point(972, 29);
            this.ToolsPanel.Margin = new System.Windows.Forms.Padding(6);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Size = new System.Drawing.Size(534, 154);
            this.ToolsPanel.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(2130, 23);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 58);
            this.button1.TabIndex = 4;
            this.button1.Text = "RCE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(1968, 23);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 58);
            this.button2.TabIndex = 5;
            this.button2.Text = "Login";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OpenChatBtn
            // 
            this.OpenChatBtn.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.OpenChatBtn.FlatAppearance.BorderSize = 0;
            this.OpenChatBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenChatBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.OpenChatBtn.ForeColor = System.Drawing.Color.White;
            this.OpenChatBtn.Location = new System.Drawing.Point(1806, 23);
            this.OpenChatBtn.Margin = new System.Windows.Forms.Padding(6);
            this.OpenChatBtn.Name = "OpenChatBtn";
            this.OpenChatBtn.Size = new System.Drawing.Size(150, 58);
            this.OpenChatBtn.TabIndex = 6;
            this.OpenChatBtn.Text = "Chat";
            this.OpenChatBtn.UseVisualStyleBackColor = true;
            this.OpenChatBtn.Click += new System.EventHandler(this.OpenChatBtn_Click);
            // 
            // IPAddressTextBox
            // 
            this.IPAddressTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IPAddressTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPAddressTextBox.Location = new System.Drawing.Point(1968, 92);
            this.IPAddressTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.IPAddressTextBox.Name = "IPAddressTextBox";
            this.IPAddressTextBox.Size = new System.Drawing.Size(310, 37);
            this.IPAddressTextBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1790, 98);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 30);
            this.label1.TabIndex = 9;
            this.label1.Text = "Remote IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1790, 152);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 30);
            this.label2.TabIndex = 10;
            this.label2.Text = "Remote port";
            // 
            // PortTextBox
            // 
            this.PortTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PortTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PortTextBox.Location = new System.Drawing.Point(1968, 146);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(310, 37);
            this.PortTextBox.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(2310, 1369);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.IPAddressTextBox);
            this.Controls.Add(this.OpenChatBtn);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ToolsPanel);
            this.Controls.Add(this.CursorSizeAdjust);
            this.Controls.Add(this.DrawingArea);
            this.Controls.Add(this.ColorsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(20, 19, 20, 19);
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
        private System.Windows.Forms.Button button1;
        private Button button2;
        private Button OpenChatBtn;
        private TextBox IPAddressTextBox;
        private Label label1;
        private Label label2;
        private TextBox PortTextBox;
    }
}
