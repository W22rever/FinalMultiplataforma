using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehaviour1 : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField, Range(1, 2)] private int typeOfEnemy = 1;
    [SerializeField] private float enemySpeed = 3f;
    [SerializeField] private float waitedTime = 2f;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Transform _targetPos;
    private LifeSystem.LifeSystem _lifeSystem;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        
        _lifeSystem = GetComponent<LifeSystem.LifeSystem>();
    }

    private void OnEnable()
    {
        _lifeSystem.ResetLife();
        _rb.gravityScale = 0;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            _targetPos = player.transform;

            if (typeOfEnemy == 1)
            {
                StartCoroutine(EnemyRoutineType1());
            }
        }
    }
    
    //Enemies Logic
    private IEnumerator EnemyRoutineType1()
    {
        if (_targetPos == null) yield break;

        Vector2 destination = new Vector2(_targetPos.position.x, 5);
        
        while (Vector2.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, enemySpeed * Time.deltaTime);
            yield return null; 
        }
        yield return new WaitForSeconds(waitedTime);
        _rb.gravityScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            StartCoroutine(TakingDamage());
        }
    }

    private void Update()
    {
        if (_lifeSystem != null && _lifeSystem.currenthealth() <= 0)
        {
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
            
    }

    private IEnumerator TakingDamage()
    {
        Debug.Log("currentHealt" + _lifeSystem.currenthealth());
        
        _spriteRenderer.color = Color.red; 
        _lifeSystem.TakeDamage(1);
        
        yield return new WaitForSeconds(0.3f);
        
        _spriteRenderer.color = Color.white;
    }
}