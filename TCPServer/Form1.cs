namespace TCPServer
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        private CommunicationServer Server;
        private CommunicationServerClient Client;

        public Form1()
        {
            InitializeComponent();
            Server = new CommunicationServer();
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            if (Server.StartServer(22222) == false)
            {
                MessageBox.Show(@"�� ������� ���������� ������", @"������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Server.OnServerExitCallback = OnServerExit;
                Server.OnRequestRecievedCallback = OnRequestReceived;
                Client = new CommunicationServerClient();
                radioOnOff.Checked = true;
                radioOnOff.Text = "ON";
            }
        }

        private void buttonStopServer_Click(object sender, EventArgs e)
        {
            Server.StopServer();
            //radioOnOff.Checked = false;
            //radioOnOff.Text = "OFF";
        }

        private void OnServerExit()
        {
            this.radioOnOff.Invoke(new Action(() => this.radioOnOff.Checked = false));
            this.radioOnOff.Invoke(new Action(() => this.radioOnOff.Text = "OFF"));
        }

        private bool OnRequestReceived(int ClientGUID, byte[] Request, out byte[] Reply)
        {
            Reply = new byte[Request.Length];
            Array.Copy(Request, Reply, Request.Length);
            return true;
        }
    }
}