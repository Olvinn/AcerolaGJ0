using Controllers;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public TriggerType type;

    private void OnTriggerEnter(Collider other)
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
}

public enum TriggerType
{
    MoveToSecondFloor,
    MoveToFirstFloor
}