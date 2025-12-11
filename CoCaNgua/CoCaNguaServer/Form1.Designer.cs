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
            btn_Start = new Button();
            btn_Stop = new Button();
            lstClients = new ListBox();
            lb_status = new Label();
            lstLog = new ListBox();
            SuspendLayout();
            // 
            // btn_Start
            // 
            btn_Start.Location = new Point(12, 12);
            btn_Start.Name = "btn_Start";
            btn_Start.Size = new Size(97, 68);
            btn_Start.TabIndex = 0;
            btn_Start.Text = "Start";
            btn_Start.UseVisualStyleBackColor = true;
            btn_Start.Click += btn_Start_Click;
            // 
            // btn_Stop
            // 
            btn_Stop.Location = new Point(12, 86);
            btn_Stop.Name = "btn_Stop";
            btn_Stop.Size = new Size(97, 70);
            btn_Stop.TabIndex = 1;
            btn_Stop.Text = "Stop";
            btn_Stop.UseVisualStyleBackColor = true;
            btn_Stop.Click += btn_Stop_Click;
            // 
            // lstClients
            // 
            lstClients.FormattingEnabled = true;
            lstClients.Location = new Point(115, 12);
            lstClients.Name = "lstClients";
            lstClients.Size = new Size(237, 144);
            lstClients.TabIndex = 2;
            // 
            // lb_status
            // 
            lb_status.AutoSize = true;
            lb_status.Location = new Point(12, 170);
            lb_status.Name = "lb_status";
            lb_status.Size = new Size(47, 20);
            lb_status.TabIndex = 3;
            lb_status.Text = "status";
            // 
            // lstLog
            // 
            lstLog.FormattingEnabled = true;
            lstLog.Location = new Point(12, 203);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(340, 184);
            lstLog.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(363, 397);
            Controls.Add(lstLog);
            Controls.Add(lb_status);
            Controls.Add(lstClients);
            Controls.Add(btn_Stop);
            Controls.Add(btn_Start);
            Name = "Form1";
            Text = "Form1";
           // Load += Form1_Load;
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
