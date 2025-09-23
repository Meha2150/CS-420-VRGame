using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Room1KeyController : MonoBehaviour
{
    [Header("Trigger filter")]
    [SerializeField] private string triggerTag = "Puzzle1Key";

    [Header("Thing to unlock")]
    [SerializeField] private GameObject target;    // set in Inspector
    private Rigidbody targetRb;
    private XRGrabInteractable targetGrab;
    [SerializeField] private GameObject key;

    private void Reset()
    {
        // Make sure this collider is a trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Awake()
    {
        if (target == null) target = gameObject; // default to self if you want

        targetRb = target.GetComponent<Rigidbody>();
        targetGrab = target.GetComponent<XRGrabInteractable>();

        if (!targetRb) Debug.LogError($"{name}: Target has no Rigidbody.");
        if (!targetGrab) Debug.LogError($"{name}: Target has no XRGrabInteractable.");
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggerTag)) return;
        if (targetRb) targetRb.isKinematic = false;
        if (targetGrab) targetGrab.enabled = true;
        Destroy(gameObject);
        Destroy(key);
    }
}
