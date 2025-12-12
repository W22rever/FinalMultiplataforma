using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Background : MonoBehaviour
{
    private Renderer _renderer;
    public Vector2 scrollSpeed = new Vector2(0f, 0.2f);
    public string texturePropertyName = "_BaseMap"; // Nombre de la propiedad de textura (ej: _MainTex, _BumpMap)
    
    [SerializeField] private Material _material;
    private Vector2 _currentOffset;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        
        _material = _renderer.material; // Esto crea una instancia del material

        // Obtener el offset inicial si necesitas trabajar con él
        _currentOffset = _material.GetTextureOffset(texturePropertyName);
        Debug.Log($"Offset inicial: {_currentOffset}");
    }

    void Update()
    {
        _currentOffset += scrollSpeed * Time.deltaTime;
        // Debug.Log("current offset: " + _currentOffset);
        // Debug.Log(_material.GetTextureOffset(texturePropertyName));

        _material.SetTextureOffset(texturePropertyName, _currentOffset);

        if (_currentOffset.x > 1) {
            _currentOffset.x -= 1;
        }
        _material.SetTextureOffset(texturePropertyName, _currentOffset);
    }

    public Vector2 GetTextureOffset()
    {
        return _material.GetTextureOffset(texturePropertyName);
    }
    
}
