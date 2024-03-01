using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameController : Singleton<GameController>
{
    [SerializeField] private Transform _environment, _units, _players;
    
    [SerializeField] private NavMeshSurface _navmesh;

    private LocalPlayerController _playerController;

    private IEnumerator Start()
    {
        var locationHandleFloor1 = Addressables.LoadAssetAsync<GameObject>(GameConfigsContainer.instance.config.testFloor1);
        var playerHandle = Addressables.LoadAssetAsync<GameObject>(GameConfigsContainer.instance.config.unit);

        while (!locationHandleFloor1.IsDone || !playerHandle.IsDone)
            yield return null;

        Instantiate(locationHandleFloor1.Result, _environment);
        SetUpPlayer(Instantiate(playerHandle.Result, _units).GetComponent<Unit>());
        
        _navmesh.BuildNavMesh();
    }

    public void LoadSecondFloor()
    {
        StartCoroutine(LoadingSecondFloor());
    }

    private void SetUpPlayer(Unit unit)
    {
        _playerController = new GameObject("Player Controller").AddComponent<LocalPlayerController>();
        _playerController.SetUnit(unit);
    }

    IEnumerator LoadingSecondFloor()
    {
        var locationHandleFloor2 = Addressables.LoadAssetAsync<GameObject>(GameConfigsContainer.instance.config.testFloor2);
        
        while (!locationHandleFloor2.IsDone)
            yield return null;
        
        Instantiate(locationHandleFloor2.Result, _environment);
        _navmesh.BuildNavMesh();
        _playerController.Teleport(_playerController.GetPos() + Vector3.up * GameConfigsContainer.instance.config.secondFloorHeight);
    }
}
