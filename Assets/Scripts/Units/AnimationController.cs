using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Reset()
        {
            _animator = GetComponent<Animator>();
        }

        public void FootStep()
        {
            
        }

        public void SetFloat(string key, float value) => _animator.SetFloat(key, value);
        public void SetBool(string key, bool value) => _animator.SetBool(key, value);
    }
}
