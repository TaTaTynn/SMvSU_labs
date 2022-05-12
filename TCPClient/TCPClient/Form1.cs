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
                //�������� ����������� � �������� �� ��������� ����������, ���� 22222
                int ClientGUID = Client.Connect("localhost", 22222);
                if (ClientGUID != -1)
                {
                    Trace.TraceInformation(@"���������� ������ �������. ClientGuid = {0}", ClientGUID);
                    Client.AsyncTimeReceived += RefreshTime;
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

        private void RefreshTime()
        {
            this.labelTime.Invoke(new Action(() => this.labelTime.ForeColor=Color.Red));
            this.labelTime.Invoke(new Action(() => this.labelTime.Text = Client.getTime()));
            System.Threading.Thread.Sleep(100);
            this.labelTime.Invoke(new Action(() => this.labelTime.ForeColor=Color.Black));
        }
    }
}