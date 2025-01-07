using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InputController : Singleton<InputController>
    {
        public Vector2 move;
        public Vector2 lookDirection;
        public Vector2 cursorPosistion;
        public Action<bool> aim, onShoot;
        public Action onMainInteract, onSecondaryInteract, onReload, onConsole;
        public bool isJoystick;

        public void ShakeGamepad(float magnitude, float time)
        {
            if (Gamepad.current != null)
                Gamepad.current.SetMotorSpeeds(magnitude, time);
        }
        
        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            var pos = value.Get<Vector2>();
            cursorPosistion = -pos;
            pos += new Vector2(Screen.width * .5f, Screen.height * .5f);
            lookDirection = -pos;
            lookDirection.x /= Screen.width * .5f;
            lookDirection.y /= Screen.height * .5f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isJoystick = false;
        }
        
        public void OnLookGamepad(InputValue value)
        {
            lookDirection = value.Get<Vector2>();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
            aim.Invoke(lookDirection.sqrMagnitude != 0);
            isJoystick = true;
        }

        public void OnInteract(InputValue value)
        {
            onMainInteract?.Invoke();
        }
        
        public void OnSecondaryInteract(InputValue value)
        {
            onSecondaryInteract?.Invoke();
        }

        public void OnFire(InputValue value)
        {
            onShoot?.Invoke(value.isPressed);
        }

        public void OnAim(InputValue value)
        {
            aim.Invoke(value.isPressed);
        }

        public void OnReload(InputValue value)
        {
            onReload?.Invoke();
        }

        public void OnConsole(InputValue value)
        {
            onConsole?.Invoke();
        }
    }
}
