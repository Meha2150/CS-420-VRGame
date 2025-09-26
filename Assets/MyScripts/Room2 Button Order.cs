using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Room2ButtonOrder : MonoBehaviour
{
    [Header("Sequence")]
    [Tooltip("The required button IDs in order (must match XRSequenceButton.buttonId values).")]
    public int[] order;

    [Header("State")]
    [Tooltip("Current progress (# of correct presses so far).")]
    public int progress = 0;                 // increments on correct, resets on wrong
    public bool solved { get; private set; } // true when finished

    [Header("Behavior")]
    public bool playOnce = true;             // lock puzzle after first solve
    public float resetDelayOnWrong = 0f;     // optional delay before resetting progress

    [Header("Animation")]
    [Tooltip("Animator that contains the 'solved' animation/state.")]
    public Animator animator;
    [Tooltip("Name of the Trigger parameter on the Animator to fire when solved.")]
    public string solvedTrigger = "";

   
    [SerializeField] private GameObject Crown;
    private Rigidbody CrownRb;
    private XRGrabInteractable CrownGrab;
    [SerializeField]private AudioSource triggerSound;

    private bool _animTriggerExists;

    private void Awake()
    {
        

        if (Crown != null)
        {
            CrownRb = Crown.GetComponent<Rigidbody>();
            CrownGrab = Crown.GetComponent<XRGrabInteractable>();
            
        }

        ValidateAnimatorAndTrigger();
        ValidateOrder();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Runs in editor when values change—helps catch typos early.
        ValidateAnimatorAndTrigger();
    }
#endif

    private void ValidateAnimatorAndTrigger()
    {
        _animTriggerExists = false;

        
        

        // Check that the Animator has a Trigger parameter with this exact name
        foreach (var p in animator.parameters)
        {
            if (p.type == AnimatorControllerParameterType.Trigger && p.name == solvedTrigger)
            {
                _animTriggerExists = true;
                break;
            }
        }

        
           
    }

    private void ValidateOrder()
    {
        
            
    }

    public void HandleButtonPressed(int buttonId)
    {
        if (solved && playOnce)
        {
            // Ignore presses after solve if locked
            return;
        }

        // Compute expected next id safely
        bool canAdvance = (order != null && order.Length > 0 && progress < order.Length);
        int expected = canAdvance ? order[progress] : int.MinValue;

        if (buttonId == expected)
        {
            progress++;
            // Optional: Debug log to help confirm flow
            // Debug.Log($"{name}: Correct press {buttonId}. Progress {progress}/{order.Length}");

            if (progress >= order.Length)
            {
                solved = true;

                // Always drop/enable the crown when solved (not gated by animation trigger)
                if (CrownRb != null) CrownRb.isKinematic = false;
                if (CrownGrab != null) CrownGrab.enabled = true;

                // Fire the animation trigger if properly set
                if (animator != null && _animTriggerExists)
                {
                    animator.ResetTrigger(solvedTrigger); // avoid sticky trigger
                    animator.SetTrigger(solvedTrigger);
                    triggerSound.enabled = true;
                }
                else
                {
                    
                }
            }
        }
        else
        {
            // Debug.Log($"{name}: Wrong press {buttonId}. Expected {expected}. Resetting after delay {resetDelayOnWrong}s");

            if (resetDelayOnWrong <= 0f)
            {
                ResetProgress();
            }
            else
            {
                CancelInvoke(nameof(ResetProgress));
                Invoke(nameof(ResetProgress), resetDelayOnWrong);
            }
        }
    }

    public void ResetProgress()
    {
        progress = 0;
        if (solved && !playOnce) solved = false;
    }
}
