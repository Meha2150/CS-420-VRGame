using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static UnityEngine.GraphicsBuffer;

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
    public Animator animator;
    public string solvedTrigger = "";
    
    [SerializeField] private GameObject Crown;
    private Rigidbody CrownRb;
    private XRGrabInteractable CrownGrab;

   
    public void HandleButtonPressed(int buttonId)
    {
        if (solved && playOnce) return; // ignore presses after solve if locked

        // Expected next id
        int expected = order != null && order.Length > 0 && progress < order.Length
            ? order[progress]
            : int.MinValue;
        CrownRb = Crown.GetComponent<Rigidbody>();
        CrownGrab = Crown.GetComponent<XRGrabInteractable>();
        if (buttonId == expected)
        {
            progress++;
            

            if (progress >= order.Length)
            {
                solved = true; 
                if (animator && !string.IsNullOrEmpty(solvedTrigger)) return;
                
                   animator.SetTrigger(solvedTrigger);
                   CrownRb.isKinematic = false;
                   CrownGrab.enabled = true;
                
            }
        }
        else
        {
            
            if (resetDelayOnWrong <= 0f)
            {
                ResetProgress();
            }
            else
            {
                // Optional small delay (e.g., to play a buzz sound/light)
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
