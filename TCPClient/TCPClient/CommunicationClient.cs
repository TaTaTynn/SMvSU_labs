using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace TCPClient
{
    public delegate void ClientDisconnectedDelegate();
    public delegate void TimeReceivedDelegate();

    public class CommunicationClient
    {
        private Socket SyncSocket;
        private Socket AsyncSocket;
        private string ServerName;
        private int SyncPort;
        private int AsyncPort;
        private Thread AsyncSocketThread;
        private string sTime;
        private int ClientGUID;
        private Mutex SendSyncMutex;
        public ClientDisconnectedDelegate OnClientDisconnected;
        public TimeReceivedDelegate AsyncTimeReceived;

        public CommunicationClient()
        {
            OnClientDisconnected = null;
        }

        public int Connect(string HostName, int Port)
        {
            ServerName = HostName;
            SyncPort = Port;
            //Соединяем синхронный канал
            try
            {
                SyncSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не получилось создать синхронный сокет");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            if (SyncSocket == null)
            {
                Trace.TraceError("Не удалось создать SyncSocket");
                return -1;
            }
            //Пытаемся сконнектиться
            try
            {
                SyncSocket.Connect(HostName, Port);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не удалось выполнить Connect() для SyncSocket");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            if (SyncSocket.Connected == false)
            {
                Trace.TraceError("Не удалось выполнить Connect() для SyncSocket");
                return -1;
            }
            //Получаем идентификатор клиента для этого соединения
            byte[] bClientGUID = new byte[4];
            int BytesReceived = 0;
            try
            {
                BytesReceived = SyncSocket.Receive(bClientGUID, 4, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не удалось получить ClientGUID");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            if (BytesReceived != 4)
            {
                Trace.TraceError("Не удалось получить ClientGUID");
                return -1;
            }
            ClientGUID = BitConverter.ToInt32(bClientGUID, 0);
            //Подключение отвергнуто
            if (ClientGUID == -1)
            {
                Trace.TraceInformation("Подключение отвергнуто сервером");
                return ClientGUID;
            }
            ////////////////////////////////////////////////////////////////////////
            byte[] bAsyncsPort = new byte[5];
            try
            {
                BytesReceived = SyncSocket.Receive(bAsyncsPort, 5, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не удалось получить номер асинхронного порта");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            int AsyncPortNo = BitConverter.ToInt32(bAsyncsPort, 0);
            ////////////////////////////////////////////////////////////////////////
            //Ожидаем подтверждения, что сервер готов
            byte[] bNotification = new byte[1024];
            try
            {
                BytesReceived = SyncSocket.Receive(bNotification, 1024, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не удалось получить подтверждение готовности сервера");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            this.ConnectAsync(HostName, AsyncPortNo);
            //Создаем мьютекс для синхронного канала
            SendSyncMutex = new Mutex();
            return ClientGUID;
        }

        public int ConnectAsync(string HostName, int Port)
        {
            ServerName = HostName;
            AsyncPort = Port;
            //Соединяем асинхронный канал
            try
            {
                AsyncSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не получилось создать асинхронный сокет");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            if (AsyncSocket == null)
            {
                Trace.TraceError("Не удалось создать AsyncSocket");
                return -1;
            }
            //Пытаемся сконнектиться
            try
            {
                AsyncSocket.Connect(HostName, Port);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не удалось выполнить Connect() для AsyncSocket");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            if (AsyncSocket.Connected == false)
            {
                Trace.TraceError("Не удалось выполнить Connect() для AsyncSocket");
                return -1;
            }
            //Получаем идентификатор клиента для этого соединения
            byte[] bClientGUID = new byte[4];
            int BytesReceived = 0;
            try
            {
                BytesReceived = AsyncSocket.Receive(bClientGUID, 4, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не удалось получить ClientGUID");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            if (BytesReceived != 4)
            {
                Trace.TraceError("Не удалось получить ClientGUID");
                return -1;
            }
            ClientGUID = BitConverter.ToInt32(bClientGUID, 0);
            //Подключение отвергнуто
            if (ClientGUID == -1)
            {
                Trace.TraceInformation("Подключение отвергнуто сервером");
                return ClientGUID;
            }
            ////////////////////////////////////////////////////////////////////////
            byte[] bAsyncsPort = new byte[5];
            try
            {
                BytesReceived = AsyncSocket.Receive(bAsyncsPort, 5, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не удалось получить номер асинхронного порта (который уже не нужен, но все равно отправляется)");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            ////////////////////////////////////////////////////////////////////////
            //Ожидаем подтверждения, что сервер готов
            byte[] bNotification = new byte[1024];
            try
            {
                BytesReceived = AsyncSocket.Receive(bNotification, 1024, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не удалось получить подтверждение готовности сервера");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            //Запускаем поток, ожидающий от сервера время
            if (AsyncSocketThread == null)
            {
                AsyncSocketThread = new Thread(new ThreadStart(AsyncSocketThreadProc));
                AsyncSocketThread.Start();
            }
            return 0;
        }

        private void AsyncSocketThreadProc()
        {                                                                               //////////////////////////////////////////////
            byte[] RecieveBuffer = new byte[8192];
            while (true)
            {
                int BytesReceived = 0;
                //Ждем
                try
                {
                    BytesReceived = AsyncSocket.Receive(RecieveBuffer, 8192, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(@"Ошибка при получении данных по асинхронному каналу");
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
                if ((BytesReceived > 0) && (AsyncTimeReceived != null))
                {
                    byte[] bTime = new byte[BytesReceived];
                    Array.Copy(RecieveBuffer, bTime, BytesReceived);
                    sTime = Encoding.UTF8.GetString(bTime);
                    AsyncTimeReceived();
                }
            }
        }

        public string getTime()
        {
            return sTime;
        }

        public void Disconnect()
        {
            if ((SyncSocket != null) && (SyncSocket.Connected == true))
            {
                SyncSocket.Close();
            }
            if ((AsyncSocket != null) && (AsyncSocket.Connected == true))
            {
                AsyncSocket.Close();
            }
        }

        private void OnClientExit()
        {
            if (OnClientDisconnected != null)
                OnClientDisconnected();
            Trace.TraceInformation(@"Дисконнект");
            Disconnect();
        }

        public int SendSуnc(byte[] Request, out byte[] Response, int Timeout)
        {
            //по дефолту возвращаем null вместо ответа
            Response = null;
            byte[] ReceiveBuffer = new byte[8192];
            //ожидаем освобождения мьютекса, чтобы не попасть в ситуацию одновременной отправки двух запросов
            if (SendSyncMutex.WaitOne(Timeout) == false)
            {
                Trace.TraceError(@"Ожидание мьютекса вышло по таймауту");
                return -1;
            }
            //Посылаем запрос на сервер
            int BytesSent = 0;
            try
            {
                SyncSocket.SendTimeout = Timeout;
                BytesSent = SyncSocket.Send(Request);
            }
            catch (Exception ex)
            {
                Trace.TraceError(@"Не получилось отправить синхронный запрос");
                Trace.TraceError(ex.ToString());
                return -1;
            }
            //Если были отправлены все байты запроса
            if (BytesSent == Request.Length)
            {
                //Хотелось бы получить ответ
                int BytesReceived = 0;
                try
                {
                    SyncSocket.ReceiveTimeout = Timeout;
                    BytesReceived = SyncSocket.Receive(ReceiveBuffer, 8192, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(@"Не получилось получить синхронный ответ");
                    Trace.TraceError(ex.ToString());
                    return -1;
                }
                //Если количество полученных байт > 0
                if (BytesReceived > 0)
                {
                    Response = new byte[BytesReceived];
                    Array.Copy(ReceiveBuffer, Response, BytesReceived);
                }
            }
            else
            {
                BytesSent = -1;
                Trace.TraceError(@"Количество отправленных байт не совпадает с длиной буфера");
            }
            //Освобождаем объект синхронизации
            SendSyncMutex.ReleaseMutex();
            return BytesSent;
        }
    }
}
