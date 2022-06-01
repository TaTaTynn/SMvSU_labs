using System.Diagnostics;
using System.Text;

namespace TCPClient
{
    public partial class ClientForm : Form
    {
        private CommunicationClient Client;
        public ClientForm()
        {
            InitializeComponent();
            Client = new CommunicationClient();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                //�������� ����������� � �������� �� ��������� ����������, ���� 22222
                int ClientGUID = Client.Connect("localhost", 22222, 33333);
                if (ClientGUID != -1)
                {
                    Trace.TraceInformation(@"���������� ������ �������. ClientGuid = {0}", ClientGUID);
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
                    MessageBox.Show(@"�� ������� �����������", @"������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"�� ������� �����������", @"������", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                //���������� ������, ������� �� ����� - 5 ������
                int nBytes = Client.SendS�nc(request, out reply, 5);
                //���� ����� �������
                if ((nBytes > 0) && (reply != null))
                {
                    string strReply = Encoding.UTF8.GetString(reply);
                    MessageBox.Show(strReply, @"����� �� �������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(@"�� ������� �������� ����� �� �������!", @"������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void buttonSendMsg_Click(object sender, EventArgs e)
        {
            string text = "<" + textBoxGUID.Text + ">" + textBoxMsg.Text;
            byte[] request = Encoding.UTF8.GetBytes(text);
            byte[] reply = null;
            if (Client != null)
            {
                //���������� ������, ������� �� ����� - 5 ������
                int nBytes = Client.SendS�nc(request, out reply, 5);
                //���� ����� �������
                if ((nBytes > 0) && (reply != null))
                {
                    string strReply = Encoding.UTF8.GetString(reply);
                    MessageBox.Show(strReply, @"����� �� �������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(@"�� ������� �������� ����� �� �������!", @"������", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (Client.AsyncMsg[0] != '<')
            {
                labelTime.Invoke(new Action(() => labelTime.ForeColor = Color.Red));
                labelTime.Invoke(new Action(() => labelTime.Text = Client.AsyncMsg));
                Thread.Sleep(100);
                labelTime.Invoke(new Action(() => labelTime.ForeColor = Color.Black));
            }
            else
            {
                string[] GaM = Client.AsyncMsg.Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                //MessageBox.Show(GaM[1], string.Join(" ","��������� �� �������", GaM[0]), MessageBoxButtons.OK, MessageBoxIcon.Information);
                labelGUIDfrom.Invoke(new Action(() => labelGUIDfrom.Text = GaM[0]));
                textBoxMessage.Invoke(new Action(() => textBoxMessage.Text = GaM[1]));
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.Disconnect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBoxMessage.Text = "";
            labelGUIDfrom.Text = "--";
        }
    }
}