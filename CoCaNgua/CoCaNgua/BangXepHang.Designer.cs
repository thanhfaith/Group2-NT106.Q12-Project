namespace CoCaNgua
{
    partial class BangXepHang
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
            dataGridView1 = new DataGridView();
            label1 = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.PaleGreen;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(93, 78);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(355, 376);
            dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.LightGoldenrodYellow;
            label1.Font = new Font("Times New Roman", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.OrangeRed;
            label1.Location = new Point(175, 23);
            label1.Name = "label1";
            label1.Size = new Size(201, 32);
            label1.TabIndex = 1;
            label1.Text = "Bảng xếp hạng ";
            // 
            // button1
            // 
            button1.BackColor = Color.Pink;
            button1.ForeColor = Color.MediumVioletRed;
            button1.Location = new Point(212, 480);
            button1.Name = "button1";
            button1.Size = new Size(126, 56);
            button1.TabIndex = 2;
            button1.Text = "Quay lại ";
            button1.UseVisualStyleBackColor = false;
            // 
            // BangXepHang
            // 
            AutoScaleDimensions = new SizeF(15F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGoldenrodYellow;
            ClientSize = new Size(559, 560);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Font = new Font("Times New Roman", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6, 5, 6, 5);
            Name = "BangXepHang";
            Text = "BangXepHang";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Label label1;
        private Button button1;
    }
}