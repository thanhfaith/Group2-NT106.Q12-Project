namespace CoCaNgua
{
    partial class OfflineComputer
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
            button2 = new Button();
            button3 = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // button2
            // 
            button2.BackColor = Color.Turquoise;
            button2.ForeColor = Color.DarkGreen;
            button2.Location = new Point(292, 204);
            button2.Margin = new Padding(6, 5, 6, 5);
            button2.Name = "button2";
            button2.Size = new Size(183, 75);
            button2.TabIndex = 1;
            button2.Text = "3 người chơi";
            button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            button3.BackColor = Color.Turquoise;
            button3.ForeColor = Color.DarkGreen;
            button3.Location = new Point(292, 332);
            button3.Margin = new Padding(6, 5, 6, 5);
            button3.Name = "button3";
            button3.Size = new Size(183, 75);
            button3.TabIndex = 2;
            button3.Text = "4 người chơi";
            button3.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.BackColor = Color.Turquoise;
            button1.ForeColor = Color.DarkGreen;
            button1.Location = new Point(292, 73);
            button1.Margin = new Padding(6, 5, 6, 5);
            button1.Name = "button1";
            button1.Size = new Size(183, 75);
            button1.TabIndex = 0;
            button1.Text = "2 người chơi ";
            button1.UseVisualStyleBackColor = false;
            // 
            // OfflineComputer
            // 
            AutoScaleDimensions = new SizeF(15F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PaleGreen;
            ClientSize = new Size(778, 470);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Font = new Font("Times New Roman", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6, 5, 6, 5);
            Name = "OfflineComputer";
            Text = "OfflineComputer";
            ResumeLayout(false);
        }

        #endregion

        private Button button2;
        private Button button3;
        private Button button1;
    }
}