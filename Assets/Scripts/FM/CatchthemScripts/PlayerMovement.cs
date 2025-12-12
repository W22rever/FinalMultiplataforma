using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float extraPadding = 1.0f;
    [SerializeField] private GameObject basket;
    [SerializeField] private Camera cam;
    
    public float ExtraPadding => extraPadding;
    
    private float minX, maxX;
    private float halfWidth; // Mitad del ancho del jugador
    private Animator _anim;
    private bool _canMove = true;
    
    private Vector3 inputPos;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    
    private void Start()
    {
        Cursor.visible = false; 
        //  Lo confinamos para que no se salga de la ventana del juego en PC (Opcional para mayor seguridad)
        Cursor.lockState = CursorLockMode.Confined;
        
        //Calculamos automáticamente cuánto mide Santa desde su centro hasta su borde
        // bounds.extents.x nos da exactamente la mitad del ancho del sprite visible
        halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        
        Vector3 left = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 right = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        minX = left.x + halfWidth + extraPadding;
        maxX = right.x - halfWidth - extraPadding;;
    }
    
    private void OnEnable()
    {
        GameEvents.OnEndGame += OnEndGame;
    }

    private void OnDisable()
    {
        GameEvents.OnEndGame -= OnEndGame;
    }
    
    void Update()
    {
        if (!_canMove) return;
        
#if UNITY_EDITOR || UNITY_STANDALONE
        inputPos = Input.mousePosition;
#else
    // Móvil
    if (Input.touchCount == 0) return;
    inputPos = Input.GetTouch(0).position;
#endif

// Si está fuera de pantalla → no mover
        if (inputPos.x < 0 || inputPos.x > Screen.width) return;

        Vector3 worldPos = cam.ScreenToWorldPoint(inputPos);

        float clampedX = Mathf.Clamp(worldPos.x, minX, maxX);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
    
    
    private void OnEndGame(string reason)
    {
        _canMove = false; // siempre detener movimiento 

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Liberamos el mouse para que puedan dar clic en los botones
        
        if (reason == "life")
        {
            basket.SetActive(false);
            // Activamos la animación de muerte
            _anim.SetTrigger("IsDead");

        }
        else if (reason == "time")
        {
            // Detener animación o dejar idle
            _anim.speed = 0f;        // congela
        }
    }
}