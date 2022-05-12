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
        private int ListenSocketPort;
        private int AsyncSocketPort;
        private Socket IncomingConnectionSocket;
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
            AsyncSocketPort = 33333;
            timerCount = new System.Timers.Timer(10000);
            timerCount.Elapsed += SendTimeToClients;
        }

        public bool StartServer(int PortNo)
        {
            ListenSocketPort = PortNo;
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
            return true;
        }

        public void StopServer()
        {
            if (IncomingConnectionThread != null)
            {
                IncomingConnectionThread.Interrupt();
            }
            if (IncomingConnectionSocket != null)
            {
                IncomingConnectionSocket.Close();
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
            if (OnServerExitCallback != null)
            {
                OnServerExitCallback();
            }
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
            while (true)
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
                // На случай, если таймер упал
                if (timerCount != null && !timerCount.Enabled)
                {
                    timerCount.Enabled = true;
                }
            }
        }

        private void IncomingAsyncConnectionThreadProc()
        {
            Socket AsyncIncomingConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
            while (true)
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
                    OnClientConnected(newConnectionSocket);
                    if (!timerCount.Enabled)
                        timerCount.Start();
                }
            }
        }

        private void SendTimeToClients(Object source, ElapsedEventArgs e)
        {
            byte[] SendBuffer = new byte[8192];
            byte[] Reply = null;
            byte[] time = Encoding.UTF8.GetBytes(e.SignalTime.ToString());
            //Array.Copy(time, Request, BytesReceived);
            for (int i = 0; i < CommunicationClients.Length; i++)
            {
                if (CommunicationClients[i]!= null && CommunicationClients[i].ClientGuid == -1) continue;
                bool bReply = CommunicationClients[i].OnSyncRequestReceived(CommunicationClients[i].ClientGuid, time, out Reply);
                if ((bReply == true) && (Reply != null))
                {
                    if (!CommunicationClients[i].SendAsync(Reply))
                        Trace.TraceError("Не удалось передать время клиенту");
                }
            }
            
        }

        private void OnClientConnected(Socket ClientSocket)
        {
            int ClientGUID = GetGUIDForClient();
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
                Trace.TraceInformation(@"Клиент приконнектился. ClientGIUD = {0}. Всего клиентов соединено {1}.",
                ClientGUID, @"n/a");
            }
            else
            {
                client.Disconnect();
                Trace.TraceInformation(@"Коннект не удался. ClientGIUD = {0}. Всего клиентов соединено {1}.",
                ClientGUID, @"n/a");
            }
        }

        private void OnClientDisconnected(int ClientGUID)
        {
            if (OnClientDisconnectedCallback != null)
                OnClientDisconnectedCallback(ClientGUID);
            CommunicationServerClient client = CommunicationClients[ClientGUID];
            if (client != null)
            {
                Trace.TraceInformation(@"Клиент отконнектился. ClientGIUD = {0}. Всего клиентов соединено {1}.",
                ClientGUID, @"xz");
                client.Disconnect();
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
                    if (CommunicationClients[i].ClientGuid==i || CommunicationClients[i].ClientGuid == -1)
                        return i;
                }
            }
            return -1;
        }
    }
}
