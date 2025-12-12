using UnityEngine;

public class PoolItem : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    
    [HideInInspector] public SpawnerZone spawner;
    [HideInInspector] public GameObject originalPrefab;
    
    private Rigidbody2D _rb;
    private ItemInfo _info;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _info = GetComponent<ItemInfo>();
    } 
    
    private void OnEnable()
    {
        _rb.linearVelocity = new Vector2(0, -fallSpeed);
        GameEvents.OnGameStop += FreezeItem;
    }
    
    private void OnDisable()
    {
        GameEvents.OnGameStop -= FreezeItem;
    }
    
    
    private void OnBecameInvisible()
    {
        if (gameObject.activeInHierarchy) spawner.BackToPool(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Basket")) return;

        if (_info)
        {
            if (_info.isHazard) FindAnyObjectByType<GameManagerCG>().TakeDamage(1);
            else FindAnyObjectByType<GameManagerCG>().AddScore(_info.points);
            // Obtener el tipo del item
            if (CompareTag("Gift")) AudioManagerCG.Instance.PlaySFX(AudioManagerCG.Instance.giftSFX, 1.2f);

            if (CompareTag("Hazard")) AudioManagerCG.Instance.PlaySFX(AudioManagerCG.Instance.hazardSFX, 1.7f);
            
        }

        spawner.BackToPool(this);
    }
    
    
    private void FreezeItem()
    {
        if (_rb) _rb.linearVelocity = Vector2.zero;
    }
}
