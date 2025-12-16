namespace CoCaNgua
{
    partial class ChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            btnSend = new Button();
            txtMessage = new TextBox();
            rtbChatLog = new RichTextBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.LavenderBlush;
            groupBox1.Controls.Add(btnSend);
            groupBox1.Controls.Add(txtMessage);
            groupBox1.Controls.Add(rtbChatLog);
            groupBox1.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(1, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(520, 495);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Chat";
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.PaleTurquoise;
            btnSend.Location = new Point(457, 437);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(46, 42);
            btnSend.TabIndex = 2;
            btnSend.Text = "Gửi";
            btnSend.UseVisualStyleBackColor = false;
            // 
            // txtMessage
            // 
            txtMessage.BackColor = Color.FloralWhite;
            txtMessage.Location = new Point(14, 437);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(437, 42);
            txtMessage.TabIndex = 1;
            // 
            // rtbChatLog
            // 
            rtbChatLog.BackColor = Color.FloralWhite;
            rtbChatLog.Font = new Font("Times New Roman", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbChatLog.Location = new Point(14, 36);
            rtbChatLog.Name = "rtbChatLog";
            rtbChatLog.ReadOnly = true;
            rtbChatLog.Size = new Size(489, 383);
            rtbChatLog.TabIndex = 0;
            rtbChatLog.Text = "";
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(522, 495);
            Controls.Add(groupBox1);
            Name = "ChatForm";
            Text = "ChatForm";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnSend;
        private TextBox txtMessage;
        private RichTextBox rtbChatLog;
    }
}