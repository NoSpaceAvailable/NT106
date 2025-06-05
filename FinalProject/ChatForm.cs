using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FinalProject
{
    public partial class ChatForm : Form
    {
        //private String username = "admin";
        public ChatForm()
        {
            InitializeComponent();
        }

        private String GetCurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        private void SendChatBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ChatInputBox.Text))
            {
                return; // Don't send empty messages
            }

            // 1. Create a label for the message
            Label msg = new Label();
            msg.Text = $"[{GetCurrentTime()}]\r\n(You) {ChatInputBox.Text.Trim()}";
            msg.AutoSize = true; // allows the label to resize to fit text
            msg.Margin = new Padding(10); // Add some space around the message
            msg.Padding = new Padding(5);
            msg.BackColor = Color.FromArgb(225, 245, 254);
            msg.MaximumSize = new Size(DisplayChatArea.ClientSize.Width - 30, 0);
            DisplayChatArea.Controls.Add(msg);
            ChatInputBox.Clear();
            DisplayChatArea.ScrollControlIntoView(msg);
        }

        private void ChooseImageBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    long fileSize = new FileInfo(filePath).Length;

                    if (fileSize > 500 * 1024) // 500KB limit
                    {
                        MessageBox.Show("Image size exceeds 500KB limit.");
                        return;
                    }

                    // Create a small panel to hold the timestamp and the image
                    Panel imagePanel = new Panel();
                    imagePanel.AutoSize = true;
                    imagePanel.Margin = new Padding(10);

                    // Label for timestamp
                    Label timestampLabel = new Label();
                    timestampLabel.Text = $"[{GetCurrentTime()}]\r\n(You):";
                    timestampLabel.AutoSize = true;
                    timestampLabel.Dock = DockStyle.Top;
                    imagePanel.Controls.Add(timestampLabel);

                    PictureBox image = new PictureBox();
                    image.Image = Image.FromFile(filePath);
                    image.SizeMode = PictureBoxSizeMode.StretchImage;

                    int ratio;
                    int newWidth;
                    if (image.Image.Width > ChatInputBox.Width)
                    {
                        newWidth = DisplayChatArea.Width;
                        ratio = image.Image.Width / newWidth;
                    }
                    else
                    {
                        newWidth = image.Image.Width;
                        ratio = 1;
                    }
                    image.Width = newWidth;
                    image.Height = image.Image.Height / ratio;
                    image.Top = timestampLabel.Bottom + 5;
                    imagePanel.Controls.Add(image);

                    DisplayChatArea.Controls.Add(imagePanel);
                    DisplayChatArea.ScrollControlIntoView(imagePanel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error processing image: " + ex.Message);
                }
            }
        }
    }
}
