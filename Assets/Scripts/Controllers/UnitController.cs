using Units;
using UnityEngine;

namespace Controllers
{
    public class UnitController : MonoBehaviour
    {
        protected UnitModel _model;
        protected Unit _unit;
        protected bool _isShooting, isReloading;
        protected int _currentRounds;

        protected virtual void Update()
        {
            if (_isShooting)
            {
                if (_currentRounds > 0)
                {
                    if (_unit.Shoot(_model.weapon, _model)) ShootEffects();
                }
                else Reload();
            }
        }

        protected virtual void ShootEffects()
        {
            _currentRounds--;
        }

        public virtual void Reload()
        {
            if (_currentRounds == _model.weapon.magazineCapacity || isReloading)
                return;
            isReloading = true;
            _unit.Reload();
        }

        public virtual void SetUnit(Unit unit, UnitModel model)
        {
            _unit = unit;
            _unit.onDamage = TakeDamage;
            _model = model;
            _model = model;
            _model.onDead += Die;
            _unit.SetUpMobility(model.speed, model.aimSpeed, model.angularSpeed);
            _unit.SetUpFirepower(model.weapon.rateOfFire);
            _unit.onReloadComplete = ReloadComplete;
            _currentRounds = model.weapon.magazineCapacity;
        }

        public void Die()
        {
            StopAllCoroutines();
            Destroy(_unit.gameObject);
            Destroy(gameObject);
        }
        
        public virtual void TakeDamage(Damage damage)
        {
            _model.TakeDamage(damage);
        }
        
        public virtual void Shoot(bool value)
        {
            _isShooting = value;
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

        protected virtual void ReloadComplete()
        {
            _currentRounds = _model.weapon.magazineCapacity;
            isReloading = false;
        }
    }
}
