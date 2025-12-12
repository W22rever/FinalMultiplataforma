using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class Proyectile : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private string targetTag;    
    
    private Rigidbody2D _rb;
    private GameObject _player;
    private IObjectPool<Proyectile> _pool;

    // VARIABLE NUEVA: Esta es nuestro "seguro"
    private bool _isReleased;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetPool(IObjectPool<Proyectile> pool)
    {
        _pool = pool;
    }

    // Cada vez que la bala sale del pool (se activa), reseteamos el seguro
    private void OnEnable()
    {
        _isReleased = false;
    }

    private void FixedUpdate()
    {
        // Pequeña optimización: Si ya se está devolviendo, no la muevas
        if (_isReleased) return; 
        
        _rb.linearVelocity = transform.up * speed;
    }

    // FUNCIÓN SEGURA PARA DEVOLVER AL POOL
    private void ReturnToPool()
    {
        // Si ya la devolvimos (_isReleased es true) o no hay pool, cancelamos.
        if (_isReleased || _pool == null) return;

        _isReleased = true; // Marcamos que ya se va
        _pool.Release(this);
    }

    private void OnBecameInvisible()
    {
        // Si el objeto ya está desactivado (porque chocó), no ejecutamos esto
        if (!gameObject.activeInHierarchy) return;

        ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si ya chocó con algo en este mismo frame, ignoramos choques extra
        if (_isReleased) return;

        if (other.CompareTag(targetTag))
        {
            ReturnToPool();
        }
    }

    private void Update()
    {
        if (_player == null || !_player.activeInHierarchy)
        {
            return; 
        }
    }
}