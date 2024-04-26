using System;

namespace Networking
{
    public enum MessageDataType
    {
        Unidentified,
        ChatMessage
    }

    public abstract class NetworkMessage { }
        
    [Serializable]
    public class ChatMessage : NetworkMessage
    {
        public MessageDataType type;
        public string name;
        public string message;

        public ChatMessage(string message, string name)
        {
            this.message = message;
            this.name = name;
            type = MessageDataType.ChatMessage;
        }
    }
}
