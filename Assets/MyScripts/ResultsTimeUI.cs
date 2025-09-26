using TMPro;
using UnityEngine;

public class ResultsTimeUI : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    public RunResults RunResults;

    void Start()
    {
        float t = (RunResults.Instance ? RunResults.Instance.lastRunSeconds : 0f);
        timeText.text = FormatTime(t);
    }

    string FormatTime(float seconds)
    {
        int totalCenti = Mathf.Max(0, Mathf.FloorToInt(seconds * 100f));
        int centi = totalCenti % 100;
        int totalSecs = totalCenti / 100;
        int secs = totalSecs % 60;
        int mins = (totalSecs / 60) % 60;
        int hours = totalSecs / 3600;

        return hours > 0
            ? $"{hours:00}:{mins:00}:{secs:00}.{centi:00}"
            : $"{mins:00}:{secs:00}.{centi:00}";
    }
}
