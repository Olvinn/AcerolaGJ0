using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InputController : Singleton<InputController>
    {
        public Vector2 move;
        public Action onInteract;

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnInteract(InputValue value)
        {
            onInteract?.Invoke();
        }
    }
}
