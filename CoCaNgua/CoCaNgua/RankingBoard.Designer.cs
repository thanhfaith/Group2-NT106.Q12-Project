namespace CoCaNgua
{
    partial class RankingBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RankingBoard));
            listBox1 = new ListBox();
            llblQuit = new LinkLabel();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.Ivory;
            listBox1.BorderStyle = BorderStyle.None;
            listBox1.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 26;
            listBox1.Location = new Point(189, 156);
            listBox1.Margin = new Padding(5);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(457, 208);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // llblQuit
            // 
            llblQuit.AutoSize = true;
            llblQuit.BackColor = Color.Transparent;
            llblQuit.Font = new Font("Comic Sans MS", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            llblQuit.Location = new Point(406, 383);
            llblQuit.Name = "llblQuit";
            llblQuit.Size = new Size(84, 29);
            llblQuit.TabIndex = 1;
            llblQuit.TabStop = true;
            llblQuit.Text = "Trở Về";
            llblQuit.LinkClicked += llblQuit_LinkClicked;
            // 
            // RankingBoard
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(901, 508);
            Controls.Add(llblQuit);
            Controls.Add(listBox1);
            DoubleBuffered = true;
            Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "RankingBoard";
            Text = "Bảng xếp hạng";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private LinkLabel llblQuit;
    }
}