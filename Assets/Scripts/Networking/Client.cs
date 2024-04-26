using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Networking
{
    public class Client : INetworkConnector
    {
        public string serverIP = "127.0.0.1"; // Set this to your server's IP address.
        public int serverPort = 13000;             // Set this to your server's port.
        public bool isServer => false;
        public event Action<string> onReceiveMessage;

        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _clientReceiveThread;

        public void Start()
        {
            ConnectToServer();
        }

        void ConnectToServer()
        {
            try
            {
                _client = new TcpClient(serverIP, serverPort);
                _stream = _client.GetStream();

                _clientReceiveThread = new Thread(new ThreadStart(ListenForData));
                _clientReceiveThread.IsBackground = true;
                _clientReceiveThread.Start();
            }
            catch (SocketException e)
            {
                Debug.LogError("SocketException: " + e.ToString());
            }
        }

        private void ListenForData()
        {
            try
            {
                byte[] bytes = new byte[1024];
                while (true)
                {
                    if (_stream.DataAvailable)
                    {
                        int length;
                        // Read incoming stream into byte array.
                        while ((length = _stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);
                            string serverMessage = Encoding.UTF8.GetString(incomingData);
                            onReceiveMessage?.Invoke(serverMessage);
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
            catch (Exception exception)
            {
                Debug.Log("Exception:" + exception);
            }
        }

        public void SendMessage(string message)
        {
            if (_client == null || !_client.Connected)
            {
                return;
            }

            byte[] data = Encoding.UTF8.GetBytes(message);
            _stream.Write(data, 0, data.Length);
        }

        private void Stop()
        {
            if (_stream != null)
                _stream.Close();
            if (_client != null)
                _client.Close();
            if (_clientReceiveThread != null)
                _clientReceiveThread.Abort();
        }

        public void Dispose()
        {
            Stop();
            _client?.Dispose();
            _stream?.Dispose();
            onReceiveMessage = null;
        }
    }
}