using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace TCPServer
{
    public delegate void ServerExitDelegate();
    public delegate bool RequestReceivedDelegate(int ClientGUID, byte[] Request, out byte[] Reply);
    public delegate void ClientDisconnectedDelegate(int ClientGUID);
    internal class CommunicationServer
    {
        public bool ServerState
        {
            get { return _ServerState; }
        }
        private bool _ServerState;
        private int ListenSocketPort;
        private int AsyncSocketPort;
        private static int currentGUID;
        private Socket IncomingConnectionSocket;
        private Socket AsyncIncomingConnectionSocket;
        private Thread IncomingConnectionThread;
        private Thread IncomingAsyncConnectionThread;
        private CommunicationServerClient[] CommunicationClients;
        private static System.Timers.Timer timerCount;


        public ServerExitDelegate OnServerExitCallback;
        public RequestReceivedDelegate OnRequestRecievedCallback;
        public ClientDisconnectedDelegate OnClientDisconnectedCallback;
        public CommunicationServer()
        {
            OnServerExitCallback = null;
            OnRequestRecievedCallback = null;
            OnClientDisconnectedCallback = null;
            currentGUID = -1;
            _ServerState = false;
        }

        public bool StartServer(int PortNo, int AsPortNo, int TimerLimit)
        {
            ListenSocketPort = PortNo;
            AsyncSocketPort = AsPortNo;
            CommunicationClients = new CommunicationServerClient[10];
            if (CommunicationClients == null)
                return false;
            for (int i = 0; i < CommunicationClients.Length; i++)
            {
                CommunicationClients[i] = new CommunicationServerClient();
                if (CommunicationClients[i] == null)
                {
                    StopServer();
                    return false;
                }
            }
            IncomingConnectionThread = new Thread(new ThreadStart(IncomingConnectionThreadProc));
            IncomingConnectionThread.Start();
            IncomingAsyncConnectionThread = new Thread(new ThreadStart(IncomingAsyncConnectionThreadProc));
            IncomingAsyncConnectionThread.Start();
            timerCount = new System.Timers.Timer(TimerLimit);
            timerCount.Elapsed += SendTimeToClients;
            _ServerState = true;
            return true;
        }

        public void StopServer()
        {
            _ServerState = false;
            /*if (IncomingConnectionThread != null)
            {
                IncomingConnectionThread.Interrupt();
            }
            if (IncomingAsyncConnectionThread != null)
            {
                IncomingAsyncConnectionThread.Interrupt();
            }*/
            if (IncomingConnectionSocket != null)
            {
                IncomingConnectionSocket.Close();
            }
            if (AsyncIncomingConnectionSocket != null)
            {
                AsyncIncomingConnectionSocket.Close();
            }
            if (CommunicationClients != null)
            {
                for (int i = 0; i < CommunicationClients.Length; i++)
                {
                    if (CommunicationClients[i] != null)
                    {
                        CommunicationClients[i].Disconnect();
                        CommunicationClients[i] = null;
                    }
                }
            }
            OnServerExitCallback?.Invoke();
        }

        private void IncomingConnectionThreadProc()
        {
            IncomingConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (IncomingConnectionSocket == null)
            {
                Trace.TraceError("Не удалось создать IncomingConnectionSocket");
                OnServerExit();
                return;
            }
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, ListenSocketPort);
                IncomingConnectionSocket.Bind(ep);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Не удалось сделать Bind() для IncomingConnectionSocket");
                Trace.TraceError(ex.ToString());
                OnServerExit();
                return;
            }
            try
            {
                IncomingConnectionSocket.Listen(5);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Не удалось сделать Listen() для IncomingConnectionSocket");
                Trace.TraceError(ex.ToString());
                OnServerExit();
                return;
            }
            while (_ServerState)
            {
                Socket newConnectionSocket = null;
                try
                {
                    Trace.TraceInformation(@"Ожидаем входящего подключения");
                    newConnectionSocket = IncomingConnectionSocket.Accept();
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Не удалось сделать Accept() для IncomingConnectionSocket");
                    Trace.TraceError(ex.ToString());
                    OnServerExit();
                    return;
                }
                if (newConnectionSocket != null)
                {
                    OnClientConnected(newConnectionSocket);
                }
            }
        }

        private void IncomingAsyncConnectionThreadProc()
        {
            AsyncIncomingConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (AsyncIncomingConnectionSocket == null)
            {
                Trace.TraceError("Не удалось создать AsyncIncomingConnectionSocket");
                OnServerExit();
                return;
            }
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, AsyncSocketPort);
                AsyncIncomingConnectionSocket.Bind(ep);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Не удалось сделать Bind() для AsyncIncomingConnectionSocket");
                Trace.TraceError(ex.ToString());
                OnServerExit();
                return;
            }
            try
            {
                AsyncIncomingConnectionSocket.Listen(5);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Не удалось сделать Listen() для AsyncIncomingConnectionSocket");
                Trace.TraceError(ex.ToString());
                OnServerExit();
                return;
            }
            while (_ServerState)
            {
                Socket newConnectionSocket = null;
                try
                {
                    Trace.TraceInformation(@"Ожидаем входящего асинхронного подключения");
                    newConnectionSocket = AsyncIncomingConnectionSocket.Accept();
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Не удалось сделать Accept() для AsyncIncomingConnectionSocket");
                    Trace.TraceError(ex.ToString());
                    OnServerExit();
                    return;
                }
                if (newConnectionSocket != null)
                {
                    OnClientAsyncConnected(newConnectionSocket, currentGUID);
                    currentGUID = -1;
                    if (!timerCount.Enabled)
                        timerCount.Start();
                }
            }
        }

        private void SendTimeToClients(Object source, ElapsedEventArgs e)
        {
            Trace.TraceInformation(@"Шлем клиентам время");
            byte[] time = Encoding.UTF8.GetBytes(e.SignalTime.ToString());
            for (int i = 0; i < CommunicationClients.Length; i++)
            {
                if (CommunicationClients[i]!= null && CommunicationClients[i].ClientGuid == -1) continue;
                    if (!CommunicationClients[i].SendAsync(time))
                        Trace.TraceError("Не удалось передать время клиенту");
            }
        }

        private void OnClientConnected(Socket ClientSocket)
        {
            currentGUID = GetGUIDForClient();
            if (currentGUID == -1)
            {
                CommunicationClients[0].FinishConnect(ClientSocket, currentGUID);
                CommunicationClients[0].Disconnect();
            }
            //IncomingAsyncConnectionThread = new Thread(new ThreadStart(IncomingAsyncConnectionThreadProc));
            //IncomingAsyncConnectionThread.Start();
            CommunicationServerClient client = CommunicationClients[currentGUID];
            if (client == null)
                return;
            int result = client.FinishConnect(ClientSocket, currentGUID);
            if (result == 0)
            {
                client.OnSyncRequestReceived = OnRequestRecieved;
                client.OnClientDisconnected = OnClientDisconnected;
                Trace.TraceInformation(@"Клиент приконнектился. ClientGIUD = {0}. Всего клиентов соединено {1}.",
                currentGUID, ClientNum());
            }
            else
            {
                client.Disconnect();
                Trace.TraceInformation(@"Коннект не удался. ClientGIUD = {0}. Всего клиентов соединено {1}.",
                currentGUID, ClientNum());
            }
        }
        private void OnClientAsyncConnected(Socket ClientSocket, int ClientGUID)
        {
            //int ClientGUID = TheLastGUID();
            if (ClientGUID == -1)
            {
                CommunicationClients[0].FinishConnect(ClientSocket, ClientGUID);
                CommunicationClients[0].Disconnect();
            }
            CommunicationServerClient client = CommunicationClients[ClientGUID];
            if (client == null)
                return;
            int result = client.FinishConnect(ClientSocket, ClientGUID);
            if (result == 0)
            {
                client.OnSyncRequestReceived = OnRequestRecieved;
                client.OnClientDisconnected = OnClientDisconnected;
                Trace.TraceInformation(@"Клиент приконнектился к асинхронному сокету. ClientGIUD = {0}. Всего клиентов соединено {1}.",
                ClientGUID, ClientNum());
            }
            else
            {
                client.Disconnect();
                Trace.TraceInformation(@"Коннект к асинхронному сокету не удался. ClientGIUD = {0}. Всего клиентов соединено {1}.",
                ClientGUID, ClientNum());
            }
        }
        int ClientNum()
        {
            int num = 0;
            foreach (CommunicationServerClient clientelem in CommunicationClients)
            {
                num = clientelem.ClientGuid > 0 ? num + 1 : num;
            }
            return num;
        }
        private void OnClientDisconnected(int ClientGUID)
        {
            if (OnClientDisconnectedCallback != null)
                OnClientDisconnectedCallback(ClientGUID);
            CommunicationServerClient client = CommunicationClients[ClientGUID];
            if (client != null)
            {
                client.Disconnect();
                Trace.TraceInformation(@"Клиент отконнектился. ClientGIUD = {0}. Всего клиентов соединено {1}.",
                ClientGUID, ClientNum());
            }
        }
        private bool OnRequestRecieved(int ClientGUID, byte[] Request, out byte[] Reply)
        {
            if (OnRequestRecievedCallback != null)
            {
                return OnRequestRecievedCallback(ClientGUID, Request, out Reply);
            }
            Reply = null;
            return false;
        }
        private void OnServerExit()
        {
            timerCount.Stop();
            timerCount.Close();
            _ServerState = false;
            Trace.TraceError(@"OnServerExit() - критическая ошибка");
            if (OnServerExitCallback != null)
                OnServerExitCallback();
        }

        private int GetGUIDForClient()
        {
            for (int i = 1; i < CommunicationClients.Length; i++)
            {
                if (CommunicationClients[i] != null)
                {
                    if (CommunicationClients[i].ClientGuid == -1)
                        return i;
                }
            }
            return -1;
        }
    }
}
