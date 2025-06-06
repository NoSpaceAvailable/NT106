using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Sockets;
using System.Text;

namespace FinalProject
{
    public partial class ChatForm : Form
    {
        private String AuthToken = String.Empty;
        private String RemoteHost = "127.0.0.1";
        private readonly int RemotePort = 10001;
        private readonly int BUFF_SIZE = 256; // bytes

        private char delimiter = '|';
        private String username = String.Empty;
        public ChatForm()
        {
            InitializeComponent();
            CheckAuth();
        }

        public ChatForm(String username, String token, String remoteHost)
        {
            InitializeComponent();
            this.username = username;
            this.AuthToken = token;
            this.RemoteHost = remoteHost;
            CheckAuth();
        }

        private void CheckAuth()
        {
            if (string.IsNullOrEmpty(AuthToken))
            {
                MessageBox.Show("You are not authenticated. Please log in first.");
                this.Close(); // Close the chat form if not authenticated
                return;
            } else
            {
                TcpClient authClient = new TcpClient();
                authClient.Connect(this.RemoteHost, this.RemotePort);

                NetworkStream stream = authClient.GetStream();
                byte[] jwtBytesArray = Encoding.UTF8.GetBytes($"{this.username}{this.delimiter}{this.AuthToken}");
                MessageBox.Show($"{this.username}{this.delimiter}{this.AuthToken}");
                stream.Write(jwtBytesArray, 0, jwtBytesArray.Length);

                byte[] response = new byte[BUFF_SIZE];
                stream.Read(response, 0, response.Length);
                String data = Encoding.UTF8.GetString(response);
                String[] splitted = data.Split(delimiter);
                if (splitted[0].Trim('\0') == "0")
                {
                    // this mean the token is valid
                } 
                else
                {
                    MessageBox.Show("Nahhhhhhh");
                    this.Close();
                }
            }
        }

        private void SendMessageToServer(String message)
        {

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
