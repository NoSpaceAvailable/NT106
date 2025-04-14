using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FinalProject.RemoteColorEditor;

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
        private string selectedButton = null;
        private RemoteColorEditor remotecoloreditor;
        public const int DOWN = 1;
        public const int MOVE = 2;
        public const int UP = 3;
        public class Draw_data
        {
            public int Event { get; set; }
            public int penWid { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public PointData prevPoint { get; set; }
            public PointData Location { get; set; }
        }
        public class DrawPacket
        {
            public Draw_data[] Data { get; set; }
            public ColorData Color { get; set; }

            public DrawPacket() { }

            public DrawPacket(Draw_data[] data, Color color)
            {
                Data = data;
                Color = new ColorData(color);
            }
        }

        public class PointData
        {
            public int X { get; set; }
            public int Y { get; set; }

            public PointData() { }

            public PointData(Point p)
            {
                X = p.X;
                Y = p.Y;
            }

            public Point ToPoint() => new Point(X, Y);
        }

        public class ColorData
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public ColorData() { }

            public ColorData(Color color)
            {
                R = color.R;
                G = color.G;
                B = color.B;
                A = color.A;
            }

            public Color ToColor() => Color.FromArgb(A, R, G, B);
        }

        private const long _size = 4096 * 4096;
        private Draw_data[] buffer = new Draw_data[_size];
        private int current_ptr = 0;

        public MainForm()
        {
            InitializeComponent();
            LoadColorPalette();
            SetupBrushSizeMenu();
            UpdateCursor();
            LoadTools();
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

        private void LoadTools()
        {
            int buttonSize = 30;

            // Add an eraser button to the ToolsPanel
            Button eraserButton = new Button();
            Bitmap eraserImage = new Bitmap(@"../../Images/EraserIcon.png");
            eraserButton.Image = new Bitmap(eraserImage, new Size(buttonSize, buttonSize));
            eraserButton.Width = buttonSize;
            eraserButton.Height = buttonSize;
            eraserButton.Left = (1 % 8) * (buttonSize + 5);
            eraserButton.Top = (1 / 8) * (buttonSize + 5);
            eraserButton.FlatStyle = FlatStyle.Flat;
            eraserButton.FlatAppearance.BorderSize = 0;
            eraserButton.Tag = Color.White; // Set the eraser color to white
            eraserButton.Click += toolsSelection_Click;
            eraserButton.MouseEnter += ColorBtn_MouseEnter;
            eraserButton.MouseLeave += ColorBtn_MouseLeave;
            ToolsPanel.Controls.Add(eraserButton);
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

        private void toolsSelection_Click(object sender, EventArgs e)
        {
            selectedColor = Color.White;
            UpdateCursor();
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

        private void FlushBuf()
        {
            if (remotecoloreditor == null || !remotecoloreditor.Visible)
            {
                return;
            }
            remotecoloreditor.SendBuf(buffer, current_ptr, selectedColor);
            for (int i = 0; i < _size; i++)
                buffer[i] = null;
            current_ptr = 0;
        }

        private void SyncWithRemote(MouseEventArgs e, int evnt)
        {
            buffer[current_ptr++] = new Draw_data
            {
                Event = evnt,
                prevPoint = new PointData(previousPoint),
                penWid = penWidth,
                X = e.X,
                Y = e.Y,
                Location = new PointData(e.Location)
            };
            if (current_ptr == _size)
            {
                FlushBuf();
            }
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
            SyncWithRemote(e, DOWN);
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
            SyncWithRemote(e, MOVE);
            previousPoint = e.Location;
        }

        private void DrawingArea_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            FlushBuf();
        }

        private void DrawingArea_Paint(object sender, PaintEventArgs e)
        {
            bufferedGraphics.Render(e.Graphics);
        }

        public void DrawFromNetwork(Draw_data[] datas, Color crt_color)
        {
            Point prevPoint = new Point(0, 0);
            foreach (Draw_data x in datas)
            {
                switch (x.Event)
                {
                    case DOWN:
                        prevPoint = x.Location.ToPoint();

                        // Draw the initial point
                        using (Brush brush = new SolidBrush(crt_color))
                        {
                            bufferedGraphics.Graphics.FillEllipse(brush,
                                x.X - x.penWid / 2, x.Y - x.penWid / 2, x.penWid, x.penWid);
                        }
                        bufferedGraphics.Render(DrawingArea.CreateGraphics());
                        break;
                    case MOVE:
                        using (Pen pen = new Pen(crt_color, x.penWid))
                        {
                            pen.StartCap = LineCap.Round;
                            pen.EndCap = LineCap.Round;
                            bufferedGraphics.Graphics.DrawLine(pen, prevPoint, x.Location.ToPoint());
                        }

                        // Also draw a circle at the current position for better coverage
                        using (Brush brush = new SolidBrush(crt_color))
                        {
                            bufferedGraphics.Graphics.FillEllipse(brush,
                                x.X - x.penWid / 2, x.Y - x.penWid / 2, x.penWid, x.penWid);
                        }
                        bufferedGraphics.Render(DrawingArea.CreateGraphics());
                        prevPoint = x.Location.ToPoint();
                        break;
                    case UP:
                        break;
                    default:
                        break;
                }
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            remotecoloreditor = new RemoteColorEditor(this);
            remotecoloreditor.Show();
        }
    }
}