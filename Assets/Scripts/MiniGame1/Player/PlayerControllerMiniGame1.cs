using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
public class PlayerControllerMiniGame1 : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float fireRate = 0.5f;

    private Vector2 _moveDirection;
    private bool _ispressed = false;
    private Rigidbody2D _rb;
    private Coroutine _activeFireCoroutine;
    
    private LifeSystem.LifeSystem _lifeSystem;
    private BulletPooling _bulletPooling;
    private SpriteRenderer _spriteRenderer;
    private UIManager _uiManager;
    private Camera _mainCamera; 

    //unity Events
    private void Awake()
    {
        _mainCamera = Camera.main;
        _uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bulletPooling = FindFirstObjectByType<BulletPooling>();
        _rb = GetComponent<Rigidbody2D>();
        _lifeSystem = GetComponent<LifeSystem.LifeSystem>();
    }

    private void Update()
    {
        if (_lifeSystem != null && _lifeSystem.currenthealth() <= 0)
        {
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
    }

    //input
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _ispressed = true;
            if (_activeFireCoroutine == null)
            {
                _activeFireCoroutine = StartCoroutine(FireBulletsRoutine());
            }
        }

        if (context.canceled)
        {
            _ispressed = false;
            if (_activeFireCoroutine != null)
            {
                StopCoroutine(_activeFireCoroutine);
                _activeFireCoroutine = null;
            }
        }
    }
    
    public void OnTouchPosition(InputAction.CallbackContext context) 
    {
        Vector2 screenPosition = context.ReadValue<Vector2>(); 
    
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;

        Debug.Log($"Pantalla: {screenPosition} -> Mundo: {worldPosition}");
        
        transform.position = worldPosition;
    }
    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveDirection * moveSpeed;
    }

    private IEnumerator FireBulletsRoutine()
    {
        while (_ispressed)
        {
            if (_bulletPooling != null)
            {
                _bulletPooling.SpawnBullet();
                AudioManager.AudioManager.Instance.PlaySound("PlayerGun");
            }
            
            yield return new WaitForSeconds(fireRate);
        }
        _activeFireCoroutine = null;
    }
    
    //Life System
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(TakingDamage());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _uiManager.YouLose();
    }

    private IEnumerator TakingDamage()
    {
        // Debug.Log("currentHealt" + _lifeSystem.currenthealth());
        _spriteRenderer.color = Color.red; 
        _lifeSystem.TakeDamage(1);
        AudioManager.AudioManager.Instance.PlaySound("DamagePlayer");
        
        
        yield return new WaitForSeconds(0.3f);
        
        _spriteRenderer.color = Color.white;
    }
}