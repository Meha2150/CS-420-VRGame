using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalTimeLoader : MonoBehaviour
{
    [SerializeField] string resultsSceneName = "Ending Screen";
    [SerializeField] UITimerContoller timer;  // assign in Inspector (recommended)
    public string playerTag = "";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (!timer) timer = FindFirstObjectByType<UITimerContoller>(FindObjectsInactive.Include);

            float finalSeconds;

            if (timer.mode == UITimerContoller.Mode.Countdown)
                finalSeconds = Mathf.Max(0f, timer.countdownStartSeconds - timer.CurrentSeconds());
            else
                finalSeconds = timer.CurrentSeconds(); // Stopwatch mode = elapsed

            if (RunResults.Instance) RunResults.Instance.lastRunSeconds = finalSeconds;

            SceneManager.LoadScene(resultsSceneName);
        }
    }
}
