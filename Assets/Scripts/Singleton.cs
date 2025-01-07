using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this as T;
    }
}
