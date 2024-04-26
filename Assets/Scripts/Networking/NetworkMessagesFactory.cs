using UnityEngine;

namespace Networking
{
    public static class NetworkMessagesFactory
    {
        public static NetworkMessage CreateMessageData(string message)
        {
            var wrapper = JsonUtility.FromJson<MessageDataWrapper>(message);
            switch (wrapper.type)
            {
                case MessageDataType.ChatMessage:
                    return JsonUtility.FromJson<ChatMessage>(message);
            }

            return null;
        }
    }

    public class MessageDataWrapper
    {
        public MessageDataType type;
    }
}
