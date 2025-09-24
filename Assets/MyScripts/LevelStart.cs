using UnityEngine;

public class LevelTimerCoordinator : MonoBehaviour
{
    [Tooltip("Reference to the UITimer component that renders the time.")]
    public UITimerContoller timer;

    [Tooltip("Reference to the PauseMenuWatcher in the scene.")]
    public PauseMenuWatch pauseWatcher;

    [Tooltip("If true, starts the timer on scene load (Start).")]
    public bool autoStartOnSceneLoad = true;

    private bool _finished;

    private void Awake()
    {
        if (!timer) timer = FindFirstObjectByType<UITimerContoller>(FindObjectsInactive.Include);
        if (!pauseWatcher) pauseWatcher = FindFirstObjectByType<PauseMenuWatch>(FindObjectsInactive.Include);

        if (!timer) Debug.LogError($"{name}: No UITimer found/assigned.");
        if (!pauseWatcher) Debug.LogWarning($"{name}: No PauseMenuWatcher found/assigned (pause won’t affect timer).");
    }

    

    private void Start()
    {
        if (!timer) return;

        // Ensure unscaled time (so Time.timeScale=0 won’t tick the timer)
        timer.useUnscaledTime = true;

        // Fresh reset, then start if requested
        timer.ResetTimer();
        if (autoStartOnSceneLoad) timer.StartTimer();
    }

    private void HandlePauseChanged(bool paused)
    {
        if (!timer || _finished) return;
        if (paused) timer.PauseTimer();
        else timer.ResumeTimer();
    }

    // Call this from your level win logic (e.g., door opened, boss dead, etc.)
    public void OnLevelComplete()
    {
        if (!timer || _finished) return;
        _finished = true;
        timer.PauseTimer();  // final time is frozen on screen
        // Optional: fire any celebration here, save time, etc.
    }
}
