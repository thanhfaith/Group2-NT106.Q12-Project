namespace CoCaNguaServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btn_Start = new Button();
            btn_Stop = new Button();
            lstClients = new ListBox();
            lb_status = new Label();
            lstLog = new ListBox();
            SuspendLayout();
            // 
            // btn_Start
            // 
            btn_Start.BackColor = Color.SteelBlue;
            btn_Start.BackgroundImageLayout = ImageLayout.Stretch;
            btn_Start.FlatAppearance.BorderSize = 0;
            btn_Start.FlatStyle = FlatStyle.Flat;
            btn_Start.Font = new Font("Times New Roman", 12F);
            btn_Start.ForeColor = Color.White;
            btn_Start.Location = new Point(12, 63);
            btn_Start.Name = "btn_Start";
            btn_Start.Size = new Size(107, 48);
            btn_Start.TabIndex = 0;
            btn_Start.Text = "Start";
            btn_Start.UseVisualStyleBackColor = false;
            btn_Start.Click += btn_Start_Click;
            // 
            // btn_Stop
            // 
            btn_Stop.BackColor = Color.SteelBlue;
            btn_Stop.BackgroundImageLayout = ImageLayout.Stretch;
            btn_Stop.FlatAppearance.BorderSize = 0;
            btn_Stop.FlatStyle = FlatStyle.Flat;
            btn_Stop.Font = new Font("Times New Roman", 12F);
            btn_Stop.ForeColor = Color.White;
            btn_Stop.Location = new Point(12, 131);
            btn_Stop.Name = "btn_Stop";
            btn_Stop.Size = new Size(107, 48);
            btn_Stop.TabIndex = 1;
            btn_Stop.Text = "Stop";
            btn_Stop.UseVisualStyleBackColor = false;
            btn_Stop.Click += btn_Stop_Click;
            // 
            // lstClients
            // 
            lstClients.BackColor = Color.PowderBlue;
            lstClients.Font = new Font("Times New Roman", 12F);
            lstClients.FormattingEnabled = true;
            lstClients.ItemHeight = 22;
            lstClients.Location = new Point(141, 54);
            lstClients.Name = "lstClients";
            lstClients.Size = new Size(318, 136);
            lstClients.TabIndex = 2;
            // 
            // lb_status
            // 
            lb_status.AutoSize = true;
            lb_status.BackColor = Color.Transparent;
            lb_status.Font = new Font("Times New Roman", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lb_status.Location = new Point(25, 212);
            lb_status.Name = "lb_status";
            lb_status.Size = new Size(79, 33);
            lb_status.TabIndex = 3;
            lb_status.Text = "status";
            // 
            // lstLog
            // 
            lstLog.BackColor = Color.PowderBlue;
            lstLog.Font = new Font("Times New Roman", 12F);
            lstLog.FormattingEnabled = true;
            lstLog.ItemHeight = 22;
            lstLog.Location = new Point(25, 248);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(434, 158);
            lstLog.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(500, 504);
            Controls.Add(lstLog);
            Controls.Add(lb_status);
            Controls.Add(lstClients);
            Controls.Add(btn_Stop);
            Controls.Add(btn_Start);
            Name = "Form1";
            Text = "Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_Start;
        private Button btn_Stop;
        private ListBox lstClients;
        private Label lb_status;
        private ListBox lstLog;
    }
}
