using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Networking
{
    public class Client
    {
        public async void StartClient()
        {
            await ConnectToServer();
        }
            
        async Task ConnectToServer()
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                IPAddress serverIpAddress = IPAddress.Parse("127.0.0.1");
                int serverPort = 13000;

                await tcpClient.ConnectAsync(serverIpAddress, serverPort);
                Debug.Log("Connected to server!");

                // Send message to the server
                await SendMessage(tcpClient, "Hello from Unity client!");

                // Receive response from the server
                string response = await ReceiveMessage(tcpClient);
                Debug.Log($"Received from server: {response}");

                tcpClient.Close();
            }
            catch (Exception e)
            {
                Debug.Log($"Exception: {e}");
            }
        }

        async Task SendMessage(TcpClient tcpClient, string message)
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
            Debug.Log($"Sent to server: {message}");
        }

        async Task<string> ReceiveMessage(TcpClient tcpClient)
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            return receivedMessage;
        }
    }
}