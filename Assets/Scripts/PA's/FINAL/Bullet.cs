using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int bulletSpeed;
    [HideInInspector] public float distanceToReturn;
    private Vector3 _positionToReturn;
    private Vector3 _direction = Vector3.forward;
    private bool _isShooting;
    private int _initialSpeed;
    public static event Action<int> OnDestroyDuck;
    private int _ducksShooted;

    private void Start()
    {
        _positionToReturn = transform.position;
        ChangeScore();
    }
    
    public void Update()
    {
        if (!_isShooting) return;
     
        // Movimiento global (no dependiente del padre)
        //transform.Translate(_direction * (bulletSpeed * Time.deltaTime), Space.World);
        
        transform.position += _direction * (bulletSpeed * Time.deltaTime);
        
        if (transform.position.z > distanceToReturn)
        {
            _isShooting = false;
            bulletSpeed = 0;
            transform.position = _positionToReturn;
            gameObject.SetActive(false);
        }
    }
    
    public void ShootBullet(Vector3 startPosition)
    {
        // Actualiza la posición de inicio cada vez que dispare
        _positionToReturn = startPosition; // Guardamos la posición original de la bala para usarla como punto de retorno
        transform.position = startPosition; // Recolocamos la bala físicamente en su posición de inicio antes de disparar
        
        // Si es la primera vez que se dispara, guardamos la velocidad original
        if (_initialSpeed == 0)
            _initialSpeed = bulletSpeed;

        // Restauramos velocidad y activamos disparo
        bulletSpeed = _initialSpeed;  
        _isShooting = true;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Duck"))
        {
            Destroy(other.gameObject);
            Debug.Log("Duck eliminado");;
            AddScore(1);
        }
    }

    // Método para incrementar el puntaje
    private void AddScore(int score)
    {
        _ducksShooted += score;
        ChangeScore();
    }

    // Método donde realizamos el evento de cambio de puntaje
    private void ChangeScore()
    {
        OnDestroyDuck?.Invoke(_ducksShooted);
    }
}
