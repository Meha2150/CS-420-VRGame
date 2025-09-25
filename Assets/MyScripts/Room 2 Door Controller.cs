using UnityEngine;

public class Room2DoorController : MonoBehaviour
{
    [SerializeField] public Animator LeftDoorAnimator;
    [SerializeField] public Animator RightDoorAnimator;
    [SerializeField] public LeverController[] LeverController;
    [SerializeField] public string SolveTrigger = "";

    private int CorrectLevers = 0;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < LeverController.Length; i++)
        {
            CorrectLevers += LeverController[i].solve;
        }
        if (CorrectLevers == LeverController.Length)
        {
            LeftDoorAnimator.SetTrigger(SolveTrigger);
            RightDoorAnimator.SetTrigger(SolveTrigger);
        }
    }
}
