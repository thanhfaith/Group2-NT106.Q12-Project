namespace CoCaNgua
{
    partial class PhongCaNgua
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
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.LavenderBlush;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 31;
            listBox1.Location = new Point(212, 131);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(367, 345);
            listBox1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.LemonChiffon;
            label1.ForeColor = Color.Black;
            label1.Location = new Point(50, 60);
            label1.Name = "label1";
            label1.Size = new Size(128, 33);
            label1.TabIndex = 1;
            label1.Text = "Mã phòng";
            label1.Click += label1_Click;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.LavenderBlush;
            textBox1.Location = new Point(263, 57);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(267, 39);
            textBox1.TabIndex = 2;
            // 
            // button1
            // 
            button1.BackColor = Color.PaleGreen;
            button1.BackgroundImageLayout = ImageLayout.Center;
            button1.ForeColor = Color.DarkMagenta;
            button1.Location = new Point(619, 227);
            button1.Name = "button1";
            button1.Size = new Size(135, 58);
            button1.TabIndex = 3;
            button1.Text = "Sẵn sàng";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.PaleGreen;
            button2.BackgroundImageLayout = ImageLayout.Center;
            button2.ForeColor = Color.DarkMagenta;
            button2.Location = new Point(619, 330);
            button2.Name = "button2";
            button2.Size = new Size(135, 58);
            button2.TabIndex = 4;
            button2.Text = "Thoát phòng ";
            button2.UseVisualStyleBackColor = false;
            // 
            // PhongCaNgua
            // 
            AutoScaleDimensions = new SizeF(15F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Pink;
            ClientSize = new Size(802, 526);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(listBox1);
            Font = new Font("Times New Roman", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6, 5, 6, 5);
            Name = "PhongCaNgua";
            Text = "PhongCaNgua";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private Button button2;
    }
}