using Controllers;
using Units;
using UnityEngine;

namespace Triggers
{
    [RequireComponent(typeof(Collider))]
    public class HiddenTrigger : TriggerBase
    {
        [SerializeField] protected bool _playerOnly;
        [SerializeField] private Switcher _switcher;
        
        private void OnTriggerEnter(Collider other)
        {
            Unit unit;
            if (other.gameObject.TryGetComponent(out unit))
            {
                if (_playerOnly && GameController.instance.IsPlayer(unit))
                    if (_switcher)
                        _switcher.On();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Unit unit;
            if (other.gameObject.TryGetComponent(out unit))
            {
                if (_playerOnly && GameController.instance.IsPlayer(unit))
                    if (_switcher)
                        _switcher.Off();
            }
        }
        
        public override void Trigger()
        {
            base.Trigger();
            if (_switcher)
                _switcher.Switch();
        }
    }
}