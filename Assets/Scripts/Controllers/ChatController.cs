using System;
using System.Collections.Generic;
using Networking;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class ChatController : Singleton<ChatController>
    {
        [SerializeField] private TextMeshProUGUI _history;
        [SerializeField] private TMP_InputField _input;
        private Queue<string> _toShow;
        private object _locker = new object();

        public Action<ChatMessage> onSendNetworkMessage;

        protected override void Awake()
        {
            base.Awake();
            _history.text = String.Empty;
            _toShow = new Queue<string>();

            var scale = _history.rectTransform.sizeDelta;
            scale.y = 50;
            _history.rectTransform.sizeDelta = scale;
        }

        private void Update()
        {
            lock (_locker)
            {
                if (_toShow.Count == 0)
                    return;
                
                _history.text += _toShow.Dequeue() + "\n";
                _history = Instantiate(_history, _history.transform.parent);
                _history.text = String.Empty;
                _input.transform.SetAsLastSibling();
            }
        }

        public void ReceiveNetworkMessage(NetworkMessage message)
        {
            if (message is ChatMessage chatMessage)
            {
                lock (_locker)
                {
                    _toShow.Enqueue($"{chatMessage.name}: {chatMessage.message}");
                }
            }
        }

        public void SendMessage()
        {
            onSendNetworkMessage?.Invoke(new ChatMessage(_input.text, NetworkController.Instance.networkName));
            _input.text = String.Empty;
        }
    }
}
