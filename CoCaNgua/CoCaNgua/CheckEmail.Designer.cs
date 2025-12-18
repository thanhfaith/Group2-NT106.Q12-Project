namespace CoCaNgua
{
    partial class CheckEmail
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
            txtEmail = new TextBox();
            btnXacthucEmail = new Button();
            label1 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.Ivory;
            txtEmail.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(104, 119);
            txtEmail.Margin = new Padding(4, 3, 4, 3);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(373, 34);
            txtEmail.TabIndex = 0;
            // 
            // btnXacthucEmail
            // 
            btnXacthucEmail.BackgroundImage = Properties.Resources.Screenshot_2025_11_28_222956;
            btnXacthucEmail.BackgroundImageLayout = ImageLayout.Stretch;
            btnXacthucEmail.Cursor = Cursors.No;
            btnXacthucEmail.FlatAppearance.BorderSize = 0;
            btnXacthucEmail.FlatStyle = FlatStyle.Flat;
            btnXacthucEmail.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnXacthucEmail.Location = new Point(195, 182);
            btnXacthucEmail.Margin = new Padding(4, 3, 4, 3);
            btnXacthucEmail.Name = "btnXacthucEmail";
            btnXacthucEmail.Size = new Size(158, 54);
            btnXacthucEmail.TabIndex = 1;
            btnXacthucEmail.Text = "Gửi xác thực ";
            btnXacthucEmail.UseVisualStyleBackColor = true;
            btnXacthucEmail.Click += btnXacthucEmail_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(13, 127);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(71, 26);
            label1.TabIndex = 2;
            label1.Text = "Email ";
            label1.Click += label1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Times New Roman", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.MediumVioletRed;
            label3.Location = new Point(133, 43);
            label3.Name = "label3";
            label3.Size = new Size(279, 39);
            label3.TabIndex = 4;
            label3.Text = "Xác thực tài khoản ";
            // 
            // CheckEmail
            // 
            AutoScaleDimensions = new SizeF(11F, 22F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_28_222745;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(505, 286);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(btnXacthucEmail);
            Controls.Add(txtEmail);
            DoubleBuffered = true;
            Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 3, 4, 3);
            Name = "CheckEmail";
            Text = "CheckEmail";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtEmail;
        private Button btnXacthucEmail;
        private Label label1;
        private Label label3;
    }
}