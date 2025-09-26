using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuRoot;        // World-space Canvas root
    [SerializeField] private UITimerContoller timer;          // optional
    [SerializeField] private GameObject IngameView;
    [SerializeField] private XRRayInteractor Ray;
    [SerializeField] private CharacterController VRPlayer;

    [Header("Head / Placement")]
    [Tooltip("Center-eye (VR camera) transform.")]
    [SerializeField] private Transform head;                  // assign XR Origin's Camera
    [Tooltip("Distance in front of the head.")]
    [SerializeField] private float spawnDistance = 1.25f;
    [Tooltip("Vertical offset from head height.")]
    [SerializeField] private float verticalOffset = -0.05f;
    [Tooltip("How fast the menu follows position/rotation while paused.")]
    [SerializeField] private float followLerp = 12f;

    [Header("Turn Actions (assign from your input actions)")]
    [Tooltip("Continuous turn (e.g., XRI RightHand/Turn)")]
    [SerializeField] private InputActionReference continuousTurnAction;
    [Tooltip("Snap turn (e.g., XRI RightHand/SnapTurn)")]
    [SerializeField] private InputActionReference snapTurnAction;

    [Header("UI pointers to enable while paused")]
    [SerializeField] private XRRayInteractor[] uiRays;

    private bool paused;

    void Awake()
    {
        if (!head)
        {
            var cam = Camera.main;
            if (cam) head = cam.transform;
        }
        if (pauseMenuRoot) pauseMenuRoot.SetActive(false);

        // Ensure turning actions are enabled at startup
        EnableTurn(true);
    }

    void Start() => SetPaused(false);

    public void TogglePause() => SetPaused(!paused);
    public void ShowPause() => SetPaused(true);
    public void HidePause() => SetPaused(false);

    void Update()
    {
        if (paused && pauseMenuRoot && head)
        {
            // Keep the menu in front of the player, yaw-only facing
            Vector3 flatForward = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
            if (flatForward.sqrMagnitude < 0.0001f) flatForward = head.forward; // fallback

            Vector3 targetPos = head.position + flatForward * spawnDistance;
            targetPos.y = head.position.y + verticalOffset;

            pauseMenuRoot.transform.position =
                Vector3.Lerp(pauseMenuRoot.transform.position, targetPos, Time.deltaTime * followLerp);

            Quaternion targetRot = Quaternion.LookRotation(flatForward, Vector3.up);
            pauseMenuRoot.transform.rotation =
                Quaternion.Slerp(pauseMenuRoot.transform.rotation, targetRot, Time.deltaTime * followLerp);
        }
    }

    private void SetPaused(bool value)
    {
        paused = value;

        if (pauseMenuRoot) pauseMenuRoot.SetActive(paused);

        // Disable/enable right-stick turn actions while paused
        EnableTurn(!paused);

        if (value)
        {
            // Place immediately in front on open
            SnapMenuInFront();

            IngameView?.SetActive(false);
            if (VRPlayer) VRPlayer.enabled = false;
            if (Ray) Ray.maxRaycastDistance = 100;
        }
        else
        {
            IngameView?.SetActive(true);
            if (VRPlayer) VRPlayer.enabled = true;
            if (Ray) Ray.maxRaycastDistance = 1;
        }

        // Timer control
        if (timer)
        {
            if (paused) timer.PauseTimer();
            else timer.ResumeTimer();
        }

        // Enable/disable UI rays
        if (uiRays != null)
        {
            foreach (var ray in uiRays)
                if (ray) ray.enabled = paused;
        }
    }

    private void SnapMenuInFront()
    {
        if (!pauseMenuRoot || !head) return;

        Vector3 flatForward = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
        if (flatForward.sqrMagnitude < 0.0001f) flatForward = head.forward;

        Vector3 pos = head.position + flatForward * spawnDistance;
        pos.y = head.position.y + verticalOffset;

        pauseMenuRoot.transform.position = pos;
        pauseMenuRoot.transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
    }

    private void EnableTurn(bool enabled)
    {
        void Set(InputActionReference aref, bool en)
        {
            if (aref == null) return;
            var action = aref.action;
            if (action == null) return;

            if (en)
            {
                if (!action.enabled) action.Enable();
            }
            else
            {
                if (action.enabled) action.Disable();
            }
        }

        Set(continuousTurnAction, enabled);
        Set(snapTurnAction, enabled);
    }

    public void Retry() => SceneManager.LoadScene("Scene 1-Level 1");
    public void Exit() => SceneManager.LoadScene("Main Menu");
}
