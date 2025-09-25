using UnityEngine;
using UnityEngine.Rendering;

public class Room3StairContoller : MonoBehaviour
{
    [SerializeField] private Animator stairAnimator; // drag the DOOR's Animator here
    private string  Move = "MoveStairs";
    public Room3PlateBController Room3PlateBControllerRefernce;
    public Room3PlateCController room3PlateCControllerRefernce;

    private int platesDowns = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (platesDowns <= 2)
        {
            platesDowns = Room3PlateBControllerRefernce.platesDown + room3PlateCControllerRefernce.platesDown;
        }
        
        if (platesDowns == 2)
        {
            Debug.Log("Stairs have moved");
            stairAnimator.SetTrigger(Move);
            platesDowns++;
        }
    }
}
