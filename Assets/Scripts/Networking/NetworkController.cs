using System;
using UnityEngine;

namespace Networking
{
    public class NetworkController : Singleton<NetworkController>
    {
        public string networkName;
        public Action<NetworkMessage> onReceiveMessage;
        
        private INetworkConnector _connector;

        public void SendMessage<T>(T data) where T : NetworkMessage 
        {
            _connector?.SendMessage(JsonUtility.ToJson(data));
        }

        public void HostGame()
        {
            _connector?.Dispose();
            _connector = new Server();
            _connector.Start();
            _connector.onReceiveMessage += OnReceiveMessage;
        }

        public void ConnectToHost()
        {
            _connector?.Dispose();
            _connector = new Client();
            _connector.Start();
            _connector.onReceiveMessage += OnReceiveMessage;
        }

        public void Stop()
        {
            _connector?.Dispose();
            _connector = null;
        }

        private void OnReceiveMessage(string message)
        {
            var messageObject = NetworkMessagesFactory.CreateMessageData(message);
            if (messageObject != null)
                onReceiveMessage?.Invoke(messageObject);
        }
    }
}
