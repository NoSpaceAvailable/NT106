using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.IO.Compression;
using System.Threading.Tasks;

namespace FinalProject
{
    public partial class ChatForm : Form
    {
        private String AuthToken = String.Empty;
        private String RemoteHost;
        private MainForm mainForm = null;
        private int RemotePort;
        private readonly int BUFF_SIZE = 1024; // bytes
        private const byte CHAT = 3;
        private byte[] prefix = BitConverter.GetBytes(CHAT);

        private String username = String.Empty;

        private NetworkStream networkStream = null;
        public ChatForm()
        {
            InitializeComponent();
        }

        enum Action
        {
            Login,
            Register,
            Verify,
            SendMessage,
            Broadcast,
        }
        enum AuthStatus
        {
            Success,
            AuthenticationSuccessful,
            UserAlreadyExists,
            InvalidCredentials,
            NetWorkError,
            BadRequest,
            UnexpectedError
        }
        enum MessageType
        {
            Text,
            Image
        }

        /*
            Struct of a message:
                action | username | auth_token | message_type | message |

            Where action is Action.SendMessage
            If the message is an image, it must be a hex string of the compressed image
         */

        public ChatForm(MainForm form, String username, String token)
        {
            InitializeComponent();
            mainForm = form;
            this.username = username;
            this.AuthToken = token;
            this.RemoteHost = mainForm.IPAddressTextBox.Text.Trim();
            if (!int.TryParse(mainForm.PortTextBox.Text.Trim(), out RemotePort) || RemotePort <= 0)
            {
                MessageBox.Show("Invalid port number. Please enter a valid port.");
                this.Close();
            }

            // warm up connection
            this.Load += new EventHandler(ChatForm_Load);
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            // Start the listener thread once the form has loaded
            Task.Run(() => StartListening());
        }

        private void StartListening()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(this.RemoteHost, this.RemotePort);
                this.networkStream = client.GetStream();

                // Send a single warm-up message to authenticate the session
                byte[] initialMessage = Encoding.UTF8.GetBytes($"{(int)Action.SendMessage}|{this.username}|{this.AuthToken}|{(int)MessageType.Text}||");
                byte[] room = BitConverter.GetBytes(mainForm.room_id);
                this.networkStream.Write(prefix, 0, 1);
                this.networkStream.Write(room, 0, room.Length);
                this.networkStream.Write(initialMessage, 0, initialMessage.Length);

