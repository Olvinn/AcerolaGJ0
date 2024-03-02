using Controllers;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour
{
    private Unit _unit;

    void Update()
    {
        Vector3 mov = new Vector3(InputController.instance.move.x, 0, InputController.instance.move.y);
        _unit.Move(mov.normalized * GameConfigsContainer.instance.config.playerSpeed);
        Vector3 rot = new Vector3(InputController.instance.lookDirection.x, 0,
            InputController.instance.lookDirection.y);
        _unit.Look(rot);
    }

    public void SetUnit(Unit unit)
    {
        _unit = unit;
    }

    public void Teleport(Vector3 pos, Quaternion rot)
    {
        _unit.Teleport(pos, rot);
    }

    public Vector3 GetPos() => _unit.transform.position;
}
