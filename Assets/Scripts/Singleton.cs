using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this as T;
    }
}