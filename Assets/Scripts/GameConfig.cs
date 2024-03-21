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
    public float aiDetectDistance = 10;
    
    //Camera Configs
    [Space(10)] public float cameraLerpSpeed = 10;
    public float cameraAimLerpSpeed = 3f;
    public float damageCameraShakingMagnitude = 1.5f;
    public float damageCameraShakingDuration = .35f;
    public float aimDistance = 5;
    
    //World Configs
    [Space(10)] public Color mainUseColor;
    public Color secondaryUseColor;

    [Space(10)] public Weapon[] weapons;
}
