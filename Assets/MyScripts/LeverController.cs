using UnityEngine;
using UnityEngine.Events;

public class LeverBase : MonoBehaviour
{
    [Header("State")]
    public bool IsActivated { get; private set; }

    [Header("Events")]
    public UnityEvent onActivated;
    public UnityEvent onDeactivated;

    // Optional: simple visual feedback
    

    void Start()
    {
       
    }

    public void SetActivated(bool value)
    {
        if (IsActivated == value) return;
        IsActivated = value;
        ApplyVisual();

        if (IsActivated) onActivated?.Invoke();
        else onDeactivated?.Invoke();
    }

    void ApplyVisual()
    {
        
    }
}
