using UnityEngine;
using UnityEngine.Events;

public class LeverPuzzleController : MonoBehaviour
{
    public enum Zone { Up, Mid, Down }

    [System.Serializable]
    public class Lever
    {
        [Header("Lever reference (pick one)")]
        public HingeJoint hinge;           // if physics lever
        public Transform handle;           // else read from this transform
        [Tooltip("Handle’s local axis that points forward in the REST pose.")]
        public Vector3 localAxis = Vector3.right;

        [Header("Angle thresholds (deg)")]
        [Tooltip("Angle >= this is considered UP")]
        public float upZoneMin = -10f;
        [Tooltip("Angle <= this is considered DOWN")]
        public float downZoneMax = -40f;

        [Header("Required orientation")]
        public Zone required = Zone.Down;

        // Optional: which world axis to measure around if not using hinge
        public Vector3 signedAngleAxis = Vector3.up;
    }

    [Header("Levers to watch")]
    public Lever[] levers;

    [Header("Solve condition")]
    [Tooltip("How long all levers must remain correct before solving.")]
    public float holdSeconds = 0.15f;
    [Tooltip("If true, the animation won’t retrigger after the first solve.")]
    public bool playOnce = true;

    [Header("Animation / Events")]
    public Animator animator;
    public string solvedTrigger = "Play";
    

    float solvedTimer = 0f;
    bool isSolved = false;
    bool hasPlayed = false;

    void FixedUpdate() // stable with hinge.angle
    {
        bool allGood = true;
        for (int i = 0; i < levers.Length; i++)
        {
            var L = levers[i];
            Zone z = GetZone(GetAngle(L), L.upZoneMin, L.downZoneMax);
            if (z != L.required) { allGood = false; break; }
        }

        if (allGood)
        {
            solvedTimer += Time.fixedDeltaTime;
            if (!isSolved && solvedTimer >= holdSeconds)
            {
                isSolved = true;
                if (!hasPlayed || !playOnce)
                {
                    if (animator && !string.IsNullOrEmpty(solvedTrigger))
                        animator.SetTrigger(solvedTrigger);
                    onSolved?.Invoke();
                    hasPlayed = true;
                }
            }
        }
        else
        {
            if (isSolved) onUnsolved?.Invoke();
            isSolved = false;
            solvedTimer = 0f;
        }
    }

    // -------- helpers --------
    static Zone GetZone(float angle, float upMin, float downMax)
    {
        if (angle >= upMin) return Zone.Up;
        if (angle <= downMax) return Zone.Down;
        return Zone.Mid;
    }

    static float GetAngle(Lever L)
    {
        if (L.hinge) return L.hinge.angle;

        Transform t = L.handle;
        if (!t) return 0f;

        // Compare parent forward vs handle’s chosen axis
        Vector3 refDir = t.parent ? t.parent.forward : Vector3.forward;
        Vector3 handleDir = t.TransformDirection(L.localAxis).normalized;
        return Vector3.SignedAngle(refDir, handleDir, L.signedAngleAxis);
    }
}
