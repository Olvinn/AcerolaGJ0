using System;
using System.Collections;
using Networking;
using Stages;
using Units;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Controllers
{
    public class GameController : Singleton<GameController>
    {
        [SerializeField] private Stage _gameStage;
        [SerializeField] private Transform _environment, _units, _players;
        [SerializeField] private NavMeshSurface _navmesh;

        private GameObject _enemyPrefab;

        private LocalPlayerController _playerController;
        private GameObject _level;
        private Stage _currentStage;

        private IEnumerator Start()
        {
            _currentStage = _gameStage;
            
            var locationHandleFloor1 = Addressables.LoadAssetAsync<GameObject>(GameConfigsAndSettings.instance.config.debugLevel);
            var playerHandle = Addressables.LoadAssetAsync<GameObject>(GameConfigsAndSettings.instance.config.playerUnit);
            var enemyHandle = Addressables.LoadAssetAsync<GameObject>(GameConfigsAndSettings.instance.config.enemyUnit);

            while (!locationHandleFloor1.IsDone || !playerHandle.IsDone || !enemyHandle.IsDone)
                yield return null;

            _level = Instantiate(locationHandleFloor1.Result, _environment);
            _enemyPrefab = enemyHandle.Result;
        
            _navmesh.BuildNavMesh();
            
            yield return null;
            
            SetUpPlayer(Instantiate(playerHandle.Result, _units).GetComponent<Unit>());

            SpawnEnemies();

            _gameStage.Open();
        }

        public Vector3 GetPlayerPos()
        {
            if (_playerController)
                return _playerController.GetPos();
            return Vector3.zero;
        }

        public bool IsPlayer(Unit unit)
        {
            return _playerController.GetView() == unit;
        }

        public void StartServer()
        {
            NetworkController.instance.networkName = "Host";
            NetworkController.instance.HostGame();

            ChatController.instance.onSendNetworkMessage += NetworkController.instance.SendMessage;
            ChatController.instance.onSendNetworkMessage += ChatController.instance.ReceiveNetworkMessage;
            NetworkController.instance.onReceiveMessage += ChatController.instance.ReceiveNetworkMessage;
        }

        public void StartClient()
        {
            NetworkController.instance.networkName = "Client";
            NetworkController.instance.ConnectToHost();

            ChatController.instance.onSendNetworkMessage += NetworkController.instance.SendMessage;
            NetworkController.instance.onReceiveMessage += ChatController.instance.ReceiveNetworkMessage;
        }

        public void StopNetworking()
        {
            NetworkController.instance.Stop();
        }

        private void ChangeStage(Stage stage)
        {
            _currentStage.Close();
            stage.Open();
            _currentStage = stage;
        }

        private void SetUpPlayer(Unit unit)
        {
            _playerController = new GameObject("Player Controller").AddComponent<LocalPlayerController>();
            _playerController.transform.SetParent(_players);
            Weapon weapon = GameConfigsAndSettings.instance.config.weapons[0];
            UnitModel model = new UnitModel(100, Team.Player, 1.5f, 5, 210, weapon);
            _playerController.SetUnit(unit, model);
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < 5; i++)
            {
                var pos = Point.GetPoint(PointType.Spawn)[i % Point.GetPoint(PointType.Spawn).Count];
                var enemy = new GameObject("Enemy").AddComponent<AIController>();
                enemy.transform.SetParent(_players);
                var unit = Instantiate(_enemyPrefab, _units).GetComponent<Unit>();
                unit.Teleport(pos.transform.position, pos.transform.rotation);
                Weapon weapon = GameConfigsAndSettings.instance.config.weapons[1];
                UnitModel model = new UnitModel(30, Team.Aberrations, 1, 3, 180, weapon);
                enemy.SetUnit(unit, model);
            }
        }
    }
}
