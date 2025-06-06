using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.IO.Compression;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace FinalProject
{
    public partial class ChatForm : Form
    {
        private String AuthToken = String.Empty;
        private String RemoteHost = "127.0.0.1";
        private readonly int RemotePort = 10001;
        private readonly int BUFF_SIZE = 1024; // bytes

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

        public ChatForm(String username, String token, String remoteHost)
        {
            InitializeComponent();
            this.username = username;
            this.AuthToken = token;
            this.RemoteHost = remoteHost;

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
                this.networkStream.Write(initialMessage, 0, initialMessage.Length);


                byte[] buffer = new byte[BUFF_SIZE * 4]; // Increased buffer for images
                int bytesRead;

                while ((bytesRead = this.networkStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    String response = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim('\0');

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
                            String messageType    = parts[2];
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
                                MessageBox.Show("Unimplemented!");
                            } else
                            {
                                MessageBox.Show("Received invalid message!");
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection lost: {ex.Message}");
            }
        }

        // stolen method
        public byte[] HexStringToByteArray(String hex)
        {
            if (hex.Length % 2 != 0)
            {
                Console.WriteLine("Error: Hexadecimal string must have an even number of characters.");
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
                Console.WriteLine("Error: Invalid character found in hexadecimal string.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting hex to byte array: {ex.Message}");
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
            byte[] data = HexStringToByteArray(hexOfCompressedImage);
            if (data == null || data.Length == 0)
            {
                return null;
            }
            try
            {
                using (MemoryStream memStream = new MemoryStream())
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
            if (String.IsNullOrWhiteSpace(ChatInputBox.Text))
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

                    String compressedImageData = Compress(filePath);
                    SendMessageToServer(compressedImageData, (int)MessageType.Image);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error processing image: " + ex.Message);
                }
            }
        }
    }
}
