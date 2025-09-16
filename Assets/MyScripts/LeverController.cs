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
    [Header("Optional Feedback")]
    public Renderer indicatorRenderer;
    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.green;

    void Start()
    {
        ApplyVisual();
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
        if (indicatorRenderer != null)
        {
            var m = indicatorRenderer.material;
            m.color = IsActivated ? activeColor : inactiveColor;
        }
    }
}
