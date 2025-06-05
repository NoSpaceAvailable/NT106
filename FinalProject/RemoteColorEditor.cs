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
        private String remote_addr;
        private String remote_port;

        public int room_id;

        public RemoteColorEditor()
        {
            InitializeComponent();
        }

        public RemoteColorEditor(MainForm form, String ip, String port)
        {
            this.remote_addr = ip;
            this.remote_port = port;
            InitializeComponent();
            mainForm = form;
        }

        private TcpClient client = null;
        private TcpListener listener = null;

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
                if (client != null)
                    client.Close();
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
                stream.Write(lengthPrefix, 0, lengthPrefix.Length);
                stream.Write(jsonBytes, 0, jsonBytes.Length);
            }
            catch
            {
                client.Close();
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
                MessageBox.Show("Error receiving data from server. Please check your connection.");
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

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            int port = 0;
            int room = 0;

            try
            {
                IPAddress.Parse(this.remote_addr);
            }
            catch
            {
                MessageBox.Show("Invalid ip address");
                return;
            }

            if (int.TryParse(this.remote_port, out port) == false)
            {
                MessageBox.Show("Invalid port");
                return;
            }

            if (int.TryParse(RoomTextBox.Text.Trim(), out room) == false || room == 0)
            {
                MessageBox.Show("Invalid room id");
            }

            try
            {
                ConnectToServer(this.remote_addr, port, room);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
            }
            this.room_id = room;
        }

        private void DisconnectBtn_Click(object sender, EventArgs e)
        {
            if (listener == null && client == null)
            {
                MessageBox.Show("No connection");
            }
            else
            {
                try
                {
                    if (listener != null)
                        listener.Stop();
                    if (client != null)
                        client.Close();
                    listener = null;
                    client = null;
                }
                catch
                {
                    MessageBox.Show("Disconnected");
                }
            }
        }

        private void ConnectToServer(string ipAddress, int port, int room)
        {
            try
            {
                if (client == null && listener == null)
                {
                    client = new TcpClient(ipAddress, port);
                    MessageBox.Show("Connected to server");
                    NetworkStream stream = client.GetStream();
                    byte[] lengthPrefix = BitConverter.GetBytes(room);
                    stream.Write(lengthPrefix, 0, lengthPrefix.Length);
                    Task.Run(() => Client());
                    this.Hide();
                }
                else if (client != null)
                {
                    MessageBox.Show("Already connected to server");
                    this.Hide();
                }
            }
            catch
            {
                MessageBox.Show("Cannot connect to server");
            }

        }
    }
}
