using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

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
    
    public int TotalDucks => totalDucks;
    
    [Header("SFX")]
    //[SerializeField] private AudioClip shotSFX;
    
    //GAME MANAGER
    private GameManager _gameManager;
    
    //AUDIO MANAGER
    private AudioManager _audioManager;
    
    private void Awake()
    {
        _bulletScript = bullet.GetComponent<Bullet>();
        _bulletScript.bulletSpeed = bulletSpeed;
        _bulletScript.distanceToReturn = distanceToReturn;
        _startPosition = transform.position;
        _limitYBottom = transform.position.y;
        
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Start()
    {
        _position = transform.position;

        // 1. Calculamos la distancia a la cámara
        float distanceToCamera = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);

        // 2. Calculamos la ALTURA REAL del mundo visible (basado en la cámara)
        Vector3 topPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, distanceToCamera));
        Vector3 bottomPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, distanceToCamera));
        float worldHeight = Mathf.Abs(topPoint.y - bottomPoint.y);

        // 3. Definimos la proporción deseada (16:9)
        float targetAspectRatio = 16f / 9f;

        // 4. Calculamos cuál debería ser el ANCHO del mundo para esa proporción
        float targetWorldWidth = worldHeight * targetAspectRatio;

        // 5. Definimos el límite X: (Mitad del ancho calculado) - padding
        // Esto ignora si el celular es más ancho; el arma topará en el borde del 16:9
        _limitX = (targetWorldWidth / 2f) - paddingX;

        // 6. Configuración de límite vertical (lógica original conservada)
        // Inicializamos _posEdgeWorld para evitar errores de null, aunque solo usaremos la Y
        _posEdgeWorld = new Vector3(0, heightLimit, 0); 
        // Aseguramos que el límite superior sea el que definiste en el inspector
        _posEdgeWorld.y = heightLimit; 
    }
    private void Update()
    {
        // si está volviendo a su punto de inicio, no hacer nada más
        if (_isReturning) return;

        if (_gameManager.GameStarted) GunManagement();
        
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
        scoreText.text = $"DOVES: {score}/{totalDucks}";
    }

    private void GunManagement()
    {
        /*bool inputBloqueado = false;
        
        // Si el cursor está sobre UI (botones) → bloqueamos el input
        if (EventSystem.current.IsPointerOverGameObject()) inputBloqueado = true;

        // Si el panel de pausa está activo → bloqueamos input
        if (_gameManager.panelOnPause.activeSelf) inputBloqueado = true;

        // Si acabamos de reanudar → evitar el clic fantasma
        if (_gameManager.ignoreNextClick) inputBloqueado = true;*/
        
        // Si el panel de pausa está activo o se ignora el click, salimos
        if (_gameManager.panelOnPause.activeSelf || _gameManager.ignoreNextClick) return;
        
        
        _timer += Time.deltaTime;
        if (_timer > timeToStart)
        {
            if (!_isMovingY)
            {
                MoveGunX();
                if (InputDetectado())//(!inputBloqueado && Input.GetMouseButtonDown(0))
                {
                    _isMovingY = true;    
                }
            }
            else
            {
                if (!_stopGun)
                {
                    MoveGunY();
                    if (InputDetectado())//(!inputBloqueado && Input.GetMouseButtonDown(0))
                    {
                        _stopGun = true;    
                        //AudioSource.PlayClipAtPoint(shotSFX, transform.position, 1f);
                        _audioManager.SFXGun.PlayOneShot(_audioManager.SFXGun.clip, 1);
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
    
    public void ResetGun()
    {
        transform.position = _startPosition;
        _position = _startPosition;

        _timer = 0;
        _isMovingY = false;
        _stopGun = false;
        _canShot = true;
        _isReturning = false;
    }
    
    
    // NUEVA FUNCIÓN AUXILIAR PARA INPUT (MÓVIL Y PC)
    private bool InputDetectado()
    {
        // 1. Verificación para MÓVIL (Touch)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
        
            // Solo actuamos cuando el dedo "empieza" a tocar la pantalla
            if (touch.phase == TouchPhase.Began)
            {
                // Verificamos si este toque específico está sobre un botón de UI
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return false;
            
                return true; // Es un toque válido de juego
            }
        }

        // 2. Verificación para PC/EDITOR (Mouse)
        if (Input.GetMouseButtonDown(0))
        {
            // En PC no necesitamos pasar ID, basta con la función por defecto
            if (EventSystem.current.IsPointerOverGameObject()) return false;
        
            return true;
        }

        return false;
    }
    
}
