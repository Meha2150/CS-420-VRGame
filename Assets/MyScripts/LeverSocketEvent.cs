using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LeverSocketEvents : MonoBehaviour
{
    public XRSocketInteractor socket;
    public Behaviour leverController;  // your lever script or Animator

    void OnEnable()
    {
        socket.selectEntered.AddListener(OnSnap);
        socket.selectExited.AddListener(OnUnsnap);
    }
    void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnSnap);
        socket.selectExited.RemoveListener(OnUnsnap);
    }

    void OnSnap(SelectEnterEventArgs args)
    {
        if (leverController) leverController.enabled = true;
    }

    void OnUnsnap(SelectExitEventArgs args)
    {
        if (leverController) leverController.enabled = false;
    }
}
