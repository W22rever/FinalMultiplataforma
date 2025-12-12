using System;
using UnityEngine;

public partial class Bullet : MonoBehaviour
{
    [HideInInspector] public int bulletSpeed;
    [HideInInspector] public float distanceToReturn;
    private Vector3 _positionToReturn;
    private Vector3 _direction = Vector3.forward;
    private bool _isShooting;
    private int _initialSpeed;
    public static event Action<int> OnDestroyDuck;
    public static Action ResetScore;
    private int _ducksShooted;
    
    private AudioManager _audioManager;
    private GameManager _gameManager;
    private Gun _gun;
    
    [SerializeField] private PlusScoreUI scorePrefab;

    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gun = gameObject.GetComponentInParent<Gun>();
    } 
    
    private void Start()
    {
        _positionToReturn = transform.position;
        ChangeScore();
        
        
        // Esto garantiza que el score vuelve a 0 visualmente y en lógica
        ResetScore += () =>
        {
            _ducksShooted = 0;
            ChangeScore();
        };
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
        if (_initialSpeed == 0) _initialSpeed = bulletSpeed;

        // Restauramos velocidad y activamos disparo
        bulletSpeed = _initialSpeed;  
        _isShooting = true;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Duck"))
        {
            // Instanciar el prefab en posición del pato
            Vector3 spawnPos = other.transform.position;

            PlusScoreUI ui = Instantiate(scorePrefab, spawnPos, Quaternion.identity);
            
            // Con el método PlayClipAtPoint Unity crea un componente temporal que nos permite ejecutar un sonido puntual
            // sin necesidad de agregar un componente audioSource. Ideal para hits, explosiones, etc.
            //AudioSource.PlayClipAtPoint(doveSound, other.transform.position, 1f);
            _audioManager.SFXDove.PlayOneShot(_audioManager.SFXDove.clip, 1);
            
            // Ejecutar su animación
            ui.StartAppear();
            AddScore(1);
            other.gameObject.SetActive(false);
            //Debug.Log("Duck eliminado");

            if (_ducksShooted >= _gun.TotalDucks)
            {
                _gameManager.Victory();
                _gun.ResetGun();
            }
        } 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Table"))
        {
            _isShooting = false;
            bulletSpeed = 0;
            transform.position = _positionToReturn;
            gameObject.SetActive(false);
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