                // image too large, longer buffer
                // image too large, longer buffer
                byte[] buffer = new byte[BUFF_SIZE * 1000];
                byte[] tmp = new byte[8];
                int bytesRead;
                while (true)
                {
                    if (this.networkStream.Read(tmp, 0, 4) != 4)
                        continue;
                    int data_len = BitConverter.ToInt32(tmp, 0);
                    if (data_len <= 0 || data_len > buffer.Length)
                    {
                        MessageBox.Show($"Received invalid data length from server {data_len}");
                        continue;
                    }
                    bytesRead = 0;
                    int ptr = 0;
                    while (bytesRead < data_len)
                    {
                        bytesRead += this.networkStream.Read(buffer, ptr, data_len - bytesRead);
                        ptr = bytesRead;
                    }
                    String response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // Ensure that controls are created
                    if (!this.IsHandleCreated)
                    {
                        this.CreateControl();
                    }
                    // Then call invoke to update UI
                    this.Invoke((MethodInvoker) delegate {
                        /* Structure of a broadcast message sent from server:
                         * 
                         * action | username | type | message |
                         * 
                         * Where action is Action.Broadcast
                        */
                        String[] parts = response.Split(new char[] { '|' }, 5);
                        if (parts.Length == 5 && parts[0] == $"{(int)Action.Broadcast}")
                        {
                            String senderUsername = parts[1];
                            String messageType = parts[2];
                            String messageContent = parts[3];
                            // ignore parts[4]

                            if (messageType.Equals($"{(int)MessageType.Text}"))
                            {
                                Label msgLabel = new Label
                                {
                                    Text = $"[{GetCurrentTime()}]\r\n({senderUsername}) {messageContent}",
                                    AutoSize = true,
                                    Margin = new Padding(10),
                                    Padding = new Padding(5),
                                    BackColor = Color.LightGray,
                                    MaximumSize = new Size(DisplayChatArea.ClientSize.Width - 30, 0)
                                };
                                DisplayChatArea.Controls.Add(msgLabel);
                                DisplayChatArea.ScrollControlIntoView(msgLabel);
                            }
                            else if (messageType.Equals($"{(int)MessageType.Image}"))
                            {
                                Bitmap image = Decompress(messageContent);
                                if (image != null)
                                {
                                    // Create a panel to hold the timestamp and the image
                                    Panel imagePanel = new Panel();
                                    imagePanel.AutoSize = true;
                                    imagePanel.Margin = new Padding(10);
                                    imagePanel.BackColor = Color.LightGray;
                                    imagePanel.Padding = new Padding(5);

                                    // Label for timestamp and sender
                                    Label timestampLabel = new Label();
                                    timestampLabel.Text = $"[{GetCurrentTime()}]\r\n({senderUsername}):";
                                    timestampLabel.AutoSize = true;
                                    timestampLabel.Dock = DockStyle.Top;
                                    imagePanel.Controls.Add(timestampLabel);

                                    // PictureBox for the image
                                    PictureBox pictureBox = new PictureBox();
                                    pictureBox.Image = image;
                                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                                    // Calculate appropriate size
                                    int maxWidth = DisplayChatArea.ClientSize.Width - 50;
                                    int newWidth, newHeight;

                                    if (image.Width > maxWidth)
                                    {
                                        double ratio = (double)maxWidth / image.Width;
                                        newWidth = maxWidth;
                                        newHeight = (int)(image.Height * ratio);
                                    }
                                    else
                                    {
                                        newWidth = image.Width;
                                        newHeight = image.Height;
                                    }

                                    pictureBox.Size = new Size(newWidth, newHeight);
                                    pictureBox.Top = timestampLabel.Bottom + 5;
                                    imagePanel.Controls.Add(pictureBox);

                                    DisplayChatArea.Controls.Add(imagePanel);
                                    DisplayChatArea.ScrollControlIntoView(imagePanel);
                                }
                                else
                                {
                                    Label errorLabel = new Label
                                    {
                                        Text = $"[{GetCurrentTime()}]\r\n({senderUsername}) [Failed to load image]",
                                        AutoSize = true,
                                        Margin = new Padding(10),
                                        Padding = new Padding(5),
                                        BackColor = Color.LightCoral,
                                        MaximumSize = new Size(DisplayChatArea.ClientSize.Width - 30, 0)
                                    };
                                    DisplayChatArea.Controls.Add(errorLabel);
                                    DisplayChatArea.ScrollControlIntoView(errorLabel);
                                }
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // stolen method
        public byte[] HexStringToByteArray(String hex)
        {
            if (hex.Length % 2 != 0)
            {
                MessageBox.Show("Error: Hexadecimal string must have an even number of characters.");
                return null;
            }
            try
            {
                byte[] bytes = new byte[hex.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    String byteChars = hex.Substring(i * 2, 2);
                    bytes[i] = Convert.ToByte(byteChars, 16);
                }
                return bytes;
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: Invalid character found in hexadecimal string.");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting hex to byte array: {ex.Message}");
                return null;
            }
        }

        // This method compress an image and return the compressed image as hex string
        private String Compress(String filePath)
        {
            try
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                byte[] compressed;
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(outputStream, CompressionLevel.Optimal))
                    {
                        gzipStream.Write(fileData, 0, fileData.Length);
                    }
                    compressed = outputStream.ToArray();
                    String compressedHex = BitConverter.ToString(compressed).Replace("-", "");
                    return compressedHex;
                }
            } 
            catch (Exception)
            {
                // caller will alert user for this exception
                return null;
            }
        }

        // This one reverse the above method to display the image to the chat box
        private Bitmap Decompress(String hexOfCompressedImage)
        {
            byte[] compressedData = HexStringToByteArray(hexOfCompressedImage);
            if (compressedData == null || compressedData.Length == 0)
            {
                return null;
            }
            try
            {
                using (MemoryStream memStream = new MemoryStream(compressedData))
                {
                    using (GZipStream decompressStream = new GZipStream(memStream, CompressionMode.Decompress))
                    {
                        return new Bitmap(decompressStream);
                    }
                }
            }
            catch (Exception e)
            {
                // remove this line on product
                MessageBox.Show(e.Message);
                return null;
            }
        }

        private void SendMessageToServer(String message, int type = (int)MessageType.Text)
        {
            if (this.networkStream == null || !this.networkStream.CanWrite)
            {
                MessageBox.Show("Not connected to the server.");
                return;
            }

            try
            {
                String msg = $"{(int)Action.SendMessage}|{this.username}|{this.AuthToken}|{type}|{message}|";
                byte[] messageBytes = Encoding.UTF8.GetBytes(msg);
                byte[] room = BitConverter.GetBytes(mainForm.room_id);
                this.networkStream.Write(prefix, 0, 1); // Prefix for load balancing
                this.networkStream.Write(room, 0, room.Length);
                this.networkStream.Write(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }

        private String GetCurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        private void SendChatBtn_Click(object sender, EventArgs e)
        {
           
        }

        private void ChooseImageBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ChatInputBox.Text.Trim()))
            {
                return; // Don't send empty messages
            }

            // 1. Create a label for the message
            Label msg = new Label();
            String actualMessage = ChatInputBox.Text.Trim();
            msg.Text = $"[{GetCurrentTime()}]\r\n(You) {actualMessage}";
            msg.AutoSize = true;            // allows the label to resize to fit text
            msg.Margin = new Padding(10);   // Add some space around the message
            msg.Padding = new Padding(5);
            msg.BackColor = Color.FromArgb(225, 245, 254);
            msg.MaximumSize = new Size(DisplayChatArea.ClientSize.Width - 30, 0);
            DisplayChatArea.Controls.Add(msg);
            ChatInputBox.Clear();
            DisplayChatArea.ScrollControlIntoView(msg);
            SendMessageToServer(actualMessage);
        }

