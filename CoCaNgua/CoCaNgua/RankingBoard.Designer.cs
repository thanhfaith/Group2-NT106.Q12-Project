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
            llblQuit.Font = new Font("Times New Roman", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            llblQuit.Location = new Point(374, 378);
            llblQuit.Name = "llblQuit";
            llblQuit.Size = new Size(170, 33);
            llblQuit.TabIndex = 1;
            llblQuit.TabStop = true;
            llblQuit.Text = "Về Trang Chủ";
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
            Text = "0";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private LinkLabel llblQuit;
    }
}