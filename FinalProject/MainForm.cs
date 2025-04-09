using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FinalProject
{
    public partial class MainForm: Form
    {
        private Color selectedColor = Color.Black;
        private Button customcoButton; // Button for custom color selection like you can type hex value on that and change color to that value
        private int penWidth = 10;
        public MainForm()
        {
            InitializeComponent();
            LoadColorPalette();
            SetupBrushSizeMenu();
            UpdateCursor();
        }

        private void SetupBrushSizeMenu()
        {
            trackBar.Minimum = 1;
            trackBar.Maximum = 30;
            trackBar.Value = penWidth;
            trackBar.Width = 150;

            trackBar.Scroll += (s, e) =>
            {
                penWidth = trackBar.Value;
                UpdateCursor();
            };

            Controls.Add(trackBar);


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

            // Set custom cursor
            Cursor customCursor = new Cursor(bmp.GetHicon());
            pictureBox1.Cursor = customCursor;
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

                panelColors.Controls.Add(colorBtn);

                if (i == colors.Length - 1)
                {
                    customcoButton = colorBtn;
                    customcoButton.Click += customcoButton_Click;
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

        private void customcoButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                customcoButton.BackColor = colorDialog.Color;
                customcoButton.Tag = colorDialog.Color;
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
        
    }
}
