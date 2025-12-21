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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaitingRoom));
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
            lblRoomCode.Font = new Font("Times New Roman", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRoomCode.Location = new Point(212, 24);
            lblRoomCode.Margin = new Padding(5, 0, 5, 0);
            lblRoomCode.Name = "lblRoomCode";
            lblRoomCode.Size = new Size(137, 35);
            lblRoomCode.TabIndex = 0;
            lblRoomCode.Text = "Mã phòng ";
            // 
            // txtRoomCode
            // 
            txtRoomCode.BackColor = Color.FloralWhite;
            txtRoomCode.Location = new Point(380, 25);
            txtRoomCode.Name = "txtRoomCode";
            txtRoomCode.Size = new Size(185, 34);
            txtRoomCode.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.MistyRose;
            groupBox1.Controls.Add(lstPlayers);
            groupBox1.Font = new Font("Times New Roman", 13.8F);
            groupBox1.Location = new Point(187, 98);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(451, 307);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Danh sách các thành viên";
            // 
            // lstPlayers
            // 
            lstPlayers.FormattingEnabled = true;
            lstPlayers.ItemHeight = 26;
            lstPlayers.Location = new Point(21, 48);
            lstPlayers.Name = "lstPlayers";
            lstPlayers.Size = new Size(409, 238);
            lstPlayers.TabIndex = 0;
            lstPlayers.SelectedIndexChanged += lstPlayers_SelectedIndexChanged;
            // 
            // btnStartGame
            // 
            btnStartGame.BackColor = Color.DimGray;
            btnStartGame.BackgroundImage = (Image)resources.GetObject("btnStartGame.BackgroundImage");
            btnStartGame.BackgroundImageLayout = ImageLayout.Stretch;
            btnStartGame.FlatAppearance.BorderSize = 0;
            btnStartGame.FlatStyle = FlatStyle.Flat;
            btnStartGame.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnStartGame.Location = new Point(316, 423);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(183, 67);
            btnStartGame.TabIndex = 3;
            btnStartGame.Text = "BẮT ĐẦU";
            btnStartGame.UseVisualStyleBackColor = false;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // WaitingRoom
            // 
            AutoScaleDimensions = new SizeF(13F, 26F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(826, 494);
            Controls.Add(btnStartGame);
            Controls.Add(groupBox1);
            Controls.Add(txtRoomCode);
            Controls.Add(lblRoomCode);
            DoubleBuffered = true;
            Font = new Font("Times New Roman", 13.8F);
            ForeColor = SystemColors.ControlText;
            Margin = new Padding(5, 4, 5, 4);
            Name = "WaitingRoom";
            Text = "Phòng đợi";
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