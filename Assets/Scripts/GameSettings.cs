using UnityEngine;

[CreateAssetMenu(menuName = "Game/Settings")]
public class GameSettings : ScriptableObject
{
    //Camera Configs
    [Space(10)] public bool cameraShaking;
}
