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

        public abstract StageType GetStageType();

        protected void Update()
        {
            if (!_isOpen)
                return;
            OnUpdate();
        }

        protected void LateUpdate()
        {
            if (!_isOpen)
                return;
            OnLateUpdate();
        }

        protected virtual void OnUpdate() { }

        protected virtual void OnLateUpdate() { }

        protected abstract void OnOpen();

        protected abstract void OnClose();
    }
}
