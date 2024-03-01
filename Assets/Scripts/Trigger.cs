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
                GameController.instance.LoadSecondFloor();
                break;
            }
        }
    }
}

public enum TriggerType
{
    MoveToSecondFloor,
}