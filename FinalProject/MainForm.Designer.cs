using System.Windows.Forms;

namespace FinalProject
{
    partial class MainForm
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
            this.ColorsPanel.Location = new System.Drawing.Point(15, 15);
            this.ColorsPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ColorsPanel.Name = "ColorsPanel";
            this.ColorsPanel.Size = new System.Drawing.Size(447, 81);
            this.ColorsPanel.TabIndex = 0;
            // 
            // DrawingArea
            // 
            this.DrawingArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DrawingArea.Location = new System.Drawing.Point(-3, 125);
            this.DrawingArea.Margin = new System.Windows.Forms.Padding(2);
            this.DrawingArea.Name = "DrawingArea";
            this.DrawingArea.Size = new System.Drawing.Size(1159, 587);
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
            this.CursorSizeAdjust.CausesValidation = false;
            this.CursorSizeAdjust.Location = new System.Drawing.Point(17, 100);
            this.CursorSizeAdjust.Margin = new System.Windows.Forms.Padding(2);
            this.CursorSizeAdjust.Name = "CursorSizeAdjust";
            this.CursorSizeAdjust.Size = new System.Drawing.Size(187, 21);
            this.CursorSizeAdjust.TabIndex = 2;
            this.CursorSizeAdjust.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // ToolsPanel
            // 
            this.ToolsPanel.Location = new System.Drawing.Point(486, 15);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Size = new System.Drawing.Size(200, 81);
            this.ToolsPanel.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(1065, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "RCE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1155, 712);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ToolsPanel);
            this.Controls.Add(this.CursorSizeAdjust);
            this.Controls.Add(this.DrawingArea);
            this.Controls.Add(this.ColorsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Advanced Paint";
            ((System.ComponentModel.ISupportInitialize)(this.DrawingArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CursorSizeAdjust)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ColorsPanel;
        private System.Windows.Forms.PictureBox DrawingArea;
        private TrackBar CursorSizeAdjust;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Panel ToolsPanel;
        private Button button1;
    }
}

