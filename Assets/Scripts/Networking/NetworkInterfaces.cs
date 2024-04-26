using System;

namespace Networking
{
    public interface INetworkConnector : IDisposable
    {
        public bool isServer { get; }
        public event Action<string> onReceiveMessage;
        public void Start();
        public void SendMessage(string message);
    }
}
