using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Transform _camera;
        
        void LateUpdate()
        {
            var pos = GameController.instance.GetPlayerPos();
            _cameraHolder.position = Vector3.Lerp(_cameraHolder.position, pos, 
                Time.unscaledDeltaTime * GameConfigsContainer.instance.config.cameraLerpSpeed);
            _camera.LookAt(pos);
        }

        private void Reset()
        {
            _camera = Camera.main.transform;
            _cameraHolder = _camera.parent;
        }

        public void Shake(float magnitude, float time)
        {
            StartCoroutine(Shaking(magnitude, time));
        }

        IEnumerator Shaking(float magnitude, float time)
        {
            float timer = time;
            while (timer > 0)
            {
                var power = magnitude * (timer / time);
                _cameraHolder.position += new Vector3(Random.Range(-power, power), 0,
                    Random.Range(-power, power));
                timer -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
