using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game/Configs")]
public class GameConfig : ScriptableObject
{
    // Addressables
    public AssetReferenceGameObject playerUnit;
    public AssetReferenceGameObject enemyUnit;
    public AssetReferenceGameObject debugLevel;
    public AssetReferenceGameObject hint;
    public AssetReferenceGameObject bulletImpactVFX;
    public AssetReferenceGameObject bulletTrailVFX;
    
    // Unit Configs
    [Space(10)] 
    public float shootingDistance = 100;
    
    //AI configs
    [Space(10)] public float aiThinkingDelay;
    
    //Camera Configs
    [Space(10)] public float cameraLerpSpeed = 10;
    public float damageCameraShakingMagnitude = 1.5f;
    public float damageCameraShakingDuration = .35f;
    
    //World Configs
    [Space(10)] public Color mainUseColor;
    public Color secondaryUseColor;
}
