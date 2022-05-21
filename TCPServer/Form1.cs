namespace TCPServer
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        private CommunicationServer Server;

        public Form1()
        {
            InitializeComponent();
            Server = new CommunicationServer();
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            if (Server.StartServer(22222, 33333, 10000) == false)
            {
                MessageBox.Show(@"Не удалось стартовать сервер", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Server.OnServerExitCallback = OnServerExit;
                Server.OnRequestRecievedCallback = OnRequestReceived;
                radioOnOff.Checked = true;
                radioOnOff.Text = "ON";
            }
        }

        private void buttonStopServer_Click(object sender, EventArgs e)
        {
            Server.StopServer();
            radioOnOff.Checked = false;
            radioOnOff.Text = "OFF";
        }

        private void OnServerExit()
        {
            if (Server.ServerState)
            {
                radioOnOff.Invoke(new Action(() => radioOnOff.Checked = false));
                radioOnOff.Invoke(new Action(() => radioOnOff.Text = "OFF"));
            }
        }

        private bool OnRequestReceived(int ClientGUID, byte[] Request, out byte[] Reply)
        {
            Reply = new byte[Request.Length];
            Array.Copy(Request, Reply, Request.Length);
            return true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.StopServer();
        }
    }
}