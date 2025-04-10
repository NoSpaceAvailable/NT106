using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class MainForm : Form
    {
        private Color selectedColor = Color.Black;
        private Button customColorButton;
        private int penWidth = 10;
        private BufferedGraphicsContext context;
        private BufferedGraphics bufferedGraphics;
        private Point previousPoint;
        private bool isDrawing = false;

        public MainForm()
        {
            InitializeComponent();
            LoadColorPalette();
            SetupBrushSizeMenu();
            UpdateCursor();
            InitializeBufferedGraphics();
        }

        private void InitializeBufferedGraphics()
        {
            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(DrawingArea.Width + 1, DrawingArea.Height + 1);
            bufferedGraphics = context.Allocate(DrawingArea.CreateGraphics(),
                new Rectangle(0, 0, DrawingArea.Width, DrawingArea.Height));
            bufferedGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            bufferedGraphics.Graphics.Clear(Color.White);
        }

        private void SetupBrushSizeMenu()
        {
            CursorSizeAdjust.Minimum = 1;
            CursorSizeAdjust.Maximum = 30;
            CursorSizeAdjust.Value = penWidth;
            CursorSizeAdjust.Width = 150;

            CursorSizeAdjust.Scroll += (s, e) =>
            {
                penWidth = CursorSizeAdjust.Value;
                UpdateCursor();
            };

            Controls.Add(CursorSizeAdjust);
        }

        private void UpdateCursor()
        {
            int size = penWidth;
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (Brush brush = new SolidBrush(selectedColor))
                {
                    g.FillEllipse(brush, 0, 0, size - 1, size - 1);
                }

                using (Pen pen = new Pen(Color.Black, 1))
                {
                    g.DrawEllipse(pen, 0, 0, size - 1, size - 1);
                }
            }

            Cursor customCursor = new Cursor(bmp.GetHicon());
            DrawingArea.Cursor = customCursor;
        }

        private void LoadColorPalette()
        {
            Color[] colors = new Color[]
            {
                Color.Black, Color.White, Color.Gray, Color.Red, Color.Maroon,
                Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple,
                Color.Brown, Color.Pink, Color.LightGreen, Color.LightBlue, Color.Violet
            };

            int buttonSize = 30;
            for (int i = 0; i < colors.Length; i++)
            {
                Button colorBtn = new Button();
                colorBtn.BackColor = colors[i];
                colorBtn.Width = buttonSize;
                colorBtn.Height = buttonSize;
                colorBtn.Left = (i % 8) * (buttonSize + 5);
                colorBtn.Top = (i / 8) * (buttonSize + 5);
                colorBtn.FlatStyle = FlatStyle.Flat;
                colorBtn.FlatAppearance.BorderSize = 1;
                colorBtn.Tag = colors[i];
                colorBtn.Click += ColorBtn_Click;

                colorBtn.MouseEnter += ColorBtn_MouseEnter;
                colorBtn.MouseLeave += ColorBtn_MouseLeave;

                ColorsPanel.Controls.Add(colorBtn);

                if (i == colors.Length - 1)
                {
                    customColorButton = colorBtn;
                    customColorButton.Click += customColorButton_Click;
                }
            }
        }

        private void ColorBtn_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void ColorBtn_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void customColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                customColorButton.BackColor = colorDialog.Color;
                customColorButton.Tag = colorDialog.Color;
                selectedColor = colorDialog.Color;
                UpdateCursor();
            }
        }

        private void ColorBtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            selectedColor = (Color)clickedButton.Tag;
            UpdateCursor();
        }

        private void DrawingArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            isDrawing = true;
            previousPoint = e.Location;

            // Draw the initial point
            using (Brush brush = new SolidBrush(selectedColor))
            {
                bufferedGraphics.Graphics.FillEllipse(brush,
                    e.X - penWidth / 2, e.Y - penWidth / 2, penWidth, penWidth);
            }
            bufferedGraphics.Render(DrawingArea.CreateGraphics());
        }

        private void DrawingArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || e.Button != MouseButtons.Left) return;

            // Draw a line between the previous point and current point to fill gaps
            using (Pen pen = new Pen(selectedColor, penWidth))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                bufferedGraphics.Graphics.DrawLine(pen, previousPoint, e.Location);
            }

            // Also draw a circle at the current position for better coverage
            using (Brush brush = new SolidBrush(selectedColor))
            {
                bufferedGraphics.Graphics.FillEllipse(brush,
                    e.X - penWidth / 2, e.Y - penWidth / 2, penWidth, penWidth);
            }

            bufferedGraphics.Render(DrawingArea.CreateGraphics());
            previousPoint = e.Location;
        }

        private void DrawingArea_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void DrawingArea_Paint(object sender, PaintEventArgs e)
        {
            bufferedGraphics.Render(e.Graphics);
        }

        private void DrawingArea_Resize(object sender, EventArgs e)
        {
            // Reinitialize the buffer when the drawing area is resized
            if (bufferedGraphics != null)
            {
                bufferedGraphics.Dispose();
                InitializeBufferedGraphics();
                bufferedGraphics.Render(DrawingArea.CreateGraphics());
            }
        }
    }
}