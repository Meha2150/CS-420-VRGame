using UnityEngine;

public class Room2DoorController : MonoBehaviour
{
    [SerializeField] public Animator LeftDoorAnimator;
    [SerializeField] public Animator RightDoorAnimator;
    [SerializeField] public LeverController[] LeverController;
    [SerializeField] public string SolveTrigger = "";
    [SerializeField] private AudioSource triggerSound;

    private int CorrectLevers = 0;
    
    void Start()
    {
        CorrectLevers = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (CorrectLevers < 4) 
        {
            CorrectLevers = LeverController[0].solve + LeverController[1].solve + LeverController[2].solve + LeverController[3].solve;
        }
        if (CorrectLevers == LeverController.Length)
        {
            Debug.Log("Doors open");
            LeftDoorAnimator.SetTrigger(SolveTrigger);
            RightDoorAnimator.SetTrigger(SolveTrigger);
            triggerSound.enabled = true;
            CorrectLevers++;
        }
    }
}
