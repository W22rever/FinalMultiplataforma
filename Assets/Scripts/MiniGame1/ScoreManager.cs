using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private int _actualScore;
    private float _timer; 

    private void Awake()
    {
        _actualScore = 0;
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= 1f)
        {
            AddScore(1); 
            
            _timer -= 1f; 
        }
    }

    private void UpdateScore()
    {
        if (scoreText != null) 
            scoreText.text = _actualScore.ToString("D7");
    }

    public void AddScore(int score)
    {
        if(score < 0) score = 0;
        
        _actualScore += score;
        UpdateScore();
    }
    
    public int GetScore() => _actualScore;

    public void SubtractScore(int score)
    {
        if(score < 0) score = 0;
        _actualScore -= score;
        UpdateScore();
    }
}