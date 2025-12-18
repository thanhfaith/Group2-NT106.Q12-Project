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
            listBox1 = new ListBox();
            llblQuit = new LinkLabel();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.White;
            listBox1.BorderStyle = BorderStyle.None;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 31;
            listBox1.Location = new Point(65, 112);
            listBox1.Margin = new Padding(5);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(664, 279);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // llblQuit
            // 
            llblQuit.AutoSize = true;
            llblQuit.BackColor = Color.Transparent;
            llblQuit.Location = new Point(330, 477);
            llblQuit.Name = "llblQuit";
            llblQuit.Size = new Size(160, 31);
            llblQuit.TabIndex = 1;
            llblQuit.TabStop = true;
            llblQuit.Text = "Về Trang Chủ";
            llblQuit.LinkClicked += llblQuit_LinkClicked;
            // 
            // RankingBoard
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_15_231600;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(812, 568);
            Controls.Add(llblQuit);
            Controls.Add(listBox1);
            DoubleBuffered = true;
            Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "RankingBoard";
            Text = "Bangxephang";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private LinkLabel llblQuit;
    }
}