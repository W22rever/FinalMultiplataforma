using UnityEngine;

public class EnemyPooling : GenericPool<Enemy>
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(8f, 2f);
    [SerializeField] private Color gizmoColor = Color.green;

    protected override void OnTakeFromPool(Enemy item)
    {
        base.OnTakeFromPool(item);
        item.SetPool(Pool);
        
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        
        Vector2 randomPosition = (Vector2)transform.position + new Vector2(randomX, randomY);
        item.transform.position = randomPosition;
    }

    public void SpawnEnemy()
    {
        Pool.Get();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }

    public float SpawnerInterval()
    {
        return spawnInterval;
    }
}