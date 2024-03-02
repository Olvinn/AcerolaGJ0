using Controllers;
using UnityEngine;

namespace Triggers
{
    [RequireComponent(typeof(Collider))]
    public class ExposedTrigger : TriggerBase
    {
        [SerializeField] private GameObject _hint;
        
        private void OnTriggerEnter(Collider other)
        {
            SubscribeOnInput();
        }

        private void OnTriggerExit(Collider other)
        {
            Unsubscribe();
        }

        private void SubscribeOnInput()
        {
            InputController.instance.onInteract += Trigger;
            _hint.gameObject.SetActive(true);
        }

        private void Unsubscribe()
        {
            InputController.instance.onInteract -= Trigger;
            _hint.gameObject.SetActive(false);
        }

        public override void Trigger()
        {
            base.Trigger();
            Unsubscribe();
        }
    }
}
