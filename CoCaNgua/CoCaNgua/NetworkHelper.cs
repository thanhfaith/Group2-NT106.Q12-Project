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
        private StringBuilder messageBuffer = new StringBuilder(); // ✅ THÊM BUFFER

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

                if (client == null || !client.Connected)
                    client = new TcpClient();

                client.Connect(ip, port);
                client.NoDelay = true; // ✅ DISABLE NAGLE'S ALGORITHM

                stream = client.GetStream();

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
                    byte[] data = Encoding.UTF8.GetBytes(message + "\n"); // ✅ THÊM \n
                    stream.Write(data, 0, data.Length);
                    stream.Flush(); // ✅ FORCE GỬI NGAY
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
                        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        messageBuffer.Append(data);

                        // ✅ TÁCH MESSAGE BẰNG \n
                        string allData = messageBuffer.ToString();
                        int newlineIndex;

                        while ((newlineIndex = allData.IndexOf('\n')) >= 0)
                        {
                            string message = allData.Substring(0, newlineIndex).Trim();
                            allData = allData.Substring(newlineIndex + 1);

                            if (!string.IsNullOrEmpty(message))
                            {
                                try
                                {
                                    OnMessageReceived?.Invoke(message);
                                }
                                catch { }
                            }
                        }

                        // Cập nhật buffer
                        messageBuffer.Clear();
                        messageBuffer.Append(allData);
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