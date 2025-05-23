﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using static FinalProject.MainForm;
using System.Net.Http;

namespace FinalProject
{

    public partial class RemoteColorEditor: Form
    {
        private MainForm mainForm;
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
        private TcpListener listener = null;
        private List<TcpClient> clients = new List<TcpClient>();
        private void ConnectToServer(string ipAddress, int port)
        {
            try
            {
                if (client == null && listener == null)
                {
                    client = new TcpClient(ipAddress, port);
                    MessageBox.Show("Connected to server");
                    Task.Run(() => Client());
                }
                else if (client == null)
                {
                    MessageBox.Show("Already connected to server");
                }
                else
                {
                    MessageBox.Show("Already host");
                }
            }
            catch
            {
                MessageBox.Show("Cannot connect to server");
            }

        }

        private void Client()
        {
            try
            {
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
                client = null;
            }
        }

        private void HostServer(string ipAddress, int port)
        {
            try
            {
                if (client == null && listener == null)
                {
                    listener = new TcpListener(IPAddress.Parse(ipAddress), port);
                    listener.Start();
                    MessageBox.Show("Server hosted at " + ipAddress + ":" + port);
                    Task.Run(() => Server());
                }
                else if (client == null)
                {
                    MessageBox.Show("Already connected to server");
                }
                else
                {
                    MessageBox.Show("Already host");
                }
            }
            catch
            {
                MessageBox.Show("Cannot host server");
            }

        }

        private void Server()
        {
            try
            {
                while (true)
                {
                    clients.Add(listener.AcceptTcpClient());
                    Task.Run(() => ClientHandler(clients[clients.Count() - 1]));
                }
            }
            catch
            {
                MessageBox.Show("Server stopped");
            }
        }

        private void ClientHandler(TcpClient _client)
        {
            try
            {
                while (true) 
                {
                    DrawPacket received = ReceiveBuf(_client);
                    if (received == null)
                        break;
                    mainForm.DrawFromNetwork(received.Data, received.Color.ToColor());
                }    
            }
            catch
            {
                _client.Close();
                clients.Remove(_client);
            }
        }

        public void SendBuf(Draw_data[] datas, int len, Color crt_color)
        {
            if (client == null && listener == null)
                return;
            if (listener != null)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    try
                    {
                        TcpClient tcp = clients[i];
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
                        clients.Remove(clients[i]);
                    }
                }
                return;
            }
            else if (client != null)
            {
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
                    client = null;
                }
                return;
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

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            string ipAddr = IPTextBox.Text.Trim();
            int port = 0;

            try
            {
                IPAddress.Parse(IPTextBox.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Invalid ip address");
                return;
            }

            if (int.TryParse(PortTextBox.Text.Trim(), out port) == false)
            {
                MessageBox.Show("Invalid port");
                return;
            }

            try
            {
                ConnectToServer(ipAddr, port);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
            }
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
                    if (clients.Count != 0)
                        for (int i = 0; i < clients.Count; i++)
                                clients[i].Close();
                    clients.Clear();
                    listener = null;
                    client = null;
                }
                catch
                {
                }
                MessageBox.Show("Disconnected");
            }
        }

        private void HostBtn_Click(object sender, EventArgs e)
        {
            string ipAddr = IPTextBox.Text.Trim();
            int port = 0;

            try
            {
                IPAddress.Parse(IPTextBox.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Invalid ip address");
                return;
            }

            if (int.TryParse(PortTextBox.Text.Trim(), out port) == false)
            {
                MessageBox.Show("Invalid port");
                return;
            }

            try
            {
                HostServer(ipAddr, port);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error hosting server: {ex.Message}");
            }
        }
    }
}
