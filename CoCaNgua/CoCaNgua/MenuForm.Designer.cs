namespace CoCaNgua
{
    partial class MenuForm
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
            btnRule = new Button();
            btnPlay = new Button();
            SuspendLayout();
            // 
            // btnRule
            // 
            btnRule.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_010746;
            btnRule.BackgroundImageLayout = ImageLayout.Stretch;
            btnRule.Cursor = Cursors.Hand;
            btnRule.FlatStyle = FlatStyle.Popup;
            btnRule.Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRule.Location = new Point(367, 219);
            btnRule.Margin = new Padding(5);
            btnRule.Name = "btnRule";
            btnRule.Size = new Size(198, 74);
            btnRule.TabIndex = 6;
            btnRule.Text = "Luật chơi";
            btnRule.UseVisualStyleBackColor = true;
            btnRule.Click += btnRule_Click;
            // 
            // btnPlay
            // 
            btnPlay.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_010746;
            btnPlay.BackgroundImageLayout = ImageLayout.Stretch;
            btnPlay.Cursor = Cursors.Hand;
            btnPlay.FlatStyle = FlatStyle.Popup;
            btnPlay.Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPlay.Location = new Point(367, 303);
            btnPlay.Margin = new Padding(5);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(198, 74);
            btnPlay.TabIndex = 5;
            btnPlay.Text = "Chơi online";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_15_005328;
            ClientSize = new Size(931, 528);
            Controls.Add(btnRule);
            Controls.Add(btnPlay);
            Name = "MenuForm";
            Text = "MenuForm";
            ResumeLayout(false);
        }

        #endregion

        private Button btnRule;
        private Button btnPlay;
    }
}