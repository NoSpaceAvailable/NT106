using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{

    public partial class RemoteColorEditor: Form
    {
        private Socket clientSocket;
        public RemoteColorEditor()
        {
            InitializeComponent();
        }

        private void ConnectToServer(string ipAddress, int port)
        {

        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(PortTextBox.Text);
            string ipAddr = IPTextBox.Text;

            if (string.IsNullOrEmpty(ipAddr) || port <= 0)
            {
                MessageBox.Show("Please enter a valid IP address and port number.");
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
    }
}
