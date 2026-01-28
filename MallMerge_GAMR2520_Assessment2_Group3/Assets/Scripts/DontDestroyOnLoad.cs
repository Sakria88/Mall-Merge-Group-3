using UnityEngine;

public sealed class DontDestroyOnLoadUI : MonoBehaviour
{
    private static DontDestroyOnLoadUI instance;

    private bool Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return false;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        return true;
    }
}

