namespace CoCaNgua
{
    partial class OnlineMultiplayer
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
            button1.BackColor = Color.Aquamarine;
            button1.ForeColor = Color.Navy;
            button1.Location = new Point(256, 105);
            button1.Name = "button1";
            button1.Size = new Size(194, 72);
            button1.TabIndex = 0;
            button1.Text = "Tạo phòng ";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.Aquamarine;
            button2.ForeColor = Color.Navy;
            button2.Location = new Point(256, 242);
            button2.Name = "button2";
            button2.Size = new Size(194, 72);
            button2.TabIndex = 1;
            button2.Text = "Vào phòng ";
            button2.UseVisualStyleBackColor = false;
            // 
            // OnlineMultiplayer
            // 
            AutoScaleDimensions = new SizeF(15F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PaleGreen;
            ClientSize = new Size(712, 410);
            Controls.Add(button2);
            Controls.Add(button1);
            Font = new Font("Times New Roman", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6, 5, 6, 5);
            Name = "OnlineMultiplayer";
            Text = "OnlineMultiplayer";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
    }
}