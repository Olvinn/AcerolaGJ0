using Triggers;
using Units;
using UnityEngine;

namespace Controllers
{
    public class LocalPlayerController : UnitController
    {
        private TriggerBase _mainInteraction, _secondaryInteraction;
        private int _cachedMainHint, _cachedSecondaryHint;

        private void Start()
        {
            InputController.instance.onShoot = Shoot;
        }

        protected override void Update()
        {
            base.Update();
            Vector3 mov = new Vector3(InputController.instance.move.x, 0, InputController.instance.move.y);
            if (mov.magnitude > 1f)
                mov.Normalize();
            _unit.Move(mov);
            Vector3 rot = new Vector3(InputController.instance.lookDirection.x, 0,
                InputController.instance.lookDirection.y);
            InputController.instance.aim = _unit.Aim;
            _unit.Look(rot);
        }

        public void SetUnit(Unit unit, UnitModel model)
        {
            base.SetUnit(unit, model);
            _unit.onTriggerEnter += SubscribeOnInput;
            _unit.onTriggerExit += UnsubscribeOnInput;
        }

        public override void TakeDamage(Damage damage)
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

        protected override void ShootEffects()
        {
            CameraController.instance.Shake(.5f, .1f);
        }

        public void Die()
        {
            
        }
    }
}
