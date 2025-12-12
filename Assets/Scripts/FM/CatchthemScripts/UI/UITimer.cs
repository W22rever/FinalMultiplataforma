using UnityEngine;
using TMPro;
public class UITimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private void OnEnable()
    {
        GameEvents.OnTimerChanged += UpdateTimer;
    }

    private void OnDisable()
    {
        GameEvents.OnTimerChanged -= UpdateTimer;
    }

    private void UpdateTimer(int secondsRemaining)
    {
        int minutes = secondsRemaining / 60;
        int seconds = secondsRemaining % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
