using UnityEngine;

public class LocalPlayerController : MonoBehaviour
{
    private Unit _unit;
    
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 mov = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _unit.Move(mov.normalized * GameConfigsContainer.instance.config.playerSpeed);
    }

    public void SetUnit(Unit unit)
    {
        _unit = unit;
    }
}
