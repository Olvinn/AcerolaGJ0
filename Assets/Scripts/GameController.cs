using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform _environment, _units, _players;
    
    [SerializeField] private NavMeshSurface _navmesh;

    private LocalPlayerController _playerController;

    private IEnumerator Start()
    {
        var locationHandle = Addressables.LoadAssetAsync<GameObject>(GameConfigsContainer.instance.config.testEnvironmentAddressable);
        var playerHandle = Addressables.LoadAssetAsync<GameObject>(GameConfigsContainer.instance.config.unitAddressable);

        while (!locationHandle.IsDone || !playerHandle.IsDone)
            yield return null;

        Instantiate(locationHandle.Result, _environment);
        SetUpPlayer(Instantiate(playerHandle.Result, _units).GetComponent<Unit>());
        
        _navmesh.BuildNavMesh();
    }

    private void SetUpPlayer(Unit unit)
    {
        _playerController = new GameObject("Player Controller").AddComponent<LocalPlayerController>();
        _playerController.SetUnit(unit);
    }
}
