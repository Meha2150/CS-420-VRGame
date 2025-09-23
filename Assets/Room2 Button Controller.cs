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

    [Header("Interaction")]
    [Tooltip("If true, only XRRayInteractor (pointer) can press.")]
    public bool rayOnly = true;

    XRSimpleInteractable interactable;

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
    }

    void OnDestroy()
    {
        if (interactable) interactable.selectEntered.RemoveListener(OnSelectEntered);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Ray-only filter if desired
        if (rayOnly && !(args.interactorObject is XRRayInteractor)) return;

        if (puzzle) puzzle.HandleButtonPressed(buttonId);
    }
}
