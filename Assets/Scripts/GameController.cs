using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameController : MonoBehaviour
{
    [SerializeField] private string _playerAddressable;
    [SerializeField] private string _locationAddressable;

    [SerializeField] private Transform _environment, _units;
    
    [SerializeField] private NavMeshSurface _navmesh;

    private IEnumerator Start()
    {
        var locationHandle = Addressables.LoadAssetAsync<GameObject>(_locationAddressable);
        var playerHandle = Addressables.LoadAssetAsync<GameObject>(_playerAddressable);

        while (!locationHandle.IsDone || !playerHandle.IsDone)
            yield return null;

        Instantiate(locationHandle.Result, _environment);
        Instantiate(playerHandle.Result, _units);
        
        _navmesh.BuildNavMesh();
    }
}
