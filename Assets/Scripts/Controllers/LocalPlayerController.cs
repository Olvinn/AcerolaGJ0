using Triggers;
using Units;
using UnityEngine;

namespace Controllers
{
    public class LocalPlayerController : UnitController
    {
        private TriggerBase _mainInteraction, _secondaryInteraction;
        private int _cachedMainHint, _cachedSecondaryHint;
        private bool _isAiming;

        private void Start()
        {
            InputController.instance.onShoot = Shoot;
            InputController.instance.aim += Aim;
            InputController.instance.onReload += Reload;
        }

        protected override void Update()
        {
            base.Update();
            Vector3 mov = new Vector3(InputController.instance.move.x, 0, InputController.instance.move.y);
            if (mov.magnitude > 1f)
                mov.Normalize();
            _unit.Move(mov);
            _unit.Aim(_isAiming);
            Vector3 rot = AimController.instance.worldAimPos - _unit.transform.position;
            if (rot != Vector3.zero)
                _unit.Look(rot);
        }

        public void SetUnit(Unit unit, UnitModel model)
        {
            base.SetUnit(unit, model);
            _unit.onTriggerEnter += SubscribeOnInput;
            _unit.onTriggerExit += UnsubscribeOnInput;
            UIController.instance.UpdateMagazineMagazine(_currentRounds, _model.weapon.magazineCapacity);
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
            base.ShootEffects();
            CameraController.instance.Shake(_model.weapon.recoil, .1f);
            UIController.instance.UpdateMagazineMagazine(_currentRounds, _model.weapon.magazineCapacity);
        }

        protected override void ReloadComplete()
        {
            base.ReloadComplete();
            UIController.instance.UpdateMagazineMagazine(_currentRounds, _model.weapon.magazineCapacity);
        }

        public void Die()
        {
            
        }
        
        void Aim(bool value)
        {
            _isAiming = value;
        }
    }
}
