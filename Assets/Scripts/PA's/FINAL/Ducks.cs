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
    private Vector3 _direction = Vector3.right;
    private Vector3 _posBorderWorld;
    
    void Awake()
    {
        _duckPositon = transform.position;       
    }

    private void Start()
    {
        float distanceToCamera = Mathf.Abs(transform.position.z - Camera.main.transform.position.z); 
        _posBorderWorld = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        _limitX = _posBorderWorld.x + offsetX;
    }
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer < timeToStart) return; // Equivale a poner if(timer >= timeToStart)
        MoveDucks();
    }

    private void MoveDucks()
    {
        _duckPositon += _direction * (speed * Time.deltaTime);

        if (flipMovement)
        {
            _direction = Vector3.left;

        }

        transform.position = _duckPositon;

        // Para los que se mueven a la derecha o para los que se mueven a la izquierda hacemos que al estar fuera de vision de camara retornen desde el lado opuesto
        //if (transform.position.x >= _limitX || transform.position.x <= -_limitX) _duckPositon.x = -transform.position.x;
        if (transform.position.x >= _limitX)
            _duckPositon.x = -_limitX;
        else if (transform.position.x <= -_limitX)
            _duckPositon.x = _limitX;

    }
}
