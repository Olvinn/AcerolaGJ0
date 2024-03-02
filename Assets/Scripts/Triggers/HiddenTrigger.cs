using UnityEngine;

namespace Triggers
{
    [RequireComponent(typeof(Collider))]
    public class HiddenTrigger : TriggerBase
    {
        private void OnTriggerEnter(Collider other)
        {
            Trigger();
        }
    }
}