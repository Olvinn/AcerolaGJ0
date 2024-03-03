using System.Collections;
using Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class AIController : MonoBehaviour
    {
        private UnitModel _model;
        private Unit _unit;
        [SerializeField] private bool _playerFound;
        private LayerMask _unitsLayer;

        private void Start()
        {
            _unitsLayer = (~LayerMask.NameToLayer("Units")|~LayerMask.NameToLayer("Default"));
            StartCoroutine(Thinking());
        }

        private void Update()
        {
            if (_playerFound)
                _unit.Look(GameController.instance.GetPlayerPos() - _unit.transform.position);
        }

        private void OnCollision(ContactPoint[] contacts)
        {
            _unit.MoveTo((_unit.transform.position - contacts[0].point).normalized * 5);
        }

        public void SetUnit(Unit unit, UnitModel model)
        {
            _unit = unit;
            _unit.onCollide = OnCollision;
            _unit.onDamage = TakeDamage;
            _model = model;
            _model = model;
            _model.onDead += Die;
        }
        
        public void TakeDamage(Damage damage)
        {
            if (_model.TakeDamage(damage))
            {
                CameraController.instance.Shake(GameConfigsAndSettings.instance.config.damageCameraShakingMagnitude,
                    GameConfigsAndSettings.instance.config.damageCameraShakingDuration);
            }
        }

        public void Die()
        {
            StopAllCoroutines();
            Destroy(_unit.gameObject);
            Destroy(gameObject);
        }
        
        public void Shoot()
        {
            _unit.Shoot(new Damage() { value = _model.attackDamage, from = _model });
        }

        private void FindPlayer()
        {
            Ray ray = new Ray(_unit.transform.position + Vector3.up, GameController.instance.GetPlayerPos() - _unit.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, GameConfigsAndSettings.instance.config.shootingDistance, _unitsLayer))
            {
                if (Vector3.Distance(GameController.instance.GetPlayerPos(), hit.point + Vector3.down) < 2)
                    _playerFound = true;
                Debug.DrawLine(_unit.transform.position + Vector3.up, hit.point);
            }
        }

        private void Move()
        {
            if (_playerFound)
                _unit.MoveTo(GameController.instance.GetPlayerPos());
            else
                _unit.MoveTo(_unit.transform.position + new Vector3(Random.Range(-10,10), 0, Random.Range(-10,10)));
        }

        IEnumerator Thinking()
        {
            yield return new WaitForSeconds(GameConfigsAndSettings.instance.config.aiThinkingDelay + Random.Range(-1f, 1f));
            FindPlayer();
            Move();
            if (_playerFound)
                Shoot();
            StartCoroutine(Thinking());
        }
    }
}
