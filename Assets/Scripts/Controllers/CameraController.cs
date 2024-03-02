using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
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
    }
}
