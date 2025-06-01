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
            ((System.ComponentModel.ISupportInitialize)(this.DrawingArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CursorSizeAdjust)).BeginInit();
            this.SuspendLayout();
            // 
            // ColorsPanel
            // 
            this.ColorsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(235)))));
            this.ColorsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColorsPanel.Location = new System.Drawing.Point(20, 18);
            this.ColorsPanel.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.ColorsPanel.Name = "ColorsPanel";
            this.ColorsPanel.Size = new System.Drawing.Size(595, 99);
            this.ColorsPanel.TabIndex = 0;
            // 
            // DrawingArea
            // 
            this.DrawingArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(250)))));
            this.DrawingArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DrawingArea.Location = new System.Drawing.Point(-4, 154);
            this.DrawingArea.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.CursorSizeAdjust.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.ToolsPanel.Location = new System.Drawing.Point(648, 18);
            this.ToolsPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Size = new System.Drawing.Size(356, 99);
            this.ToolsPanel.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1420, 15);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 37);
            this.button1.TabIndex = 4;
            this.button1.Text = "RCE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1540, 876);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ToolsPanel);
            this.Controls.Add(this.CursorSizeAdjust);
            this.Controls.Add(this.DrawingArea);
            this.Controls.Add(this.ColorsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Advanced Paint";
            ((System.ComponentModel.ISupportInitialize)(this.DrawingArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CursorSizeAdjust)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ColorsPanel;
        private System.Windows.Forms.PictureBox DrawingArea;
        private System.Windows.Forms.TrackBar CursorSizeAdjust;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel ToolsPanel;
        private System.Windows.Forms.Button button1;
    }
}
