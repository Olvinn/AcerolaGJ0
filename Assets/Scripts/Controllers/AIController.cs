using System;
using System.Collections;
using Units;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class AIController : MonoBehaviour
    {
        private UnitModel _model;
        private Unit _unit;
        [SerializeField] private bool _playerFound;
        private LayerMask _unitsLayer;
        private Vector3 _playerLastSeenPos;

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
            _playerFound = false;
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
            _model.TakeDamage(damage);
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

        private bool FindPlayer()
        {
            Vector3 playerPos = GameController.instance.GetPlayerPos();
            Ray ray = new Ray(_unit.transform.position + Vector3.up, playerPos - _unit.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, GameConfigsAndSettings.instance.config.shootingDistance, _unitsLayer))
            {
                if (Vector3.Distance(playerPos, hit.point + Vector3.down) < 2)
                {
                    _playerLastSeenPos = playerPos;
                    _playerFound = true;
                    return true;
                }
            }
            return false;
        }

        private void Move()
        {
            if (_playerFound)
                _unit.MoveTo(GameController.instance.GetPlayerPos());
            else
                _unit.MoveTo(_unit.transform.position + new Vector3(Random.Range(-10,10), 0, Random.Range(-10,10)));
        }
        
        private void CheckLastPlayerPos()
        {
            if (_playerFound)
                _unit.MoveTo(_playerLastSeenPos);

            _playerFound = false;
        }

        IEnumerator Thinking()
        {
            yield return new WaitForSeconds(GameConfigsAndSettings.instance.config.aiThinkingDelay + Random.Range(-1f, 1f));
            
            Move();
            if (FindPlayer())
                Shoot();
            else
                CheckLastPlayerPos();
            
            StartCoroutine(Thinking());
        }

        private void OnDrawGizmos()
        {
            Handles.color = Color.black;
            Handles.Label(_unit.transform.position, _playerLastSeenPos.ToString());
        }
    }
}
