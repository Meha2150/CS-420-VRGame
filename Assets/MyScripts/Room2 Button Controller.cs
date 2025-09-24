using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(Collider))]
public class Room2ButtonController : MonoBehaviour
{
    [Header("Setup")]
    public Room2ButtonOrder puzzle;   // auto-filled from parent if left empty
    [Tooltip("Unique id for this button. Use these ids in the puzzle's 'order'.")]
    public int buttonId = 0;

    [Header("Cap Movement")]
    [Tooltip("What to move when pressed. Leave empty to move this object.")]
    [SerializeField] private Transform cap;
    [Tooltip("Local offset to move when pressed (e.g., slightly back).")]
    [SerializeField] private Vector3 localPressOffset = new Vector3(0f, 0f, -0.02f);
    [Tooltip("Seconds to move into pressed position.")]
    [SerializeField] private float pressDuration = 0.05f;
    [Tooltip("How long to stay pressed before returning.")]
    [SerializeField] private float holdTime = 1.0f;
    [Tooltip("Seconds to move back to original position.")]
    [SerializeField] private float returnDuration = 0.08f;

    private XRSimpleInteractable interactable;
    private Vector3 _startLocalPos;
    private bool _animating;
    private Coroutine _pressRoutine;

    void Reset()
    {
        // Default collider settings for ray
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = false;
    }

    void Awake()
    {
        if (!puzzle) puzzle = GetComponentInParent<Room2ButtonOrder>();

        interactable = GetComponent<XRSimpleInteractable>();
        if (!interactable) interactable = gameObject.AddComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);

        if (!cap) cap = transform; // move self if no cap assigned
        _startLocalPos = cap.localPosition;
    }

    void OnDestroy()
    {
        if (interactable) interactable.selectEntered.RemoveListener(OnSelectEntered);
    }

    void OnDisable()
    {
        // Ensure it snaps back if object is disabled during animation
        if (cap) cap.localPosition = _startLocalPos;
        _animating = false;
        if (_pressRoutine != null) StopCoroutine(_pressRoutine);
        _pressRoutine = null;
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Kick the visual press (non-blocking)
        if (!_animating && cap) _pressRoutine = StartCoroutine(PressVisual());

        // Notify puzzle logic
        if (puzzle) puzzle.HandleButtonPressed(buttonId);
    }

    private IEnumerator PressVisual()
    {
        _animating = true;

        Vector3 pressedPos = _startLocalPos + localPressOffset;

        // Move to pressed
        yield return MoveLocal(cap, _startLocalPos, pressedPos, pressDuration);
        // Hold
        if (holdTime > 0f) yield return new WaitForSeconds(holdTime);
        // Return
        yield return MoveLocal(cap, pressedPos, _startLocalPos, returnDuration);

        _animating = false;
        _pressRoutine = null;
    }

    private IEnumerator MoveLocal(Transform t, Vector3 from, Vector3 to, float dur)
    {
        if (dur <= 0f)
        {
            t.localPosition = to;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < dur)
        {
            elapsed += Time.deltaTime;
            float k = Mathf.Clamp01(elapsed / dur);
            t.localPosition = Vector3.Lerp(from, to, k);
            yield return null;
        }
        t.localPosition = to;
    }
}
