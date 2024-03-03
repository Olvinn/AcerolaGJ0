using UnityEngine;

namespace Triggers
{
    public abstract class TriggerBase : MonoBehaviour
    {
        protected virtual void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        public virtual void Trigger() { }
    }
}
