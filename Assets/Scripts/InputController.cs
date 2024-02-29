using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Singleton<InputController>
{
    public Vector2 move;
    
    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }
}
