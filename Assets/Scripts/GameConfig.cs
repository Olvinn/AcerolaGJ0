using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Configs/Game")]
public class GameConfig : ScriptableObject
{
    // Addressables
    public AssetReferenceGameObject playerUnit;
    public AssetReferenceGameObject enemyUnit;
    public AssetReferenceGameObject debugLevel;
    public AssetReferenceGameObject hint;
    public AssetReferenceGameObject bulletImpactVFX;
    
    // Unit Configs
    [Space(10)] public float playerSpeed = 50;
    public float playerAngularSpeed = 720;
    public float shootingDistance = 100;
    
    //AI configs
    [Space(10)] public float aiThinkingDelay;
    
    //Camera Configs
    [Space(10)] public float cameraLerpSpeed = 10;
    
    //World Configs
    [Space(10)] public Color useColor;
}
