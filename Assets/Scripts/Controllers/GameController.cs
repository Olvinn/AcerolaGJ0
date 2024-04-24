using System;
using System.Collections;
using Networking;
using Units;
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

        private Server _server;
        private Client _client;

        private IEnumerator Start()
        {
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

            _server = new Server();
            _client = new Client();
        }

        private void Update()
        {
            // if (_server != null)
            //     _server.SendMessage(Time.time.ToString());
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
            _server.Start();

            ChatController.instance.onSendMessage += _server.SendMessage;
            ChatController.instance.onSendMessage += ChatController.instance.ReceiveMessage;
            _server.onRecieveMessage += ChatController.instance.ReceiveMessage;
        }

        public void StartClient()
        {
            _client.Start();
            
            ChatController.instance.onSendMessage += _client.SendMessage;
            _client.onRecieveMessage += ChatController.instance.ReceiveMessage;
        }

        public void StopNetworking()
        {
            _server.Stop();
            _client.Stop();
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
