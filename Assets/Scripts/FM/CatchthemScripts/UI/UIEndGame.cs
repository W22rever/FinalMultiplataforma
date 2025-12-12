using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIEndGame : MonoBehaviour
{
    [Header("Panel Final")]
    [SerializeField] private GameObject endPanel;
    
    [Header("UI Elements")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text finalScoreText;

    [Header("Buttons")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    private void OnEnable()
    {
        GameEvents.OnEndGame += ShowEndGame;

        retryButton.onClick.AddListener(RestartLevel);
        menuButton.onClick.AddListener(BackToMenu);
    }

    private void OnDisable()
    {
        GameEvents.OnEndGame -= ShowEndGame;

        retryButton.onClick.RemoveListener(RestartLevel);
        menuButton.onClick.RemoveListener(BackToMenu);
    }
    
    private void Start()
    {
        endPanel.SetActive(false); // esconder panel al inicio
    }

    private void ShowEndGame(string reason)
    {
        endPanel.SetActive(true);

        if (reason == "life")
            titleText.text = "NOOOOOOO SANTAAAAAA!!!";
        else
            titleText.text = "TIME'S OVER! CONGRATULATIONS!";

        // Mostrar score real sin ceros
        finalScoreText.text = $"TOTAL SCORE: {GameManagerCG.Instance.Score.ToString()}";
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("Menu"); 
    }
}
