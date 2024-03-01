using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Configs/Game")]
public class GameConfig : ScriptableObject
{
    // Addressables
    public AssetReferenceGameObject unit;
    public AssetReferenceGameObject testFloor1, testFloor2;
    
    // Unit Configs
    [Space(10)] public float playerSpeed = 50;
    
    //World Configs
    [Space(10)] public float secondFloorHeight = 6;
}
