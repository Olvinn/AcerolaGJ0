using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Configs/Game")]
public class GameConfig : ScriptableObject
{
    // Addressables
    public AssetReferenceGameObject unit;
    public AssetReferenceGameObject testFloor1, testFloor2;
    public AssetReferenceGameObject hint;
    
    // Unit Configs
    [Space(10)] public float playerSpeed = 50;
    public float playerAngularSpeed = 720;
    
    //Camera Configs
    [Space(10)] public float cameraLerpSpeed = 10;
    
    //World Configs
    [Space(10)] public Color useColor;
}
