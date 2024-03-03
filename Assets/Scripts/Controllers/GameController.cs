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

        private LocalPlayerController _playerController;
        private GameObject _level;

        private IEnumerator Start()
        {
            var locationHandleFloor1 = Addressables.LoadAssetAsync<GameObject>(GameConfigsContainer.instance.config.debugLevel);
            var playerHandle = Addressables.LoadAssetAsync<GameObject>(GameConfigsContainer.instance.config.unit);

            while (!locationHandleFloor1.IsDone || !playerHandle.IsDone)
                yield return null;

            _level = Instantiate(locationHandleFloor1.Result, _environment);
            SetUpPlayer(Instantiate(playerHandle.Result, _units).GetComponent<Unit>());
        
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
            _playerController.SetUnit(unit);
        }
    }
}
