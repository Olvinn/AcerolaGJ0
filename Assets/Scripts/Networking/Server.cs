using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Networking
{
    public class Server
    {
        public async Task StartServer()
        {
            TcpListener tcp = null;
            try
            {
                int port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                tcp = new TcpListener(localAddr, port);
                tcp.Start();
                Debug.Log($"Server started. Listening for connections...");

                while (true)
                {
                    TcpClient client = await tcp.AcceptTcpClientAsync();
                    Debug.Log($"Client connected!");

                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Exception: {e}");
            }
            finally
            {
                tcp?.Stop();
            }
        }
        
        async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Debug.Log($"Received: {data}");

                    data = data.ToUpper();
                    byte[] response = Encoding.ASCII.GetBytes(data);
                    await stream.WriteAsync(response, 0, response.Length);
                    Debug.Log($"Sent: {data}");
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Exception: {e}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
