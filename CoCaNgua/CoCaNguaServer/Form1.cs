using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoCaNguaServer
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        bool isRunning = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                listener = new TcpListener(IPAddress.Any, 8888);
                listener.Start();
                isRunning = true;
                lb_status.Text = "🟢 Server đang chạy...";
                Thread thread = new Thread(ListenForClients);
                thread.Start();
            }
        }
        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    string clientInfo = client.Client.RemoteEndPoint.ToString();
                    Invoke(new Action(() => lstClients.Items.Add(clientInfo)));
                    Thread thread = new Thread(() => HandleClient(client, clientInfo));
                    thread.Start();
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }
        private void HandleClient(TcpClient client, string clientInfo)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[2048];
                int byteCount = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                string response = "";

                // Log ra listbox nếu muốn
                // Log($"Từ {clientInfo}: {request}");

                if (request.StartsWith("REGISTER|"))
                {
                    string[] parts = request.Split('|');
                    // REGISTER|username|email|passwordHash
                    if (parts.Length == 4)
                    {
                        string username = parts[1];
                        string email = parts[2];
                        string passwordHash = parts[3];

                        bool success = DatabaseHelper.RegisterUser(username, email, passwordHash);
                        response = success ? "Đăng ký thành công!" : "Username hoặc email đã tồn tại!";
                    }
                    else response = "Dữ liệu đăng ký không hợp lệ.";
                }
                else if (request.StartsWith("LOGIN|"))
                {
                    string[] parts = request.Split('|');
                    // LOGIN|usernameOrEmail|passwordHash
                    if (parts.Length == 3)
                    {
                        string usernameOrEmail = parts[1];
                        string passwordHash = parts[2];

                        bool ok = DatabaseHelper.CheckLogin(usernameOrEmail, passwordHash);
                        response = ok ? "Đăng nhập thành công!" : "Sai tài khoản hoặc mật khẩu!";
                    }
                    else response = "Dữ liệu đăng nhập không hợp lệ.";
                }
                else
                {
                    response = "Lệnh không hợp lệ.";
                }

                byte[] responseData = Encoding.UTF8.GetBytes(response);
                stream.Write(responseData, 0, responseData.Length);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
            }
            finally
            {
                client.Close();
            }
        }
        private void Log(string message)
        {
            Invoke(new Action(() =>
            {
                lstLog.Items.Add(message); // lstLog là ListBox
                                           // Tự cuộn xuống item mới
                lstLog.TopIndex = lstLog.Items.Count - 1;

                // Giới hạn số item tối đa (ví dụ 1000)
                if (lstLog.Items.Count > 1000)
                    lstLog.Items.RemoveAt(0);
            }));
        }
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false; // Dừng vòng lặp accept client
                listener.Stop();   // Ngắt TcpListener
                lb_status.Text = "🔴 Server đã dừng.";
                Log("Server đã dừng.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
