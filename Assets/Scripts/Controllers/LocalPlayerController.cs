using Controllers;
using Triggers;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour
{
    private Unit _unit;
    private TriggerBase _mainInteraction, _secondaryInteraction;
    private int _cachedMainHint, _cachedSecondaryHint;

    private void Start()
    {
        InputController.instance.onShoot = _unit.Shoot;
    }

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
        _unit.onTriggerEnter += SubscribeOnInput;
        _unit.onTriggerExit += UnsubscribeOnInput;
    }

    public void Teleport(Vector3 pos, Quaternion rot)
    {
        _unit.Teleport(pos, rot);
    }

    public Vector3 GetPos() => _unit.transform.position;
    
    private void SubscribeOnInput(ExposedTrigger trigger)
    {
        if (_mainInteraction == null)
        {
            InputController.instance.onMainInteract += trigger.Trigger;
            _cachedMainHint = UIController.instance.ShowHint(GameConfigsContainer.instance.config.mainUseColor,
                trigger.hintText,
                trigger.transform.position + Vector3.up);
            _mainInteraction = trigger;
        }
        else
        {
            InputController.instance.onSecondaryInteract += trigger.Trigger;
            _cachedSecondaryHint = UIController.instance.ShowHint(GameConfigsContainer.instance.config.secondaryUseColor,
                trigger.hintText,
                trigger.transform.position + Vector3.up);
            _secondaryInteraction = trigger;
        }
    }
    
    private void UnsubscribeOnInput(ExposedTrigger trigger)
    {
        if (trigger == _mainInteraction)
        {
            InputController.instance.onMainInteract -= trigger.Trigger;
            UIController.instance.HideHint(_cachedMainHint);
            _mainInteraction = null;
        }
        else if (trigger == _secondaryInteraction)
        {
            InputController.instance.onSecondaryInteract -= trigger.Trigger;
            UIController.instance.HideHint(_cachedSecondaryHint);
            _secondaryInteraction = null;
        }
    }
}
