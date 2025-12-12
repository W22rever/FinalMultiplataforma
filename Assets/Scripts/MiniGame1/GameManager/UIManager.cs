using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    [Header("UI Parameters")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private ScoreManager scoreManager;
    

    public void YouLose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
        AudioManager.AudioManager.Instance.StopMusic();
        finalScore.text = scoreManager.GetScore().ToString("D7");
    }
    
}