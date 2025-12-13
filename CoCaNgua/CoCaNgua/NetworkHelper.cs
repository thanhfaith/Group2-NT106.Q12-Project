using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CoCaNgua
{
    public class NetworkHelper
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;

        // Sự kiện để bắn tin nhắn về cho Form (ChessBoard, Menu,...) xử lý
        public event Action<string> OnMessageReceived;

        public bool IsConnected => client != null && client.Connected;

        public NetworkHelper()
        {
            client = new TcpClient();
        }

        public bool Connect(string ip, int port)
        {
            try
            {
                if (client != null && client.Connected) return true;

                // Nếu client đã bị đóng, tạo mới
                if (client == null || !client.Connected)
                    client = new TcpClient();

                client.Connect(ip, port);
                stream = client.GetStream();

                // Start receive loop once
                if (receiveThread == null || !receiveThread.IsAlive)
                {
                    receiveThread = new Thread(ReceiveLoop);
                    receiveThread.IsBackground = true;
                    receiveThread.Start();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}");
                return false;
            }
        }

        public void Send(string message)
        {
            try
            {
                if (client != null && client.Connected && stream != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send Error: " + ex.Message);
                try { OnMessageReceived?.Invoke("ERROR|Mất kết nối tới Server"); } catch { }
            }
        }

        private void ReceiveLoop()
        {
            byte[] buffer = new byte[4096];

            while (client != null && client.Connected)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        // Fire event (subscriber should marshal to UI thread if needed)
                        try { OnMessageReceived?.Invoke(message); } catch { }
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }

            try { OnMessageReceived?.Invoke("ERROR|Đã ngắt kết nối với Server."); } catch { }
        }

        public void Disconnect()
        {
            try
            {
                if (stream != null) stream.Close();
                if (client != null) client.Close();
                stream = null;
                client = null;
            }
            catch { }
        }
    }
}
