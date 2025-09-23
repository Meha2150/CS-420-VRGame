using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(HingeJoint), typeof(XRGrabInteractable))]
public class XRLeverDriver : MonoBehaviour
{
    public Transform handReference;      // auto-set while grabbed
    public Transform pivot;              // usually the handle itself or a child at the hinge anchor
    public Vector3 worldRotationAxis = Vector3.right;   // world-space axis to project onto

    public float minAngle = -45f;        // match Hinge limits
    public float maxAngle = 0f;

    HingeJoint hinge;
    XRGrabInteractable grab;

    void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        handReference = args.interactorObject?.transform;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        handReference = null;
    }

    void FixedUpdate()
    {
        if (handReference == null || pivot == null) return;

        // Vector from pivot to hand projected onto a plane perpendicular to the hinge axis.
        Vector3 axis = worldRotationAxis.normalized;
        Vector3 pivotToHand = handReference.position - pivot.position;

        // Build two directions that define the rotation plane
        Vector3 planeRight = Vector3.ProjectOnPlane(Vector3.right, axis).normalized;
        if (planeRight.sqrMagnitude < 0.01f) planeRight = Vector3.ProjectOnPlane(Vector3.forward, axis).normalized;
        Vector3 planeUp = Vector3.Cross(axis, planeRight).normalized;

        // Project hand direction onto that plane
        float x = Vector3.Dot(pivotToHand.normalized, planeRight);
        float y = Vector3.Dot(pivotToHand.normalized, planeUp);
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        // Remap angle into your hinge range (tune offset if needed)
        // You may add a calibration offset (e.g., 'angle -= 90f') depending on your geometry.
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // Drive the hinge spring target to that angle
        var spring = hinge.spring;
        spring.targetPosition = angle;
        hinge.spring = spring;
    }
}
