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
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(166, 233);
            label2.Name = "label2";
            label2.Size = new Size(121, 31);
            label2.TabIndex = 1;
            label2.Text = "Mã phòng ";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.LightGoldenrodYellow;
            textBox1.Location = new Point(322, 222);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(217, 50);
            textBox1.TabIndex = 2;
            // 
            // button1
            // 
            button1.BackColor = Color.Cyan;
            button1.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_013600;
            button1.Location = new Point(582, 222);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(191, 51);
            button1.TabIndex = 3;
            button1.Text = "Tạo phòng ";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_214511;
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Location = new Point(391, 310);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(67, 60);
            button2.TabIndex = 4;
            button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(365, 374);
            label1.Name = "label1";
            label1.Size = new Size(116, 31);
            label1.TabIndex = 5;
            label1.Text = "BẮT ĐẦU";
            // 
            // CodeRoom
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Blue;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_15_013300;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(866, 562);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
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
        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private Label label1;
    }
}