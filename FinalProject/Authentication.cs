using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Authentication : Form
    {
        private String IPAddr = "127.0.0.1";
        private readonly int Port = 9999;    // this will be fixed
        private char delimiter = '|';

        public bool AuthResult = false; 

        enum Action
        {
            Login,
            Register
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

        public Authentication()
        {
            InitializeComponent();
        }

        public Authentication(String ipAddr)
        {
            InitializeComponent();
            this.IPAddr = ipAddr;
        }

        private bool sanitize(String username, String password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                ErrorTextLabel.Text = "Empty username and password";
                ErrorTextLabel.Visible = true;
                return false;
            }
            if (username.Length < 5 || username.Length > 20)
            {
                ErrorTextLabel.Text = "Length of username must be in range (5, 20)";
                ErrorTextLabel.Visible = true;
                return false;
            }
            if (password.Length < 8 || password.Length > 30)
            {
                ErrorTextLabel.Text = "Length of password must be in range (8, 30)";
                ErrorTextLabel.Visible = true;
                return false;
            }
            return true;
        }

        private int SendDataToAuthServer(String username, String password, int action)
        {
            try
            {
                TcpClient client = new TcpClient(IPAddr, Port);

                NetworkStream stream = client.GetStream();
                String dataToSend = $"{action}{delimiter}{username}{delimiter}{password}";
                byte[] sendBuffer = Encoding.UTF8.GetBytes(dataToSend);
                stream.Write(sendBuffer, 0, sendBuffer.Length);

                byte[] reveiceBuffer = new byte[256];
                int bytesRead = stream.Read(reveiceBuffer, 0, reveiceBuffer.Length);
                String data = Encoding.UTF8.GetString(reveiceBuffer, 0, bytesRead);
                stream.Close();

                String[] splitted = data.Split(delimiter);
                int status = int.Parse(splitted[0]);
                if (status == (int)AuthStatus.AuthenticationSuccessful)
                {
                    this.AuthResult = true;
                }
                return status;
            }
            catch (SocketException)
            {
                return (int)AuthStatus.NetWorkError;
            }
            catch (Exception)
            {
                return (int)AuthStatus.UnexpectedError;
            }
        }

        private void RegisterLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ConfirmPasswordLabel.Visible = !ConfirmPasswordLabel.Visible;
            ConfirmPasswordTextBox.Visible = !ConfirmPasswordTextBox.Visible;
            LoginOrRegisterBtn.Text = ConfirmPasswordLabel.Visible ? "Register" : "Login";
            RegisterOrLoginLinkLabel.Text = ConfirmPasswordLabel.Visible ? "Already have an account? Login here" : "Don't have an account? Register now";
            TitleLabel.Text = ConfirmPasswordLabel.Visible ? "Chat X register" : "Chat X login";
        }

        private void LoginOrRegisterBtn_Click(object sender, EventArgs e)
        {
            String action = LoginOrRegisterBtn.Text;
            String username = UsernameBox.Text;
            String password = PasswordBox.Text;

            if (!sanitize(username, password))
            {
                return;
            }
            ErrorTextLabel.Visible = false;

            if (action.Equals("Register") || action.Equals("Login"))
            {
                String confirmPassword = ConfirmPasswordTextBox.Visible ? ConfirmPasswordTextBox.Text : null;
                if (ConfirmPasswordTextBox.Visible && password != confirmPassword)
                {
                    ErrorTextLabel.Text = "Passwords do not match!";
                    ErrorTextLabel.Visible = true;
                    return;
                }
                int loginResult = SendDataToAuthServer(
                    username,
                    password,
                    (int)(
                        action.Equals("Login") ? Action.Login : Action.Register
                    )
                );
                switch (loginResult)
                {
                    case (int)AuthStatus.NetWorkError:
                        ErrorTextLabel.Text = "Network error! Please try again later.";
                        ErrorTextLabel.ForeColor = System.Drawing.Color.Red;
                        break;
                    case (int)AuthStatus.UnexpectedError:
                        ErrorTextLabel.Text = "Unexpected error! Please try again later.";
                        ErrorTextLabel.ForeColor = System.Drawing.Color.Red;
                        break;
                    case (int)AuthStatus.Success:
                        ErrorTextLabel.Text = "Register successfully! Please login";
                        ErrorTextLabel.ForeColor = System.Drawing.Color.Green;
                        break;
                    case (int)AuthStatus.AuthenticationSuccessful:
                        ErrorTextLabel.Text = "Login successfully! Closing form ...";
                        ErrorTextLabel.ForeColor = System.Drawing.Color.Green;
                        this.Close();
                        break;
                    case (int)AuthStatus.UserAlreadyExists:
                        ErrorTextLabel.Text = "User already exists!";
                        ErrorTextLabel.ForeColor = System.Drawing.Color.Red;
                        break;
                    case (int)AuthStatus.InvalidCredentials:
                        ErrorTextLabel.Text = "Invalid username or password!";
                        ErrorTextLabel.ForeColor = System.Drawing.Color.Red;
                        break;
                    case (int)AuthStatus.BadRequest:
                        ErrorTextLabel.Text = "Bad request! Please re-check your input.";
                        ErrorTextLabel.ForeColor = System.Drawing.Color.Red;
                        break;
                    default:
                        ErrorTextLabel.Text = "Unknown error! Please try again later.";
                        ErrorTextLabel.ForeColor = System.Drawing.Color.Red;
                        break;
                }
            }
            else
            {
                ErrorTextLabel.Text = "Invalid action!";
                ErrorTextLabel.ForeColor = System.Drawing.Color.Red;
            }
            ErrorTextLabel.Visible = true;
        }

        private void InputBox_TextChanged(object sender, EventArgs e)
        {
            ErrorTextLabel.Visible = false;
            ErrorTextLabel.Text = String.Empty;
        }
    }
}
