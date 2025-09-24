using UnityEngine;
using UnityEngine.Events;

public class PauseMenuWatch : MonoBehaviour
{
    [Tooltip("Root GameObject of your pause menu UI (the one you SetActive(true/false)).")]
    public GameObject pauseMenuRoot;

    [System.Serializable] public class BoolEvent : UnityEvent<bool> { }
    [Tooltip("Invoked with true when paused, false when unpaused.")]


    private bool _last;

    private void Awake()
    {
        if (!pauseMenuRoot)
            Debug.LogWarning($"{name}: PauseMenuWatcher has no pauseMenuRoot assigned.");
        _last = pauseMenuRoot && pauseMenuRoot.activeSelf;
    }

    private void Update()
    {
        if (!pauseMenuRoot) return;
        bool now = pauseMenuRoot.activeSelf;
        if (now != _last)
        {
            _last = now;
            
        }
    }
}
