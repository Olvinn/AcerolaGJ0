using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class AIController : MonoBehaviour
    {
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

        public void SetUnit(Unit unit)
        {
            _unit = unit;
            _unit.onCollide = OnCollision;
            _unit.onDamage = Die;
        }

        public void Die()
        {
            Destroy(_unit.gameObject);
            Destroy(gameObject);
        }

        private void FindPlayer()
        {
            Ray ray = new Ray(_unit.transform.position + Vector3.up, GameController.instance.GetPlayerPos() - _unit.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, GameConfigsContainer.instance.config.shootingDistance, _unitsLayer))
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
            yield return new WaitForSeconds(GameConfigsContainer.instance.config.aiThinkingDelay);
            FindPlayer();
            Move();
            if (_playerFound)
                _unit.Shoot();
            StartCoroutine(Thinking());
        }
    }
}
