using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("Hinge")]
    public HingeJoint hinge;          // assign the lever's HingeJoint

    [Header("Thresholds (deg)")]
    public float upThreshold = -29f; // <= this => Up
    public float downThreshold = 29f; // >= this => Down
   

    [Header("Solve when lever is...")]
    [Tooltip("1 = Up, -1 = Down, 0 = Either")]
    public int orientation = 0;

    

    float prevAngle;
    bool initialized;
    public int solve = 0;

    void Reset()
    {
        hinge = GetComponent<HingeJoint>(); // only auto-fill hinge
    }

    void FixedUpdate()
    {
        if (!hinge) return;

        float a = hinge.angle;

        // Initialize previous angle on first tick
        if (!initialized) { prevAngle = a; initialized = true; return; }

        // Crossing detection (no state machine)
        bool crossedIntoUp =  (a <= upThreshold);
        bool crossedIntoDown = (a >= downThreshold);

        if (ShouldFire(crossedIntoUp, crossedIntoDown))
            Fire();

        prevAngle = a;
    }

    bool ShouldFire(bool crossedUp, bool crossedDown)
    {
        if (orientation == 1) return crossedUp;
        if (orientation == -1) return crossedDown;
        /* orientation == 0 (Either) */
        return crossedUp || crossedDown;
    }

    void Fire()
    {
        
        solve = 1;
    }
}
