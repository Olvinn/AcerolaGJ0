using Controllers;
using UnityEngine;

namespace Triggers
{
    public class TriggerBase : MonoBehaviour
    {
        [SerializeField] private Transform _cattachedPoint;
        public TriggerType type;

        protected virtual void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        public virtual void Trigger()
        {
            switch (type)
            {
                case TriggerType.MoveToSecondFloor:
                {
                    GameController.instance.MoveToSecondFloor(_cattachedPoint);
                    break;
                }
                case TriggerType.MoveToFirstFloor:
                {
                    GameController.instance.MoveToFirstFloor(_cattachedPoint);
                    break;
                }
            }
        }

        public enum TriggerType
        {
            None,
            MoveToSecondFloor,
            MoveToFirstFloor
        }
    }
}
