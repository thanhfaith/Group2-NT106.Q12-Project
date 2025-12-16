using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CoCaNgua
{
    partial class WaitingRoom
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
            components = new System.ComponentModel.Container();
            lblRoomCode = new Label();
            txtRoomCode = new TextBox();
            groupBox1 = new GroupBox();
            lstPlayers = new ListBox();
            btnStartGame = new Button();
            timerRefresh = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblRoomCode
            // 
            lblRoomCode.AutoSize = true;
            lblRoomCode.BackColor = Color.MistyRose;
            lblRoomCode.BorderStyle = BorderStyle.FixedSingle;
            lblRoomCode.Cursor = Cursors.No;
            lblRoomCode.Location = new Point(130, 62);
            lblRoomCode.Margin = new Padding(5, 0, 5, 0);
            lblRoomCode.Name = "lblRoomCode";
            lblRoomCode.Size = new Size(123, 33);
            lblRoomCode.TabIndex = 0;
            lblRoomCode.Text = "Mã phòng ";
            // 
            // txtRoomCode
            // 
            txtRoomCode.BackColor = Color.FloralWhite;
            txtRoomCode.Location = new Point(280, 51);
            txtRoomCode.Margin = new Padding(3, 4, 3, 4);
            txtRoomCode.Multiline = true;
            txtRoomCode.Name = "txtRoomCode";
            txtRoomCode.Size = new Size(199, 48);
            txtRoomCode.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.MistyRose;
            groupBox1.Controls.Add(lstPlayers);
            groupBox1.Location = new Point(93, 140);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(472, 429);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Danh sách các thành viên";
            // 
            // lstPlayers
            // 
            lstPlayers.FormattingEnabled = true;
            lstPlayers.ItemHeight = 31;
            lstPlayers.Location = new Point(18, 72);
            lstPlayers.Margin = new Padding(3, 4, 3, 4);
            lstPlayers.Name = "lstPlayers";
            lstPlayers.Size = new Size(436, 252);
            lstPlayers.TabIndex = 0;
            // 
            // btnStartGame
            // 
            btnStartGame.BackColor = Color.DimGray;
            btnStartGame.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_010746;
            btnStartGame.BackgroundImageLayout = ImageLayout.Stretch;
            btnStartGame.FlatStyle = FlatStyle.Popup;
            btnStartGame.Location = new Point(249, 581);
            btnStartGame.Margin = new Padding(3, 4, 3, 4);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(138, 66);
            btnStartGame.TabIndex = 3;
            btnStartGame.Text = "BẮT ĐẦU";
            btnStartGame.UseVisualStyleBackColor = false;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // WaitingRoom
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_15_221556;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(634, 656);
            Controls.Add(btnStartGame);
            Controls.Add(groupBox1);
            Controls.Add(txtRoomCode);
            Controls.Add(lblRoomCode);
            DoubleBuffered = true;
            Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.ControlText;
            Margin = new Padding(5);
            Name = "WaitingRoom";
            Text = "Taophong";
            Load += WaitingRoom_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRoomCode;
        private TextBox txtRoomCode;
        private GroupBox groupBox1;
        private ListBox lstPlayers;
        private Button btnStartGame;
        private System.Windows.Forms.Timer timerRefresh;
    }
}