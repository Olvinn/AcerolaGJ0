using Controllers;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour
{
    private Unit _unit;
    
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 mov = new Vector3(InputController.instance.move.x, 0, InputController.instance.move.y);
        _unit.Move(mov.normalized * GameConfigsContainer.instance.config.playerSpeed);
    }

    public void SetUnit(Unit unit)
    {
        _unit = unit;
    }

    public void Teleport(Vector3 pos)
    {
        _unit.Teleport(pos);
    }

    public Vector3 GetPos() => _unit.transform.position;
}
