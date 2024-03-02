using Controllers;
using UnityEngine;

namespace Triggers
{
    [RequireComponent(typeof(Collider))]
    public class ExposedTrigger : TriggerBase
    {
        [SerializeField] private Switcher _switcher;
        [SerializeField] private string _hintText;
        private int _cachedHintKey;
        
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
            _cachedHintKey = UIController.instance.ShowHint(GameConfigsContainer.instance.config.useColor, _hintText, transform.position + Vector3.up);
        }

        private void Unsubscribe()
        {
            InputController.instance.onInteract -= Trigger;
            UIController.instance.HideHint(_cachedHintKey);
        }

        public override void Trigger()
        {
            base.Trigger();
            Unsubscribe();
            if (_switcher)
                _switcher.Switch();
        }

        protected override void Reset()
        {
            base.Reset();
            _switcher = GetComponent<Switcher>();
        }
    }
}
