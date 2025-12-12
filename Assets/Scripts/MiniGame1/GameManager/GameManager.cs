    
using System.Collections;
using System.Collections.Generic;
using MiniGame1;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private List<EnemyPooling> enemyPool;
    
    private ExploPooling _exploPool;
    private GameObject _player;
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _scoreManager = GetComponent<ScoreManager>();
        _exploPool = FindFirstObjectByType<ExploPooling>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            enemyPool[0].SpawnEnemy();
            yield return new WaitForSeconds(enemyPool[0].SpawnerInterval());
            
            enemyPool[1].SpawnEnemy();
            yield return new WaitForSeconds(enemyPool[1].SpawnerInterval());
            if (_player.activeSelf == false)
            {
                yield break;
            }
        }
    }
    
    // explotion logic
    private void OnEnable()
    {
        //surcibir evento
        Enemy.OnEnemyDeactivated += SpawnReward;
    }

    private void OnDisable()
    {
        //Desuscribir evento
        Enemy.OnEnemyDeactivated -= SpawnReward;
    }

    private void SpawnReward(Transform enemyPos)
    {
        _scoreManager.AddScore(10);
         // Debug.Log($"¡Enemigo eliminado en la posición: {enemyPos}!");

        _exploPool.SpawnExplotion(enemyPos);
        AudioManager.AudioManager.Instance.PlaySound("Explotion");

        
    }
}
