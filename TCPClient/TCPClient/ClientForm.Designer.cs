namespace TCPClient
{
    partial class ClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.textBoxRequest = new System.Windows.Forms.TextBox();
            this.buttonSendRequest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.labelGUID = new System.Windows.Forms.Label();
            this.checkSync = new System.Windows.Forms.CheckBox();
            this.checkAsync = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxGUID = new System.Windows.Forms.TextBox();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelGUIDfrom = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonClean = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(243, 17);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(97, 31);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(243, 56);
            this.buttonDisconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(97, 31);
            this.buttonDisconnect.TabIndex = 1;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // textBoxRequest
            // 
            this.textBoxRequest.Location = new System.Drawing.Point(14, 114);
            this.textBoxRequest.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxRequest.Name = "textBoxRequest";
            this.textBoxRequest.PlaceholderText = "Request";
            this.textBoxRequest.Size = new System.Drawing.Size(221, 27);
            this.textBoxRequest.TabIndex = 2;
            // 
            // buttonSendRequest
            // 
            this.buttonSendRequest.Location = new System.Drawing.Point(242, 114);
            this.buttonSendRequest.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.buttonSendRequest.Name = "buttonSendRequest";
            this.buttonSendRequest.Size = new System.Drawing.Size(98, 27);
            this.buttonSendRequest.TabIndex = 3;
            this.buttonSendRequest.Text = "Отправить";
            this.buttonSendRequest.UseVisualStyleBackColor = true;
            this.buttonSendRequest.Click += new System.EventHandler(this.buttonSendRequest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(30, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Время:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(91, 62);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(21, 20);
            this.labelTime.TabIndex = 5;
            this.labelTime.Text = "--";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(30, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Состояние:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelState.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelState.Location = new System.Drawing.Point(107, 13);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(107, 20);
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
            this.labelGUID.Size = new System.Drawing.Size(23, 20);
            this.labelGUID.TabIndex = 6;
            this.labelGUID.Text = "-1";
            // 
            // checkSync
            // 
            this.checkSync.AutoSize = true;
            this.checkSync.Enabled = false;
            this.checkSync.Location = new System.Drawing.Point(57, 37);
            this.checkSync.Name = "checkSync";
            this.checkSync.Size = new System.Drawing.Size(59, 24);
            this.checkSync.TabIndex = 7;
            this.checkSync.Text = "sync";
            this.checkSync.UseVisualStyleBackColor = true;
            // 
            // checkAsync
            // 
            this.checkAsync.AutoSize = true;
            this.checkAsync.Enabled = false;
            this.checkAsync.Location = new System.Drawing.Point(123, 37);
            this.checkAsync.Name = "checkAsync";
            this.checkAsync.Size = new System.Drawing.Size(67, 24);
            this.checkAsync.TabIndex = 8;
            this.checkAsync.Text = "async";
            this.checkAsync.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(14, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Запрос серверу";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(14, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Сообщение клиенту";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxGUID
            // 
            this.textBoxGUID.Location = new System.Drawing.Point(14, 173);
            this.textBoxGUID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxGUID.Name = "textBoxGUID";
            this.textBoxGUID.PlaceholderText = "GUID";
            this.textBoxGUID.Size = new System.Drawing.Size(54, 27);
            this.textBoxGUID.TabIndex = 11;
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.Location = new System.Drawing.Point(74, 173);
            this.textBoxMsg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.PlaceholderText = "Message";
            this.textBoxMsg.Size = new System.Drawing.Size(161, 27);
            this.textBoxMsg.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(243, 173);
            this.button1.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 27);
            this.button1.TabIndex = 13;
            this.button1.Text = "Отправить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonSendMsg_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(14, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(169, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Сообщение от клиента";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(14, 234);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "GUID:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelGUIDfrom
            // 
            this.labelGUIDfrom.AutoSize = true;
            this.labelGUIDfrom.Location = new System.Drawing.Point(57, 234);
            this.labelGUIDfrom.Name = "labelGUIDfrom";
            this.labelGUIDfrom.Size = new System.Drawing.Size(21, 20);
            this.labelGUIDfrom.TabIndex = 16;
            this.labelGUIDfrom.Text = "--";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(86, 234);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 20);
            this.label8.TabIndex = 17;
            this.label8.Text = "Сообщение:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonClean
            // 
            this.buttonClean.Location = new System.Drawing.Point(46, 258);
            this.buttonClean.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(98, 27);
            this.buttonClean.TabIndex = 19;
            this.buttonClean.Text = "Очистить";
            this.buttonClean.UseVisualStyleBackColor = true;
            this.buttonClean.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(184, 231);
            this.textBoxMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.PlaceholderText = "Message";
            this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessage.Size = new System.Drawing.Size(157, 54);
            this.textBoxMessage.TabIndex = 20;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 300);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.buttonClean);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelGUIDfrom);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.textBoxGUID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkAsync);
            this.Controls.Add(this.checkSync);
            this.Controls.Add(this.labelGUID);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSendRequest);
            this.Controls.Add(this.textBoxRequest);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ClientForm";
            this.Text = "TCP/IP Клиент";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
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
        private CheckBox checkSync;
        private CheckBox checkAsync;
        private Label label3;
        private Label label4;
        private TextBox textBoxGUID;
        private TextBox textBoxMsg;
        private Button button1;
        private Label label5;
        private Label label6;
        private Label labelGUIDfrom;
        private Label label8;
        private Button buttonClean;
        private TextBox textBoxMessage;
    }
}