namespace TCPServer
{
    public partial class Form1 : Form
    {
        private CommunicationServer Server;

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
            }
        }

        private void buttonStopServer_Click(object sender, EventArgs e)
        {
            Server.StopServer();
        }

        private void OnServerExit()
        {
        }

        private bool OnRequestReceived(int ClientGUID, byte[] Request, out byte[] Reply)
        {
            Reply = new byte[Request.Length];
            Array.Copy(Request, Reply, Request.Length);
            return true;
        }
    }
}