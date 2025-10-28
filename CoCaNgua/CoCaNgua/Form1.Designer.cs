namespace CoCaNgua
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.Wheat;
            button1.Location = new Point(470, 272);
            button1.Name = "button1";
            button1.Size = new Size(184, 84);
            button1.TabIndex = 0;
            button1.Text = "Online Multiplayer ";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.Wheat;
            button2.Location = new Point(470, 423);
            button2.Name = "button2";
            button2.Size = new Size(184, 84);
            button2.TabIndex = 1;
            button2.Text = "Offline Computer ";
            button2.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(15F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_10_28_233722;
            ClientSize = new Size(1086, 605);
            Controls.Add(button2);
            Controls.Add(button1);
            Font = new Font("Times New Roman", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6, 5, 6, 5);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
    }
}
