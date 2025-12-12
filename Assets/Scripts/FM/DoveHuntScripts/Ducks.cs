using UnityEngine;

public class Ducks : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private bool flipMovement;
    [SerializeField] private float offsetX;
    [SerializeField] private float timeToStart;
    
    private float _limitX;
    private float _timer;
    private Vector3 _duckPositon;
    private Vector3 _initialPosition;
    private Vector3 _direction = Vector3.right;
    private Vector3 _posBorderWorld;
    
    private GameManager _gameManager;
    
    void Awake()
    {
        _initialPosition = transform.position;
        _duckPositon = _initialPosition; 
        
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        // 1. Calculamos la distancia Z (igual que antes)
        float distanceToCamera = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
    
        // 2. En lugar de pedir el ancho de pantalla (que varía), pedimos la ALTURA del mundo visible
        // Tomamos el punto superior (Y=1) y el inferior (Y=0) y restamos
        Vector3 topPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, distanceToCamera));
        Vector3 bottomPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, distanceToCamera));
        float worldHeight = Mathf.Abs(topPoint.y - bottomPoint.y);
    
        // 3. Definimos tu Aspect Ratio de diseño (ej: 16:9 para HD estándar)
        float targetAspectRatio = 16f / 9f; 

        // 4. Calculamos el ancho "correcto" basándonos en esa proporción fija
        float targetWorldWidth = worldHeight * targetAspectRatio;

        // 5. Fijamos el límite: La mitad de ese ancho + tu offset
        _limitX = (targetWorldWidth / 2f) + offsetX;
    }
    void Update()
    {
        if (_gameManager.GameStarted) DovesManagement();
    }

    private void MoveDucks()
    {
        _duckPositon += _direction * (speed * Time.deltaTime);

        if (flipMovement) _direction = Vector3.left;

        transform.position = _duckPositon;

        // Para los que se mueven a la derecha o para los que se mueven a la izquierda hacemos que al estar fuera de vision de camara retornen desde el lado opuesto
        //if (transform.position.x >= _limitX || transform.position.x <= -_limitX) _duckPositon.x = -transform.position.x;
        if (transform.position.x >= _limitX)
            _duckPositon.x = -_limitX;
        else if (transform.position.x <= -_limitX)
            _duckPositon.x = _limitX;

       
    }

    private void DovesManagement()
    {
        _timer += Time.deltaTime;
        if(_timer < timeToStart) return; // Equivale a poner if(timer >= timeToStart)
        MoveDucks();
    }
    
    public void ResetDuck()
    {
        // Restaurar posición original
        transform.position = _initialPosition;
        _duckPositon = _initialPosition;

        // Reset timer
        _timer = 0;
    }
}
