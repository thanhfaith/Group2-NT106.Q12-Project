namespace CoCaNgua
{
    partial class Online
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
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Aqua;
            label2.Location = new Point(156, 281);
            label2.Name = "label2";
            label2.Size = new Size(112, 26);
            label2.TabIndex = 1;
            label2.Text = "Mã phòng ";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.LightGoldenrodYellow;
            textBox1.Location = new Point(336, 274);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(202, 43);
            textBox1.TabIndex = 2;
            // 
            // button1
            // 
            button1.BackColor = Color.Cyan;
            button1.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_013600;
            button1.Location = new Point(349, 200);
            button1.Name = "button1";
            button1.Size = new Size(177, 43);
            button1.TabIndex = 3;
            button1.Text = "Tạo phòng ";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_214511;
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Location = new Point(407, 342);
            button2.Name = "button2";
            button2.Size = new Size(62, 50);
            button2.TabIndex = 4;
            button2.UseVisualStyleBackColor = true;
            // 
            // Online
            // 
            AutoScaleDimensions = new SizeF(13F, 26F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Blue;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_15_013300;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(804, 471);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label2);
            DoubleBuffered = true;
            Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5, 4, 5, 4);
            Name = "Online";
            Text = "Online";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private TextBox textBox1;
        private Button button1;
        private Button button2;
    }
}