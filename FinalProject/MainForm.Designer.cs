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
            this.ColorsPanel.BackColor = Color.FromArgb(255, 245, 235); // warm peach
            this.ColorsPanel.BorderStyle = BorderStyle.FixedSingle;
            this.ColorsPanel.Location = new Point(15, 15);
            this.ColorsPanel.Margin = new Padding(10);
            this.ColorsPanel.Name = "ColorsPanel";
            this.ColorsPanel.Size = new Size(447, 81);
            this.ColorsPanel.TabIndex = 0;
            // 
            // DrawingArea
            // 
            this.DrawingArea.BackColor = Color.FromArgb(255, 255, 250); // off white
            this.DrawingArea.BorderStyle = BorderStyle.FixedSingle;
            this.DrawingArea.Location = new Point(-3, 125);
            this.DrawingArea.Margin = new Padding(2);
            this.DrawingArea.Name = "DrawingArea";
            this.DrawingArea.Size = new Size(1159, 587);
            this.DrawingArea.SizeMode = PictureBoxSizeMode.CenterImage;
            this.DrawingArea.TabIndex = 1;
            this.DrawingArea.TabStop = false;
            this.DrawingArea.Paint += new PaintEventHandler(this.DrawingArea_Paint);
            this.DrawingArea.MouseDown += new MouseEventHandler(this.DrawingArea_MouseDown);
            this.DrawingArea.MouseMove += new MouseEventHandler(this.DrawingArea_MouseMove);
            this.DrawingArea.MouseUp += new MouseEventHandler(this.DrawingArea_MouseUp);
            // 
            // CursorSizeAdjust
            // 
            this.CursorSizeAdjust.AutoSize = false;
            this.CursorSizeAdjust.BackColor = Color.FromArgb(230, 245, 255); // pastel blue
            this.CursorSizeAdjust.Location = new Point(17, 100);
            this.CursorSizeAdjust.Margin = new Padding(2);
            this.CursorSizeAdjust.Maximum = 30;
            this.CursorSizeAdjust.Minimum = 1;
            this.CursorSizeAdjust.Name = "CursorSizeAdjust";
            this.CursorSizeAdjust.Size = new Size(187, 21);
            this.CursorSizeAdjust.TabIndex = 2;
            this.CursorSizeAdjust.TickStyle = TickStyle.None;
            this.CursorSizeAdjust.Value = 5;
            // 
            // ToolsPanel
            // 
            this.ToolsPanel.BackColor = Color.FromArgb(240, 255, 240); // mint green
            this.ToolsPanel.BorderStyle = BorderStyle.FixedSingle;
            this.ToolsPanel.Location = new Point(486, 15);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Size = new Size(200, 81);
            this.ToolsPanel.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.BackColor = Color.MediumSlateBlue;
            this.button1.ForeColor = Color.White;
            this.button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.button1.Location = new Point(1065, 12);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 30);
            this.button1.TabIndex = 4;
            this.button1.Text = "RCE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(250, 250, 255); // very light violet
            this.ClientSize = new Size(1155, 712);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ToolsPanel);
            this.Controls.Add(this.CursorSizeAdjust);
            this.Controls.Add(this.DrawingArea);
            this.Controls.Add(this.ColorsPanel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.Padding = new Padding(10);
            this.StartPosition = FormStartPosition.CenterScreen;
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
