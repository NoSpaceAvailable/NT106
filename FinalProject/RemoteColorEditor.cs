using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using static FinalProject.MainForm;

namespace FinalProject
{

    public partial class RemoteColorEditor : Form
    {
        private MainForm mainForm;
        public int room_id;
        private const byte DRAW = 2;
        private byte[] prefix = BitConverter.GetBytes(DRAW);
        public RemoteColorEditor()
        {
            InitializeComponent();
        }

        public RemoteColorEditor(MainForm form)
        {
            InitializeComponent();
            mainForm = form;
        }

        private TcpClient client = null;

        private void Client()
        {
            try
            {
                syncWithRoom();
                while (true)
                {
                    DrawPacket received = ReceiveBuf(client);
                    if (received == null)
                        break;
                    mainForm.DrawFromNetwork(received.Data, received.Color.ToColor());
                }
            }
            catch
            {
                MessageBox.Show("Connection lost or error occurred while receiving data.");
                client = null;
            }
        }

        private void syncWithRoom()
        {
            if (mainForm == null)
                return;
            mainForm.cleanGraphics();
            try
            {
                mainForm.LoadCanvasFromBytes(ReceiveByte(client));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error syncing with room: {ex.Message}");
            }
        }

        public void SendBuf(Draw_data[] datas, int len, Color crt_color, TcpClient Exeption)
        {
            if (client == null)
                return;
            try
            {
                TcpClient tcp = client;
                NetworkStream stream = tcp.GetStream();
                var packet = new DrawPacket(datas.Take(len).ToArray(), crt_color);
                string json = JsonSerializer.Serialize(packet);
                byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
                byte[] lengthPrefix = BitConverter.GetBytes(jsonBytes.Length);
                byte[] roomid = BitConverter.GetBytes(this.room_id); 
                stream.Write(prefix, 0, 1); // Prefix for load balancing
                stream.Write(roomid, 0, roomid.Length);
                stream.Write(lengthPrefix, 0, lengthPrefix.Length);
                stream.Write(jsonBytes, 0, jsonBytes.Length);
            }
            catch
            {
                MessageBox.Show("Error sending data to server. Please check your connection.");
                client = null;
            }
            return;
        }

        public DrawPacket ReceiveBuf(TcpClient tcp)
        {
            try
            {
                NetworkStream stream = tcp.GetStream();

                byte[] lengthPrefix = new byte[4];
                stream.Read(lengthPrefix, 0, 4);
                int length = BitConverter.ToInt32(lengthPrefix, 0);

                byte[] buffer = new byte[length];
                int read = 0;
                while (read < length)
                    read += stream.Read(buffer, read, length - read);

                string json = Encoding.UTF8.GetString(buffer);
                return JsonSerializer.Deserialize<DrawPacket>(json);
            }
            catch
            {
                tcp.Close();
                return null;
            }
        }

        private byte[] ReceiveByte(TcpClient tcp)
        {
            try
            {
                NetworkStream stream = tcp.GetStream();
                byte[] lengthPrefix = new byte[4];
                stream.Read(lengthPrefix, 0, 4);
                int length = BitConverter.ToInt32(lengthPrefix, 0);
                byte[] buffer = new byte[length];
                int read = 0;
                while (read < length)
                    read += stream.Read(buffer, read, length - read);
                return buffer;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving data: {ex.Message}");
                tcp.Close();
                return null;
            }
        }

        private void ConnectToServer(string ipAddress, int port, int room)
        {
            try
            {
                if (client == null)
                {
                    client = new TcpClient(ipAddress, port);
                    MessageBox.Show("Connected to server");
                    NetworkStream stream = client.GetStream();
                    byte[] room_id = BitConverter.GetBytes(room);
                    byte[] prefix = BitConverter.GetBytes(DRAW);
                    stream.Write(prefix, 0, 1); // Prefix for load balancing
                    stream.Write(room_id, 0, room_id.Length);
                    this.room_id = room;
                    Task.Run(() => Client());
                    ConnectBtn.Enabled = false;
                    RoomTextBox.ReadOnly = true;
                    this.Hide();
                    mainForm.room_id = room;
                    mainForm.NewChat();
                }
                else if (client != null)
                {
                    MessageBox.Show("Already connected to server");
                    this.Hide();
                }
            }
            catch
            {
                MessageBox.Show($"Cannot connect to server {ipAddress} {port} {room}");
                client = null;
                ConnectBtn.Enabled = true;
                RoomTextBox.ReadOnly = false;
                this.Hide();
            }

        }

        public bool Live()
        {
            return client != null && client.Connected && !ConnectBtn.Enabled;
        }

        private void ConnectBtn_Click_1(object sender, EventArgs e)
        {
            int port = 0;
            int room = 0;

            try
            {
                IPAddress.Parse(mainForm.IPAddressTextBox.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Invalid ip address");
                return;
            }

            if (int.TryParse(mainForm.PortTextBox.Text.Trim(), out port) == false)
            {
                MessageBox.Show("Invalid port");
                return;
            }

            if (int.TryParse(RoomTextBox.Text.Trim(), out room) == false || room == 0)
            {
                MessageBox.Show("Invalid room id");
                return;
            }

            try
            {
                ConnectToServer(mainForm.IPAddressTextBox.Text.Trim(), port, room);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
                client = null;
                this.Hide();
            }
        }

        private void DisconnectBtn_Click_1(object sender, EventArgs e)
        {
            if (client == null)
            {
                MessageBox.Show("No connection");
                this.Hide();
            }
            else
            {
                try
                {
                    if (client != null)
                           client.Close();
                        client = null;
                    MessageBox.Show("Disconnected");
                    ConnectBtn.Enabled = true;
                    RoomTextBox.ReadOnly = false;
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error disconnecting to server: {ex.Message}");
                    client = null;
                    ConnectBtn.Enabled = true;
                    RoomTextBox.ReadOnly = false;
                    this.Hide();
                }
            }
        }
    }
}
