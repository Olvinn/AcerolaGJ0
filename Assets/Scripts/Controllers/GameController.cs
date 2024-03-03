using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Controllers
{
    public class GameController : Singleton<GameController>
    {
        [SerializeField] private Transform _environment, _units, _players;
        [SerializeField] private NavMeshSurface _navmesh;

        private GameObject _enemyPrefab;

        private LocalPlayerController _playerController;
        private GameObject _level;

        private IEnumerator Start()
        {
            var locationHandleFloor1 = Addressables.LoadAssetAsync<GameObject>(GameConfigsAndSettings.instance.config.debugLevel);
            var playerHandle = Addressables.LoadAssetAsync<GameObject>(GameConfigsAndSettings.instance.config.playerUnit);
            var enemyHandle = Addressables.LoadAssetAsync<GameObject>(GameConfigsAndSettings.instance.config.enemyUnit);

            while (!locationHandleFloor1.IsDone || !playerHandle.IsDone || !enemyHandle.IsDone)
                yield return null;

            _level = Instantiate(locationHandleFloor1.Result, _environment);
            _enemyPrefab = enemyHandle.Result;
            SetUpPlayer(Instantiate(playerHandle.Result, _units).GetComponent<Unit>());

            SpawnEnemies();
        
            _navmesh.BuildNavMesh();
        }

        public Vector3 GetPlayerPos()
        {
            if (_playerController)
                return _playerController.GetPos();
            return Vector3.zero;
        }

        private void SetUpPlayer(Unit unit)
        {
            _playerController = new GameObject("Player Controller").AddComponent<LocalPlayerController>();
            _playerController.transform.SetParent(_players);
            _playerController.SetUnit(unit);
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < 5; i++)
            {
                var pos = Point.points[i % Point.points.Count];
                var enemy = new GameObject("Enemy").AddComponent<AIController>();
                enemy.transform.SetParent(_players);
                var unit = Instantiate(_enemyPrefab, _units).GetComponent<Unit>();
                unit.Teleport(pos.transform.position, pos.transform.rotation);
                enemy.SetUnit(unit);
            }
        }
    }
}
