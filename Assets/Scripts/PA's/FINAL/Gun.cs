using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    [SerializeField] private float speedToReturn;
    [SerializeField] private float heightLimit;
    [SerializeField] private float paddingX;
    [SerializeField] private int timeToStart = 3;

    private float _timer;
    private float _limitX;
    private float _limitYBottom;
    private Vector3 _posEdgeWorld; 
    private Vector3 _directionX = Vector3.right;
    private Vector3 _directionY = Vector3.up;
    private Vector3 _position; // manipulación de movimiento
    private Vector3 _startPosition;
    private bool _isMovingY;
    private bool _stopGun;
    private bool _canShot = true;
    private bool _isReturning;
    
    [Header("Bullet Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private int bulletSpeed;
    [SerializeField] private float distanceToReturn;

    private Bullet _bulletScript;
    
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int totalDucks = 20;
    
    private void Awake()
    {
        _bulletScript = bullet.GetComponent<Bullet>();
        _bulletScript.bulletSpeed = bulletSpeed;
        _bulletScript.distanceToReturn = distanceToReturn;
        _startPosition = transform.position;
        _limitYBottom = transform.position.y;
    }

    private void Start()
    {
        _position = transform.position;
        float distanceToCamera = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
        _posEdgeWorld = Camera.main.ViewportToWorldPoint(new Vector3(1,1,distanceToCamera));
        _limitX = _posEdgeWorld.x - paddingX;
        _posEdgeWorld.y = heightLimit;
    }
    private void Update()
    {
        // si está volviendo a su punto de inicio, no hacer nada más
        if (_isReturning) return;
        
        _timer += Time.deltaTime;
        if (_timer > timeToStart)
        {
            if (!_isMovingY)
            {
                MoveGunX();
                if (Input.GetKeyDown("space"))
                {
                    _isMovingY = true;    
                }
            }
            else
            {
                if (!_stopGun)
                {
                    MoveGunY();
                    if (Input.GetKeyDown("space"))
                    {
                        _stopGun = true;    
                    }    
                }
                else
                {
                    if (_canShot) // Aseguramos que solo se ejecute una vez y no en cada frame
                    {
                        _bulletScript.ShootBullet(bullet.transform.position);
                        _canShot = false; // desactiva el disparo mientras la bala está activa
                    } else if (!bullet.activeSelf) // si ya se desactivó
                    {
                        StartCoroutine(ReturnToStart());
                    }
                }
            }
        }
    }
    private void MoveGunX()
    {
        _position += _directionX * (speedX * Time.deltaTime);
            
        if (transform.position.x >= _limitX) _directionX = Vector3.left;
        else if (transform.position.x <= -_limitX) _directionX = Vector3.right;
            
        transform.position = _position;
    }

    private void MoveGunY()
    {
        _position += _directionY * (speedY * Time.deltaTime);
        
        if(transform.position.y >= _posEdgeWorld.y) _directionY = Vector3.down;
        else if (transform.position.y <= _limitYBottom) _directionY = Vector3.up;
        
        transform.position = _position;
    }

    private IEnumerator ReturnToStart()
    {
        _isReturning = true; // evita que Update siga moviendo o dispare
        // mientras no haya llegado a la posición inicial...
        while (Vector3.Distance(transform.position, _startPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _startPosition,
                speedToReturn * Time.deltaTime
            );
            yield return null; // espera un frame y sigue moviendo
        }
        // Sincronizar variables
        _position = _startPosition;
        
        // cuando llega, restablece el estado
        _canShot = true;
        _isMovingY = false;
        _stopGun = false;
        _isReturning = false;
    }

    private void OnEnable()
    {
        Bullet.OnDestroyDuck += UpdateScore;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = $"DUCKS: {score}/{totalDucks}";
    }
}
