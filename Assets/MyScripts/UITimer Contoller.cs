using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UITimerContoller : MonoBehaviour
{
    public enum Mode { Stopwatch, Countdown }

    [Header("Mode & Time")]
    public Mode mode = Mode.Stopwatch;
    [Tooltip("Countdown only: starting time (seconds).")]
    public float countdownStartSeconds = 300f; // 5 minutes
    [Tooltip("Use unscaled time (ignores Time.timeScale).")]
    public bool useUnscaledTime = true;

    [Header("Display")]
    [Tooltip("Where to print the time (TextMeshProUGUI).")]
    public TMP_Text targetText;
    [Tooltip("Show hours when >= 1 hour.")]
    public bool showHours = true;
    [Tooltip("Show hundredths (00:00:00 style).")]
    public bool showHundredths = true;

    private bool _running;
    private float _time;  // seconds (elapsed for stopwatch, remaining for countdown)

    void Awake()
    {
        if (targetText == null)
            Debug.LogWarning($"{name}: UITimer targetText not assigned.");
        ResetTimer();
        UpdateText();
    }

    void Update()
    {
        if (!_running) return;

        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        if (mode == Mode.Stopwatch)
        {
            _time += dt;
        }
        else // Countdown
        {
            _time -= dt;
            if (_time <= 0f)
            {
                _time = 0f;
                _running = false;
                
            }
        }

        UpdateText();
    }

    private void UpdateText()
    {
        if (!targetText) return;

        int totalCentis = Mathf.Max(0, Mathf.FloorToInt(_time * 100f));
        int centi = totalCentis % 100;
        int totalSecs = totalCentis / 100;
        int secs = totalSecs % 60;
        int mins = (totalSecs / 60) % 60;
        int hours = totalSecs / 3600;

        if (showHours || hours > 0)
        {
            if (showHundredths)
                targetText.text = $"{hours:00}:{mins:00}:{secs:00}.{centi:00}";
            else
                targetText.text = $"{hours:00}:{mins:00}:{secs:00}";
        }
        else
        {
            if (showHundredths)
                targetText.text = $"{mins:00}:{secs:00}.{centi:00}";
            else
                targetText.text = $"{mins:00}:{secs:00}";
        }
    }

    // --- Public controls (call these from buttons or other scripts) ---

    public void StartTimer()
    {
        if (_running) return;
        _running = true;
       
    }

    public void PauseTimer()
    {
        if (!_running) return;
        _running = false;
        
    }

    public void ResumeTimer()
    {
        if (_running) return;
        _running = true;
        
    }

    public void ResetTimer()
    {
        _running = false;
        _time = (mode == Mode.Countdown) ? Mathf.Max(0f, countdownStartSeconds) : 0f;
        UpdateText();
      
    }

    public void SetCountdownSeconds(float seconds)
    {
        countdownStartSeconds = Mathf.Max(0f, seconds);
        if (mode == Mode.Countdown && !_running)
        {
            _time = countdownStartSeconds;
            UpdateText();
        }
    }

    public bool IsRunning() => _running;
    public float CurrentSeconds() => _time;
}
