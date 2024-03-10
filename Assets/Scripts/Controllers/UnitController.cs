using Units;
using UnityEngine;

namespace Controllers
{
    public class UnitController : MonoBehaviour
    {
        protected UnitModel _model;
        protected Unit _unit;
        
        public virtual void SetUnit(Unit unit, UnitModel model)
        {
            _unit = unit;
            _unit.onDamage = TakeDamage;
            _model = model;
            _model = model;
            _model.onDead += Die;
            _unit.SetUp(GameConfigsAndSettings.instance.config.playerSpeed, GameConfigsAndSettings.instance.config.playerSpeed * .5f,
                GameConfigsAndSettings.instance.config.playerAngularSpeed);
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
        
        public virtual void Shoot()
        {
            _unit.Shoot(new Damage() { value = _model.attackDamage, from = _model });
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
    }
}
