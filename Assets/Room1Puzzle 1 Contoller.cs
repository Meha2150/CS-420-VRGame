using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Collider))]
public class Room1Puzzle1Controller : MonoBehaviour
{
    [Header("Trigger filter")]
    [SerializeField] private string triggerTag = "Puzzle1Food"; 

    [Header("Thing to unlock")]
    [SerializeField] private GameObject target;    // set in Inspector
    private Rigidbody targetRb;
    private XRGrabInteractable targetGrab;

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
        Debug.Log("Script Starting");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggerTag)) return;
        if (targetRb) targetRb.isKinematic = false;
        if (targetGrab) targetGrab.enabled = true;
        Debug.Log("Key is unlocked");
    }

    
}
