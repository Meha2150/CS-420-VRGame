using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuRoot; // your pause canvas root
    [SerializeField] private UITimerContoller timer;            // optional: your timer script

    [Header("Movement to disable while paused")]
    // Drag in your locomotion providers here (e.g. ActionBasedContinuousMoveProvider,
    // ActionBasedContinuousTurnProvider, TeleportationProvider, etc.)
    [SerializeField] private Behaviour[] locomotion;

    [Header("UI pointers to enable while paused")]
    // Laser(s) configured to hit only UI (Tracked Device Graphic Raycaster on canvas)
    [SerializeField] private XRRayInteractor[] uiRays;

    private bool paused;

    private void Start() => SetPaused(false);

    public void TogglePause() => SetPaused(!paused);
    public void ShowPause() => SetPaused(true);
    public void HidePause() => SetPaused(false);

    private void SetPaused(bool value)
    {
        paused = value;

        if (pauseMenuRoot) pauseMenuRoot.SetActive(paused);

        // stop/resume timer
        if (timer)
        {
            if (paused) timer.PauseTimer();
            else timer.ResumeTimer();
        }

        // stop/resume movement
        if (locomotion != null)
            foreach (var comp in locomotion)
                if (comp) comp.enabled = !paused;

        // enable/disable UI rays
        if (uiRays != null)
            foreach (var ray in uiRays)
                if (ray) ray.enabled = paused;
    }
}
