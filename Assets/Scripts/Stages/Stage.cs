using System;
using UnityEngine;

namespace Stages
{
    public abstract class Stage : MonoBehaviour
    {
        private bool _isOpen;
        
        public void Open()
        {
            _isOpen = true;
            OnOpen();
        }

        public void Close()
        {
            _isOpen = false;
            OnClose();
        }

        public void Update()
        {
            if (!_isOpen)
                return;
            OnUpdate();
        }

        public void LateUpdate()
        {
            if (!_isOpen)
                return;
            OnLateUpdate();
        }

        protected virtual void OnLateUpdate()
        {
            
        }

        protected virtual void OnOpen()
        {
            
        }

        protected virtual void OnClose()
        {
            
        }

        protected virtual void OnUpdate()
        {
            
        }
    }
}
