using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace TCPServer
{

    internal class CommunicationServerClient
    {
        private Socket SyncSocket;
        private Thread SyncSocketThread;
        private int _ClientGUID = 0;
        public int ClientGuid
        {
            get { return _ClientGUID; }
        }
        public RequestReceivedDelegate OnSyncRequestReceived;
        public ClientDisconnectedDelegate OnClientDisconnected;
        public CommunicationServerClient()
        {
            _ClientGUID = -1;
            OnSyncRequestReceived = null;
            OnClientDisconnected = null;
        }

        public void Disconnect()
        {
            _ClientGUID = -1;
            OnSyncRequestReceived = null;
            OnClientDisconnected = null;
            if (SyncSocket != null)
            {
                SyncSocket.Close();
            }
            if (SyncSocketThread != null)
            {
                SyncSocketThread.Abort();
            }
        }

        private void OnClientExit()
        {
            if (OnClientDisconnected != null)
                OnClientDisconnected(_ClientGUID);
            Disconnect();
        }

        public int FinishConnect(Socket ClientSocket, int ConnectionGUID)
        {
            SyncSocket = ClientSocket;
            _ClientGUID = ConnectionGUID;
            //Отправляем идентификатор соединения
            try
            {
                int BytesSend = SyncSocket.Send(BitConverter.GetBytes(_ClientGUID));
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не получилось отправить ClientGUID");
                Trace.TraceError(ex.ToString());
                OnClientExit();
                return -1;
            }
            if (_ClientGUID == -1)
                return -1;
            //Создаем поток синхронного канала
            SyncSocketThread = new Thread(new ThreadStart(SyncSocketThreadProc));
            SyncSocketThread.Start();
            return 0;
        }

        private void SyncSocketThreadProc()
        {
            //Отправляем нотификацию, что сервер готов
            try
            {
                int BytesSend = SyncSocket.Send(BitConverter.GetBytes(_ClientGUID));
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не получилось отправить нотификацию, что сервер готов");
                Trace.TraceError(ex.ToString());
                OnClientExit();
                return;
            }
            byte[] RecieveBuffer = new byte[8192];
            byte[] SendBuffer = new byte[8192];
            while (true)
            {
                int BytesReceived = 0;
                //Ждем запроса
                try
                {
                    BytesReceived = SyncSocket.Receive(RecieveBuffer, 8192, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(@"Ошибка при получении данных по синхронному каналу");
                    Trace.TraceError(ex.ToString());
                    OnClientExit();
                    return;
                }
                //Это, как ни странно, свидетельствует о дисконнекте
                if (BytesReceived <= 0)
                {
                    OnClientExit();
                    return;
                }
                if ((BytesReceived > 0) && (OnSyncRequestReceived != null))
                {
                    byte[] Reply = null;
                    byte[] Request = new byte[BytesReceived];
                    Array.Copy(RecieveBuffer, Request, BytesReceived);
                    bool bReply = OnSyncRequestReceived(_ClientGUID, Request, out Reply);
                    if ((bReply == true) && (Reply != null))
                    {
                        //Отправляем ответ
                        try
                        {
                            int BytesSend = SyncSocket.Send(Reply);
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(@"Не получилось отправить ответ на синхронный запрос");
                            Trace.TraceError(ex.ToString());
                            OnClientExit();
                            return;
                        }
                    }
                }
            }
        }


    }
}
