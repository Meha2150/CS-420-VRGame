using UnityEngine;

public class RunResults : MonoBehaviour
{
    public static RunResults Instance { get; private set; }
    public float lastRunSeconds;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
