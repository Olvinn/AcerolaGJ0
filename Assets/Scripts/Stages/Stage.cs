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

        public void UpdateStage()
        {
            if (!_isOpen)
                return;
            OnUpdate();
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
