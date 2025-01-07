using Commands;
using UnityEngine;

namespace Controllers
{
    public class AimController : Singleton<AimController>
    {
        public Vector3 worldAimPos, screenAimPos;
        private bool _isAiming;
        
        void Start()
        {
            InputController.Instance.aim += Aim;
        }

        void Update()
        {
            if (_isAiming)
            {
                if (InputController.Instance.isJoystick)
                {
                    Vector3 rot = new Vector3(InputController.Instance.lookDirection.x, 0,
                        InputController.Instance.lookDirection.y);
                    Vector2 center = new Vector2(Screen.width * .5f, Screen.height * .5f);
                    screenAimPos = center + InputController.Instance.lookDirection * (Screen.height * .4f);
                    Rycast();
                    CameraController.Instance.SetOffset(rot);
                }
                else
                {
                    screenAimPos = InputController.Instance.cursorPosistion;
                    Rycast();
                    Vector3 rot = new Vector3(InputController.Instance.lookDirection.x, 0,
                        InputController.Instance.lookDirection.y);
                    CameraController.Instance.SetOffset(rot);
                }
                CommandBus.Instance.Handle(new UpdateAim() { Show = true, Pos = screenAimPos });
            }
            else
            {
                CameraController.Instance.SetOffset(Vector3.zero);
                CommandBus.Instance.Handle(new UpdateAim() { Show = false, Pos = screenAimPos });
            }
        }

        void Aim(bool value)
        {
            _isAiming = value;
        }

        void Rycast()
        {
            var ray = CameraController.Instance.camera.ScreenPointToRay(screenAimPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                worldAimPos = hit.point;
            else
                worldAimPos = GameController.Instance.GetPlayerPos() + Vector3.up;
        }
    }
}
