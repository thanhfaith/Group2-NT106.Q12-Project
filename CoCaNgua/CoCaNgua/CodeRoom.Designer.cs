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
            label2 = new Label();
            txtRoomCode = new TextBox();
            btnCreateRoom = new Button();
            btnJoinRoom = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(166, 233);
            label2.Name = "label2";
            label2.Size = new Size(147, 39);
            label2.TabIndex = 1;
            label2.Text = "Mã phòng ";
            // 
            // txtRoomCode
            // 
            txtRoomCode.BackColor = Color.LightGoldenrodYellow;
            txtRoomCode.Location = new Point(322, 222);
            txtRoomCode.Margin = new Padding(3, 4, 3, 4);
            txtRoomCode.Multiline = true;
            txtRoomCode.Name = "txtRoomCode";
            txtRoomCode.Size = new Size(217, 50);
            txtRoomCode.TabIndex = 2;
            // 
            // btnCreateRoom
            // 
            btnCreateRoom.BackColor = Color.Cyan;
            btnCreateRoom.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_013600;
            btnCreateRoom.Location = new Point(582, 222);
            btnCreateRoom.Margin = new Padding(3, 4, 3, 4);
            btnCreateRoom.Name = "btnCreateRoom";
            btnCreateRoom.Size = new Size(191, 51);
            btnCreateRoom.TabIndex = 3;
            btnCreateRoom.Text = "Tạo phòng ";
            btnCreateRoom.UseVisualStyleBackColor = false;
            btnCreateRoom.Click += btnCreateRoom_Click;
            // 
            // btnJoinRoom
            // 
            btnJoinRoom.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_214511;
            btnJoinRoom.BackgroundImageLayout = ImageLayout.Stretch;
            btnJoinRoom.Location = new Point(391, 310);
            btnJoinRoom.Margin = new Padding(3, 4, 3, 4);
            btnJoinRoom.Name = "btnJoinRoom";
            btnJoinRoom.Size = new Size(67, 60);
            btnJoinRoom.TabIndex = 4;
            btnJoinRoom.UseVisualStyleBackColor = true;
            btnJoinRoom.Click += btnJoinRoom_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(365, 374);
            label1.Name = "label1";
            label1.Size = new Size(147, 39);
            label1.TabIndex = 5;
            label1.Text = "BẮT ĐẦU";
            // 
            // CodeRoom
            // 
            AutoScaleDimensions = new SizeF(17F, 39F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Blue;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_15_013300;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(866, 562);
            Controls.Add(label1);
            Controls.Add(btnJoinRoom);
            Controls.Add(btnCreateRoom);
            Controls.Add(txtRoomCode);
            Controls.Add(label2);
            DoubleBuffered = true;
            Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "CodeRoom";
            Text = "Online";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private TextBox txtRoomCode;
        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Label label1;
    }
}