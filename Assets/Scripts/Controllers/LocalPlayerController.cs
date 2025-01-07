using Commands;
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
            InputController.Instance.onShoot = Shoot;
            InputController.Instance.aim += Aim;
            InputController.Instance.onReload += Reload;
        }

        protected override void Update()
        {
            base.Update();
            Vector3 mov = new Vector3(InputController.Instance.move.x, 0, InputController.Instance.move.y);
            if (mov.magnitude > 1f)
                mov.Normalize();
            _unit.Move(mov);
            _unit.Aim(_isAiming);
            Vector3 rot = AimController.Instance.worldAimPos - _unit.transform.position;
            _unit.Look(rot);
        }

        public void SetUnit(Unit unit, UnitModel model)
        {
            base.SetUnit(unit, model);
            _unit.onTriggerEnter += SubscribeOnInput;
            _unit.onTriggerExit += UnsubscribeOnInput;
            CommandBus.Instance.Handle(new UpdatePlayerWeapon()
            {
                CurrentMag = _currentRounds,
                MaxMag = _model.weapon.magazineCapacity
            });
        }

        public override void TakeDamage(Damage damage)
        {
            if (_model.TakeDamage(damage))
            {
                CameraController.Instance.Shake(GameConfigsAndSettings.Instance.config.damageCameraShakingMagnitude,
                    GameConfigsAndSettings.Instance.config.damageCameraShakingDuration);
                CommandBus.Instance.Handle(new UpdatePlayer()
                {
                    CurrentHP = _model.hp,
                    MaxHP = _model.maxHp
                });
            }
        }
    
        private void SubscribeOnInput(ExposedTrigger trigger)
        {
            if (_mainInteraction == null)
            {
                InputController.Instance.onMainInteract += trigger.Trigger;
                CommandBus.Instance.Handle(new ShowHint()
                {
                    Id = trigger.GetHashCode(),
                    Pos = trigger.transform.position + Vector3.up,
                    Text = trigger.hintText
                });
                _mainInteraction = trigger;
            }
            else
            {
                InputController.Instance.onSecondaryInteract += trigger.Trigger;
                CommandBus.Instance.Handle(new ShowHint()
                {
                    Id = trigger.GetHashCode(),
                    Pos = trigger.transform.position + Vector3.up,
                    Text = trigger.hintText
                });
                _secondaryInteraction = trigger;
            }
        }
    
        private void UnsubscribeOnInput(ExposedTrigger trigger)
        {
            if (trigger == _mainInteraction)
            {
                InputController.Instance.onMainInteract -= trigger.Trigger;
                CommandBus.Instance.Handle(new HideHint()
                {
                    Id = trigger.GetHashCode()
                });
                _mainInteraction = null;
            }
            else if (trigger == _secondaryInteraction)
            {
                InputController.Instance.onSecondaryInteract -= trigger.Trigger;
                CommandBus.Instance.Handle(new HideHint()
                {
                    Id = trigger.GetHashCode()
                });
                _secondaryInteraction = null;
            }
        }

        protected override void ShootEffects()
        {
            base.ShootEffects();
            CameraController.Instance.Shake(_model.weapon.recoil, .1f);
            CommandBus.Instance.Handle(new UpdatePlayerWeapon()
            {
                CurrentMag = _currentRounds,
                MaxMag = _model.weapon.magazineCapacity
            });
        }

        protected override void ReloadComplete()
        {
            base.ReloadComplete();
            CommandBus.Instance.Handle(new UpdatePlayerWeapon()
            {
                CurrentMag = _currentRounds,
                MaxMag = _model.weapon.magazineCapacity
            });
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
