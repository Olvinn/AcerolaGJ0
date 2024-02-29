using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Game")]
public class GameConfig : ScriptableObject
{
    // Addressables
    public string unitAddressable = "unit_Player";
    public string testEnvironmentAddressable = "location_Test";
    
    // Unit Configs
    public float playerSpeed = 50;
}
