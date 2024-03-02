using Controllers;
using UnityEngine;

namespace Triggers
{
    public class TriggerBase : MonoBehaviour
    {
        public TriggerType type;

        private void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        public virtual void Trigger()
        {
            switch (type)
            {
                case TriggerType.MoveToSecondFloor:
                {
                    GameController.instance.MoveToSecondFloor();
                    break;
                }
                case TriggerType.MoveToFirstFloor:
                {
                    GameController.instance.MoveToFirstFloor();
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
