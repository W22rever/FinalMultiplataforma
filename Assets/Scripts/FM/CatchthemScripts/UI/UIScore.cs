using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void OnEnable()
    {
        GameEvents.OnScoreAdded += UpdateScore;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreAdded -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = "SCORE: " + score.ToString("00000000");
    }
}
