using UnityEngine;
using UnityEngine.InputSystem;

public class PauseButton : MonoBehaviour
{
    public PauseManager pauseManager;
    public InputActionReference pauseAction; // reference to your Pause action

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPause;
            pauseAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPause;
            pauseAction.action.Disable();
        }
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        pauseManager.TogglePause();
    }
}
