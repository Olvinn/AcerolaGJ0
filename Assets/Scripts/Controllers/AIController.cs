using System.Collections;
using Units;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class AIController : UnitController
    {
        [SerializeField] private bool _playerFound;
        private LayerMask _unitsLayer;
        private Vector3 _playerLastSeenPos;

        private void Start()
        {
            _unitsLayer = (~LayerMask.NameToLayer("Units")|~LayerMask.NameToLayer("Default"));
            StartCoroutine(Thinking());
        }

        protected override void Update()
        {
            base.Update();
            _unit.Aim(_playerFound);
            if (_playerFound)
                _unit.Look(GameController.instance.GetPlayerPos() - _unit.transform.position);
        }

        public override void SetUnit(Unit unit, UnitModel model)
        {
            base.SetUnit(unit, model);
            _unit.onCollide = OnCollision;
        }

        private void OnCollision(ContactPoint[] contacts)
        {
            _unit.MoveTo((_unit.transform.position - contacts[0].point).normalized * 5);
            _playerFound = false;
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
            {
                _unit.MoveTo(GameController.instance.GetPlayerPos());
                _isShooting = true;
            }
            else
                _unit.MoveTo(_unit.transform.position + new Vector3(Random.Range(-10,10), 0, Random.Range(-10,10)));
        }
        
        private void CheckLastPlayerPos()
        {
            if (_playerFound)
                _unit.MoveTo(_playerLastSeenPos);

            _playerFound = false;
            _isShooting = false;
        }

        IEnumerator Thinking()
        {
            yield return new WaitForSeconds(GameConfigsAndSettings.instance.config.aiThinkingDelay + Random.Range(-1f, 1f));
            
            Move();
            if (!FindPlayer())
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
