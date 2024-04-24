using System;
using System.Collections.Generic;
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

        public Action<string> onSendMessage;

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

        public void ReceiveMessage(string message)
        {
            lock (_locker)
            {
                _toShow.Enqueue(message);
            }
        }

        public void SendMessage()
        {
            onSendMessage?.Invoke(_input.text);
            _input.text = String.Empty;
        }
    }
}
