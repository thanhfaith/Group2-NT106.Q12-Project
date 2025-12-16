using System;
using System.Windows.Forms;

namespace CoCaNgua
{
    public partial class ChatForm : Form
    {
        private NetworkHelper network;
        private string roomCode;

        public ChatForm(NetworkHelper existingNetwork, string room)
        {
            InitializeComponent();
            this.network = existingNetwork;
            this.roomCode = room;

            // Subscribe to network messages
            if (this.network != null)
            {
                this.network.OnMessageReceived += HandleChatMessage;
            }

            btnSend.Click += BtnSend_Click;
            txtMessage.KeyDown += TxtMessage_KeyDown;
        }

        private void HandleChatMessage(string msg)
        {
            if (this.IsDisposed) return;

            // Chỉ xử lý tin nhắn CHAT
            if (msg.StartsWith("CHAT|"))
            {
                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        string[] parts = msg.Split('|');
                        if (parts.Length >= 3)
                        {
                            string senderName = parts[1];
                            string content = string.Join("|", parts.Skip(2));

                            // ✅ CHỈ HIỂN THỊ TIN NHẮN CỦA NGƯỜI KHÁC
                            if (senderName != Session.Username)
                            {
                                AddToChat($"{senderName}: {content}");
                            }
                        }
                    }
                    catch { }
                });
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string message = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            if (network != null && network.IsConnected)
            {
                // Gửi tin nhắn kèm username
                network.Send($"CHAT|{Session.Username}|{message}");

                // Hiển thị tin nhắn của mình
                AddToChat($"Bạn: {message}");

                txtMessage.Clear();
                txtMessage.Focus();
            }
            else
            {
                MessageBox.Show("Không có kết nối tới server!");
            }
        }

        private void AddToChat(string content)
        {
            if (rtbChatLog.InvokeRequired)
            {
                rtbChatLog.Invoke(new Action(() => AddToChat(content)));
                return;
            }

            string time = DateTime.Now.ToString("HH:mm:ss");
            rtbChatLog.AppendText($"[{time}] {content}{Environment.NewLine}");
            rtbChatLog.ScrollToCaret();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (network != null)
            {
                network.OnMessageReceived -= HandleChatMessage;
            }
            base.OnFormClosing(e);
        }
    }
}