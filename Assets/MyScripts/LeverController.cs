using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("References")]
    public HingeJoint hinge;                 // assign the lever's HingeJoint
    public Animator leftDoorAnimator;        // drag in Inspector
    public Animator rightDoorAnimator;       // drag in Inspector

    [Header("Angle thresholds (deg)")]
    public float upThreshold = -29f;         // <= this => Up
    public float downThreshold = 29f;        // >= this => Down
    [Tooltip("Gap to prevent flicker near thresholds.")]
    public float hysteresis = 3f;

    public enum Target { Up = 1, Down = -1, Either = 0 }
    [Header("Solve when lever is...")]
    public Target orientation = Target.Up;

    [Header("Animator")]
    public string solveTrigger = "LeverCorrect";

    private enum State { Mid, Up, Down }
    private State last = State.Mid;

    void Reset()
    {
        hinge = GetComponent<HingeJoint>();  // only auto-fill hinge on same object
    }

    void FixedUpdate()
    {
        if (!hinge) return;

        float a = hinge.angle;

        // Apply hysteresis so it doesn't chatter at the boundary
        float upT = (last == State.Up) ? upThreshold + hysteresis : upThreshold;
        float downT = (last == State.Down) ? downThreshold - hysteresis : downThreshold;

        State cur = State.Mid;
        if (a <= upT) cur = State.Up;
        else if (a >= downT) cur = State.Down;

        if (cur == last) return; // state didn't change

        // Fire only when entering Up/Down (not Mid), and only if it matches the target
        if (cur != State.Mid && IsCorrect(cur))
            FireSolve();

        last = cur;
    }

    bool IsCorrect(State cur)
    {
        if (orientation == Target.Either) return true;
        if (orientation == Target.Up) return cur == State.Up;
        if (orientation == Target.Down) return cur == State.Down;
        return false;
    }

    void FireSolve()
    {
        if (leftDoorAnimator) leftDoorAnimator.SetTrigger(solveTrigger);
        if (rightDoorAnimator) rightDoorAnimator.SetTrigger(solveTrigger);
    }
}
