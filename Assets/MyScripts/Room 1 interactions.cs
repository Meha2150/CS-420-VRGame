using UnityEngine;

public class Room1interactions : MonoBehaviour
{
    [SerializeField] private Animator exitDoorAnimator;

    void Start()
    {
       
            exitDoorAnimator = GetComponent<Animator>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PuzzleEnd")
        {
            // Play the "Room1DoorOpen" animation state on the exit door animator
            if (exitDoorAnimator != null)
            {
                if (Input.GetKeyUp(KeyCode.P))
                {
                    exitDoorAnimator.SetTrigger("PressurePlateDown");
                    Debug.Log("Pressure Plate Triggered");
                    // Set the trigger to activate the transition
                    //exitDoorAnimator.SetTrigger("Pressure Plate Down");
                }
            }
        }
    }
  
    void Update()
    {
            if (exitDoorAnimator != null)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    // Set the trigger to activate the transition
                    exitDoorAnimator.SetTrigger("PressurePlateDown");
            }
        }
    }
}
