using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Networking
{
    public class Server
    {
        TcpListener _server = null;
        TcpClient _client = null;
        NetworkStream _stream = null;
        Thread _thread;

        public void Start()
        {
            _thread = new Thread(new ThreadStart(SetupServer));
            _thread.Start();
        }

        private void SetupServer()
        {
            try
            {
                IPAddress serverIpAddress = IPAddress.Parse("127.0.0.1");
                int serverPort = 13000;
                _server = new TcpListener(serverIpAddress, serverPort);
                _server.Start();

                byte[] buffer = new byte[1024];
                string data = null;

                while (true)
                {
                    Debug.Log("Waiting for connection...");
                    _client = _server.AcceptTcpClient();
                    Debug.Log("Connected!");

                    data = null;
                    _stream = _client.GetStream();

                    int i;

                    while ((i = _stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        data = Encoding.UTF8.GetString(buffer, 0, i);
                        Debug.Log("Received: " + data);

                        string response = "Server response: " + data.ToString();
                        SendMessage(message: response);
                    }
                    _client.Close();
                }
            }
            catch (SocketException e)
            {
                Debug.Log("SocketException: " + e);
            }
            finally
            {
                _server.Stop();
            }
        }

        public void Stop()
        {
            _stream.Close();
            _client.Close();
            _server.Stop();
            _thread.Abort();
        }

        public void SendMessage(string message)
        {
            if (_stream == null)
                return;
            
            byte[] msg = Encoding.UTF8.GetBytes(message);
            _stream.Write(msg, 0, msg.Length);
            Debug.Log("Sent: " + message);
        }
    }
}
