using Controllers;
using Triggers;
using Units;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour
{
    private UnitModel _model;
    private Unit _unit;
    private TriggerBase _mainInteraction, _secondaryInteraction;
    private int _cachedMainHint, _cachedSecondaryHint;

    private void Start()
    {
        InputController.instance.onShoot = Shoot;
    }

    private void Update()
    {
        Vector3 mov = new Vector3(InputController.instance.move.x, 0, InputController.instance.move.y);
        _unit.Move(mov.normalized * GameConfigsAndSettings.instance.config.playerSpeed);
        Vector3 rot = new Vector3(InputController.instance.lookDirection.x, 0,
            InputController.instance.lookDirection.y);
        InputController.instance.aim = _unit.Aim;
        _unit.Look(rot);
    }

    public void SetUnit(Unit unit, UnitModel model)
    {
        _unit = unit;
        _unit.onTriggerEnter += SubscribeOnInput;
        _unit.onTriggerExit += UnsubscribeOnInput;
        _unit.onDamage += TakeDamage;
        _model = model;
        _model.onDead += Die;
    }

    public Unit GetView()
    {
        return _unit;
    }

    public void Teleport(Vector3 pos, Quaternion rot)
    {
        _unit.Teleport(pos, rot);
    }

    public Vector3 GetPos() => _unit.transform.position;

    public void TakeDamage(Damage damage)
    {
        if (_model.TakeDamage(damage))
        {
            CameraController.instance.Shake(GameConfigsAndSettings.instance.config.damageCameraShakingMagnitude,
                GameConfigsAndSettings.instance.config.damageCameraShakingDuration);
            UIController.instance.UpdatePlayerHP(_model.hp, _model.maxHp);
        }
    }
    
    private void SubscribeOnInput(ExposedTrigger trigger)
    {
        if (_mainInteraction == null)
        {
            InputController.instance.onMainInteract += trigger.Trigger;
            _cachedMainHint = UIController.instance.ShowHint(GameConfigsAndSettings.instance.config.mainUseColor,
                trigger.hintText,
                trigger.transform.position + Vector3.up);
            _mainInteraction = trigger;
        }
        else
        {
            InputController.instance.onSecondaryInteract += trigger.Trigger;
            _cachedSecondaryHint = UIController.instance.ShowHint(GameConfigsAndSettings.instance.config.secondaryUseColor,
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

    public void Shoot()
    {
        _unit.Shoot(new Damage() { value = _model.attackDamage, from = _model });
    }

    public void Die()
    {
        
    }
}
