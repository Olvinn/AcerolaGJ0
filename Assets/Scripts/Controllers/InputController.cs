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

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            var pos = value.Get<Vector2>();
            pos += new Vector2(Screen.width * .5f, Screen.height * .5f);
            lookDirection = -pos;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        public void OnLookGamepad(InputValue value)
        {
            lookDirection = value.Get<Vector2>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnInteract(InputValue value)
        {
            onInteract?.Invoke();
        }
    }
}
