using UnityEngine;
using UnityEngine.Rendering;

public class Room3StairContoller : MonoBehaviour
{
    [SerializeField] private Animator stairrAnimator; // drag the DOOR's Animator here
    private static readonly int Move = Animator.StringToHash("MoveStairs");
    public Room3PlateBController Room3PlateBControllerRefernce;
    public Room3PlateCController room3PlateCControllerRefernce;

    private int platesDowns = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        platesDowns = Room3PlateBControllerRefernce.platesDown + room3PlateCControllerRefernce.platesDown;
        if (platesDowns == 2)
        {
            stairrAnimator.SetTrigger(Move);
        }
    }
}
