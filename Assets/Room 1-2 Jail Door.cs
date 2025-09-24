using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Room2JailDoor : MonoBehaviour


{
    [SerializeField] private Animator DoorAnimator; // drag the DOOR's Animator here
    private static readonly int Jail = Animator.StringToHash("OpenJail");
    [SerializeField] private string triggerTag = "PuzzleEnd";

    // Track how many qualifying objects are on the plate (handles multiple colliders)
    

    private void Awake()
    {
        var col = GetComponent<Collider>();
        if (!col.isTrigger) Debug.LogWarning($"{name}: Collider is not set as Trigger.");
        if (DoorAnimator == null) Debug.LogError($"{name}: ExitDoorAnimator is NOT assigned.");
    }

    private bool MatchesTag(Collider other)
    {
        return other.CompareTag(triggerTag) ||
               (other.attachedRigidbody && other.attachedRigidbody.CompareTag(triggerTag)) ||
               other.transform.root.CompareTag(triggerTag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!MatchesTag(other) || DoorAnimator == null) return;

        DoorAnimator.ResetTrigger(Jail);
        DoorAnimator.SetTrigger(Jail);
    }

    

    // Optional manual test: press P to open, O to close
    private void Update()
    {
        if (DoorAnimator == null) return;
        if (Input.GetKeyDown(KeyCode.G)) { DoorAnimator.SetTrigger(Jail); }
        
    }
}

