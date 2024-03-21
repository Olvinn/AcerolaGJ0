using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class CameraController : Singleton<CameraController>
    {
        public Camera camera => _camera;
        
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Camera _camera;

        private Vector3 _targetOffset, _offset;
        
        void LateUpdate()
        {
            _offset = Vector3.Lerp(_offset, _targetOffset, 
                Time.unscaledDeltaTime * GameConfigsAndSettings.instance.config.cameraAimLerpSpeed);
            var pos = GameController.instance.GetPlayerPos() + _offset;
            _cameraHolder.position = Vector3.MoveTowards(_cameraHolder.position, pos, 
                Time.unscaledDeltaTime * GameConfigsAndSettings.instance.config.cameraLerpSpeed);
            _camera.transform.LookAt(pos);
        }

        private void Reset()
        {
            _camera = Camera.main;
            _cameraHolder = _camera.transform.parent;
        }

        public void Shake(float magnitude, float time)
        {
            if (GameConfigsAndSettings.instance.settings.cameraShaking)
            {
                StartCoroutine(Shaking(magnitude, time));
            }
        }

        public void SetOffset(Vector3 dir)
        {
            if (dir.magnitude > 1f)
                dir.Normalize();
            _targetOffset = dir * GameConfigsAndSettings.instance.config.aimDistance;
        }

        IEnumerator Shaking(float magnitude, float time)
        {
            float timer = time;
            while (timer > 0)
            {
                var power = magnitude * (timer / time);
                InputController.instance.ShakeGamepad(power, power);
                _cameraHolder.position += new Vector3(Random.Range(-power, power), 0,
                    Random.Range(-power, power));
                timer -= Time.deltaTime;
                yield return null;
            }
            InputController.instance.ShakeGamepad(0, 0);
        }
    }
}
