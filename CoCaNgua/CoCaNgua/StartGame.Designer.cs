namespace CoCaNgua
{
    partial class StartGame
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
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_010746;
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.Cursor = Cursors.No;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(405, 242);
            button1.Margin = new Padding(5, 5, 5, 5);
            button1.Name = "button1";
            button1.Size = new Size(198, 74);
            button1.TabIndex = 0;
            button1.Text = "Đăng nhập ";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_010746;
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Cursor = Cursors.No;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(405, 336);
            button2.Margin = new Padding(5, 5, 5, 5);
            button2.Name = "button2";
            button2.Size = new Size(198, 74);
            button2.TabIndex = 1;
            button2.Text = "Đăng kí ";
            button2.UseVisualStyleBackColor = true;
            // 
            // StartGame
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_15_005328;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(916, 581);
            Controls.Add(button2);
            Controls.Add(button1);
            DoubleBuffered = true;
            Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5, 5, 5, 5);
            Name = "StartGame";
            Text = "StartGame";
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private Button button2;
    }
}