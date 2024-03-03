using UnityEngine;

namespace Triggers
{
    [RequireComponent(typeof(Collider))]
    public sealed class ExposedTrigger : TriggerBase
    {
        public string hintText;
        [SerializeField] private Switcher _switcher;
        
        private void OnTriggerEnter(Collider other)
        {
            Unit unit;
            if (other.gameObject.TryGetComponent(out unit))
                unit.TriggerInter(this);
        }

        private void OnTriggerExit(Collider other)
        {
            Unit unit;
            if (other.gameObject.TryGetComponent(out unit))
                unit.TriggerExit(this);
        }

        public override void Trigger()
        {
            base.Trigger();
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
