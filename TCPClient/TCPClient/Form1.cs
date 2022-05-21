using System.Diagnostics;
using System.Text;

namespace TCPClient
{
    public partial class Form1 : Form
    {
        private CommunicationClient Client;
        public Form1()
        {
            InitializeComponent();
            Client = new CommunicationClient();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                //Пытаемся соединиться с сервером на локальном компьютере, порт 22222
                int ClientGUID = Client.Connect("localhost", 22222, 33333);
                if (ClientGUID != -1)
                {
                    Trace.TraceInformation(@"Соединение прошло успешно. ClientGuid = {0}", ClientGUID);
                    Client.AsyncTimeReceived += RefreshTime;
                    Client.OnClientDisconnected = ClientDisconnected;
                    labelState.Text = "Connected";
                    labelGUID.Text = " " + ClientGUID.ToString();
                    labelGUID.ForeColor = Color.DarkGreen;
                    labelGUID.BackColor = Color.PaleGreen;
                    checkSync.Checked = Client.IsConnectedSync;
                    checkAsync.Checked = Client.IsConnectedAsync;
                }
                else
                {
                    Client.Disconnect();
                    MessageBox.Show(@"Не удалось соединиться", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Не удалось соединиться", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Trace.TraceError(ex.ToString());
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            Client.Disconnect();
        }

        private void buttonSendRequest_Click(object sender, EventArgs e)
        {
            string text = textBoxRequest.Text;
            byte[] request = Encoding.UTF8.GetBytes(text);
            byte[] reply = null;
            if (Client != null)
            {
                //отправляем запрос, таймаут на ответ - 5 секунд
                int nBytes = Client.SendSуnc(request, out reply, 5);
                //если ответ получен
                if ((nBytes > 0) && (reply != null))
                {
                    string strReply = Encoding.UTF8.GetString(reply);
                    MessageBox.Show(strReply, @"Ответ от сервера", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(@"Не удалось получить ответ от сервера!", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ClientDisconnected()
        {
            labelState.Invoke(new Action(() => labelState.Text = "Not connected"));

            labelGUID.Invoke(new Action(() => labelGUID.Text = "-1"));
            labelGUID.Invoke(new Action(() => labelGUID.ForeColor = System.Drawing.SystemColors.ControlDark));
            labelGUID.Invoke(new Action(() => labelGUID.BackColor = System.Drawing.SystemColors.ControlLight));

            checkSync.Invoke(new Action(() => checkSync.Checked = Client.IsConnectedSync));
            checkAsync.Invoke(new Action(() => checkAsync.Checked = Client.IsConnectedAsync));
        }
        private void RefreshTime()
        {
            labelTime.Invoke(new Action(() => labelTime.ForeColor = Color.Red));
            labelTime.Invoke(new Action(() => labelTime.Text = Client.Time));
            Thread.Sleep(100);
            labelTime.Invoke(new Action(() => labelTime.ForeColor=Color.Black));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.Disconnect();
        }
    }
}