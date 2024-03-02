using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InputController : Singleton<InputController>
    {
        public Vector2 move;
        public Vector2 lookDirection;
        public Action onInteract;

        private bool _isMouse;

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            var pos = value.Get<Vector2>();
            pos += new Vector2(Screen.width * .5f, Screen.height * .5f);
            lookDirection = -pos;
            _isMouse = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        public void OnLookGamepad(InputValue value)
        {
            lookDirection = value.Get<Vector2>();
            _isMouse = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnInteract(InputValue value)
        {
            onInteract?.Invoke();
        }
    }
}
