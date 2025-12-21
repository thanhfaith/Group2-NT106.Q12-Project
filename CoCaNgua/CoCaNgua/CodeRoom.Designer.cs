namespace CoCaNgua
{
    partial class CodeRoom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeRoom));
            label2 = new Label();
            txtRoomCode = new TextBox();
            btnCreateRoom = new Button();
            btnJoinRoom = new Button();
            label1 = new Label();
            btnLogout = new Button();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Times New Roman", 16.2F);
            label2.Location = new Point(165, 239);
            label2.Name = "label2";
            label2.Size = new Size(135, 33);
            label2.TabIndex = 1;
            label2.Text = "Mã phòng ";
            // 
            // txtRoomCode
            // 
            txtRoomCode.BackColor = Color.LightGoldenrodYellow;
            txtRoomCode.Location = new Point(318, 232);
            txtRoomCode.Margin = new Padding(3, 4, 3, 4);
            txtRoomCode.Name = "txtRoomCode";
            txtRoomCode.Size = new Size(217, 40);
            txtRoomCode.TabIndex = 2;
            // 
            // btnCreateRoom
            // 
            btnCreateRoom.BackColor = Color.Cyan;
            btnCreateRoom.BackgroundImage = (Image)resources.GetObject("btnCreateRoom.BackgroundImage");
            btnCreateRoom.BackgroundImageLayout = ImageLayout.Stretch;
            btnCreateRoom.FlatAppearance.BorderSize = 0;
            btnCreateRoom.FlatStyle = FlatStyle.Flat;
            btnCreateRoom.Font = new Font("Times New Roman", 16.2F);
            btnCreateRoom.Location = new Point(596, 226);
            btnCreateRoom.Margin = new Padding(3, 4, 3, 4);
            btnCreateRoom.Name = "btnCreateRoom";
            btnCreateRoom.Size = new Size(159, 57);
            btnCreateRoom.TabIndex = 3;
            btnCreateRoom.Text = "Tạo phòng ";
            btnCreateRoom.UseVisualStyleBackColor = false;
            btnCreateRoom.Click += btnCreateRoom_Click;
            // 
            // btnJoinRoom
            // 
            btnJoinRoom.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_214511;
            btnJoinRoom.BackgroundImageLayout = ImageLayout.Stretch;
            btnJoinRoom.FlatAppearance.BorderSize = 0;
            btnJoinRoom.FlatStyle = FlatStyle.Flat;
            btnJoinRoom.Location = new Point(393, 310);
            btnJoinRoom.Margin = new Padding(3, 4, 3, 4);
            btnJoinRoom.Name = "btnJoinRoom";
            btnJoinRoom.Size = new Size(73, 60);
            btnJoinRoom.TabIndex = 4;
            btnJoinRoom.UseVisualStyleBackColor = true;
            btnJoinRoom.Click += btnJoinRoom_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Times New Roman", 16.2F);
            label1.Location = new Point(358, 374);
            label1.Name = "label1";
            label1.Size = new Size(137, 33);
            label1.TabIndex = 5;
            label1.Text = "Vào phòng";
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.IndianRed;
            btnLogout.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogout.Location = new Point(783, 12);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(72, 70);
            btnLogout.TabIndex = 6;
            btnLogout.Text = "Đăng xuất";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // CodeRoom
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Blue;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(867, 548);
            Controls.Add(btnLogout);
            Controls.Add(label1);
            Controls.Add(btnJoinRoom);
            Controls.Add(btnCreateRoom);
            Controls.Add(txtRoomCode);
            Controls.Add(label2);
            DoubleBuffered = true;
            Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "CodeRoom";
            Text = "Tạo phòng/Tham gia";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private TextBox txtRoomCode;
        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Label label1;
        private Button btnLogout;
    }
}