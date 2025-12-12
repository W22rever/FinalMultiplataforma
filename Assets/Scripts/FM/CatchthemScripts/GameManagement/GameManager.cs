using UnityEngine;

namespace FM.CatchthemScripts.GameManagement
{
    

public enum GameState
{
    Playing,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Life")]
    [SerializeField] private int maxLife = 3;
    private int _life;
    private int _currentScore;
    public int Score => _currentScore;

    [Header("Timer")]
    [SerializeField] private int startTimeSeconds = 60;
    private float _timer;

    public GameState state = GameState.Playing;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        _life = maxLife;
        _timer = startTimeSeconds;
        GameEvents.OnLifeChanged?.Invoke(_life);
        GameEvents.OnTimerChanged?.Invoke(startTimeSeconds);
    }

    private void Update()
    {
        if (state != GameState.Playing) return;

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _timer -= Time.deltaTime;

        int timeInt = Mathf.Clamp(Mathf.FloorToInt(_timer), 0, startTimeSeconds);
        GameEvents.OnTimerChanged?.Invoke(timeInt);

        if (timeInt <= 0)
        {
            EndGame("time");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.timeUpSFX,0.7f);
        }
    }

    public void AddScore(int amount)
    {
        _currentScore += amount;
        GameEvents.OnScoreAdded?.Invoke(_currentScore);
    }

    public void TakeDamage(int amount)
    {
        _life -= amount;
        _life = Mathf.Clamp(_life, 0, maxLife);

        GameEvents.OnLifeChanged?.Invoke(_life);

        if (_life <= 0)
        {
            EndGame("life");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.deathSFX, 0.6f);
        }
    }
    
    private void EndGame(string reason)
    {
        state = GameState.GameOver;

        // Detiene el gameplay
        GameEvents.OnGameStop?.Invoke();

        // Activa panel final y envía razón
        GameEvents.OnEndGame?.Invoke(reason);
    }
    
}
}