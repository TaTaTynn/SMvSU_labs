namespace TCPClient
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.textBoxRequest = new System.Windows.Forms.TextBox();
            this.buttonSendRequest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.labelGUID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(213, 13);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(213, 42);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 1;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // textBoxRequest
            // 
            this.textBoxRequest.Location = new System.Drawing.Point(12, 71);
            this.textBoxRequest.Name = "textBoxRequest";
            this.textBoxRequest.Size = new System.Drawing.Size(194, 23);
            this.textBoxRequest.TabIndex = 2;
            // 
            // buttonSendRequest
            // 
            this.buttonSendRequest.Location = new System.Drawing.Point(212, 71);
            this.buttonSendRequest.Name = "buttonSendRequest";
            this.buttonSendRequest.Size = new System.Drawing.Size(75, 23);
            this.buttonSendRequest.TabIndex = 3;
            this.buttonSendRequest.Text = "Отправить";
            this.buttonSendRequest.UseVisualStyleBackColor = true;
            this.buttonSendRequest.Click += new System.EventHandler(this.buttonSendRequest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(26, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Время:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(80, 42);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(17, 15);
            this.labelTime.TabIndex = 5;
            this.labelTime.Text = "--";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(26, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Состояние:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelState.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelState.Location = new System.Drawing.Point(94, 17);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(86, 15);
            this.labelState.TabIndex = 4;
            this.labelState.Text = "Not connected";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelGUID
            // 
            this.labelGUID.AutoSize = true;
            this.labelGUID.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelGUID.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelGUID.Location = new System.Drawing.Point(0, 0);
            this.labelGUID.Name = "labelGUID";
            this.labelGUID.Size = new System.Drawing.Size(18, 15);
            this.labelGUID.TabIndex = 6;
            this.labelGUID.Text = "-1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 106);
            this.Controls.Add(this.labelGUID);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSendRequest);
            this.Controls.Add(this.textBoxRequest);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnect);
            this.Name = "Form1";
            this.Text = "TCP/IP Клиент";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button buttonConnect;
        private Button buttonDisconnect;
        private TextBox textBoxRequest;
        private Button buttonSendRequest;
        private Label label1;
        private Label labelTime;
        private Label label2;
        private Label labelState;
        private Label labelGUID;
    }
}