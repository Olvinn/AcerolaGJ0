using System;
using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        public event Action onReloadComplete;
        [SerializeField] private Animator _animator;

        private void Reset()
        {
            _animator = GetComponent<Animator>();
        }

        public void FootStep()
        {
            
        }

        public void OnReloadComplete()
        {
            onReloadComplete?.Invoke();
        }

        public void SetFloat(string key, float value) => _animator.SetFloat(key, value);
        public void SetBool(string key, bool value) => _animator.SetBool(key, value);
        public void SetTrigger(string key) => _animator.SetTrigger(key);
    }
}
