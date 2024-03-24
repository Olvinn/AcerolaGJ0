using UnityEngine;

namespace Controllers
{
    public class AimController : Singleton<AimController>
    {
        public Vector3 worldAimPos, screenAimPos;
        private bool _isAiming;
        
        void Start()
        {
            InputController.instance.aim += Aim;
        }

        void Update()
        {
            if (_isAiming)
            {
                if (InputController.instance.isJoystick)
                {
                    Vector3 rot = new Vector3(InputController.instance.lookDirection.x, 0,
                        InputController.instance.lookDirection.y);
                    Vector2 center = new Vector2(Screen.width * .5f, Screen.height * .5f);
                    screenAimPos = center + InputController.instance.lookDirection * (Screen.height * .4f);
                    Rycast();
                    CameraController.instance.SetOffset(rot);
                }
                else
                {
                    screenAimPos = InputController.instance.cursorPosistion;
                    Rycast();
                    Vector3 rot = new Vector3(InputController.instance.lookDirection.x, 0,
                        InputController.instance.lookDirection.y);
                    CameraController.instance.SetOffset(rot);
                }
                UIController.instance.UpdateAim(true, screenAimPos);
            }
            else
            {
                CameraController.instance.SetOffset(Vector3.zero);
                UIController.instance.UpdateAim(false, screenAimPos);
            }
        }

        void Aim(bool value)
        {
            _isAiming = value;
        }

        void Rycast()
        {
            var ray = CameraController.instance.camera.ScreenPointToRay(screenAimPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                worldAimPos = hit.point;
            else
                worldAimPos = GameController.instance.GetPlayerPos();
        }
    }
}