        private void ChooseImageBtn_Click_1(object sender, EventArgs e)
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
                    String filePath = openFileDialog.FileName;
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
                    imagePanel.BackColor = Color.FromArgb(225, 245, 254);
                    imagePanel.Padding = new Padding(5);

                    // Label for timestamp
                    Label timestampLabel = new Label();
                    timestampLabel.Text = $"[{GetCurrentTime()}]\r\n(You):";
                    timestampLabel.AutoSize = true;
                    timestampLabel.Dock = DockStyle.Top;
                    imagePanel.Controls.Add(timestampLabel);

                    PictureBox image = new PictureBox();
                    image.Image = Image.FromFile(filePath);
                    image.SizeMode = PictureBoxSizeMode.StretchImage;

                    int maxWidth = DisplayChatArea.ClientSize.Width - 50;
                    int newWidth, newHeight;

                    if (image.Image.Width > maxWidth)
                    {
                        double ratio = (double)maxWidth / image.Image.Width;
                        newWidth = maxWidth;
                        newHeight = (int)(image.Image.Height * ratio);
                    }
                    else
                    {
                        newWidth = image.Image.Width;
                        newHeight = image.Image.Height;
                    }

                    image.Size = new Size(newWidth, newHeight);
                    image.Top = timestampLabel.Bottom + 5;
                    imagePanel.Controls.Add(image);

                    DisplayChatArea.Controls.Add(imagePanel);
                    DisplayChatArea.ScrollControlIntoView(imagePanel);

                    String compressedImageData = Compress(filePath);
                    if (compressedImageData != null)
                    {
                        SendMessageToServer(compressedImageData, (int)MessageType.Image);
                    }
                    else
                    {
                        MessageBox.Show("Failed to compress image.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error processing image: " + ex.Message);
                }
            }
        }
    }
}
