using System;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class Proyectile : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private string targetTag;    
    
    private Rigidbody2D _rb;
    private IObjectPool<Proyectile> _pool;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetPool(IObjectPool<Proyectile> pool)
    {
        _pool = pool;
    }

    private void FixedUpdate()
    {
            _rb.linearVelocity = transform.up * speed;
    }

    private void OnBecameInvisible()
    {
        if (_pool != null)
        {
            _pool.Release(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == targetTag)
        {
            other.gameObject.SetActive(false);
            _pool.Release(this);
        }

    }
}