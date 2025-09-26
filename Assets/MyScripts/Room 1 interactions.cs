using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Room1Interactions : MonoBehaviour
{
    [SerializeField] private Animator exitDoorAnimator; // drag the DOOR's Animator here
    private static readonly int Down = Animator.StringToHash("PressurePlateDown");
    private static readonly int Up = Animator.StringToHash("PressurePlateUp");
    [SerializeField] private string triggerTag = "PuzzleEnd";
    [SerializeField] private GameObject audioTrigger;
    private AudioSource triggerSound;

    // Track how many qualifying objects are on the plate (handles multiple colliders)
    private int occupants = 0;

    private void Awake()
    {
        var col = GetComponent<Collider>();
        if (!col.isTrigger) Debug.LogWarning($"{name}: Collider is not set as Trigger.");
        if (exitDoorAnimator == null) Debug.LogError($"{name}: ExitDoorAnimator is NOT assigned.");
        triggerSound = audioTrigger.GetComponent<AudioSource>();
    }

    private bool MatchesTag(Collider other)
    {
        return other.CompareTag(triggerTag) ||
               (other.attachedRigidbody && other.attachedRigidbody.CompareTag(triggerTag)) ||
               other.transform.root.CompareTag(triggerTag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!MatchesTag(other) || exitDoorAnimator == null) return;
        triggerSound.enabled = true;
        if (occupants++ == 0)
        {
            exitDoorAnimator.ResetTrigger(Up);
            exitDoorAnimator.SetTrigger(Down);   // OPEN
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!MatchesTag(other) || exitDoorAnimator == null) return;

        if (--occupants <= 0)
        {
            occupants = 0;
            exitDoorAnimator.ResetTrigger(Down);
            exitDoorAnimator.SetTrigger(Up);     // CLOSE
            
        }
    }

    // Optional manual test: press P to open, O to close
    private void Update()
    {
        if (exitDoorAnimator == null) return;
        if (Input.GetKeyDown(KeyCode.P)) { exitDoorAnimator.SetTrigger(Down); }
        if (Input.GetKeyDown(KeyCode.O)) { exitDoorAnimator.SetTrigger(Up); }
    }
}
