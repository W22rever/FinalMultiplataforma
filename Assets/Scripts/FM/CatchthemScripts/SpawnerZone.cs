using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;
using FM.CatchthemScripts.GameManagement;

namespace  FM.CatchthemScripts
{
    

public class SpawnerZone : MonoBehaviour
{
    [Header("List Prefabs")] [SerializeField]
    private List<GameObject> prefabs;

    [Header("Amount of Prefabs")] public int amountOfPrefabs = 2;

    [Header("Time Between Spawns")] [SerializeField]
    private float spawnInterval = 1f;

    private BoxCollider2D boxZone;
// Pool organizado correctamente
    private Dictionary<GameObject, Queue<PoolItem>> pool = new Dictionary<GameObject, Queue<PoolItem>>();

    [Header("Player")]
    [SerializeField] private SpriteRenderer playerRenderer;
    private float  extraPadding;
    private PlayerMovement playerMovement;
    
    private float spawnMinX;
    private float spawnMaxX;

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        extraPadding = playerMovement.ExtraPadding;
    }

    private void Start()
    {
        boxZone = GetComponent<BoxCollider2D>();
        
        // Calculamos los bordes de la pantalla
        Vector3 left = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        //Obtenemos el ancho real de Santa (Padding Dinámico)
        // Usamos el renderer si está asignado, sino 1f por seguridad
        float dynamicPadding = (playerRenderer != null) ? playerRenderer.bounds.extents.x : 1f;

        
        // Definimos los límites de spawn (Mundo Real)
        spawnMinX = left.x + dynamicPadding + extraPadding;
        spawnMaxX = right.x - dynamicPadding - extraPadding;

        // Calculamos el ancho total y el punto medio exacto
        float width = spawnMaxX - spawnMinX;
        float mid = (spawnMaxX + spawnMinX) / 2f;
        
        
        // Ajustamos el tamaño del collider
        boxZone.size = new Vector2(width, boxZone.size.y);
        
        // Movemos el OBJETO Spawner físicamente al centro de la pantalla
        // Mantenemos su altura original (Y) y profundidad (Z)
        transform.position = new Vector3(mid, transform.position.y, transform.position.z);
        
        // Reseteamos el offset X a 0, porque el objeto ya está centrado.
        // Mantenemos el offset Y por si lo ajustaste manualmente para subir/bajar el collider.
        boxZone.offset = new Vector2(0, boxZone.offset.y);
        
        CreatePool();
        StartCoroutine(SpawnRoutine());
    }

    private void CreatePool()
    {
        foreach (var prefab in prefabs)
        {
            Queue<PoolItem> cola = new Queue<PoolItem>();

            for (int i = 0; i < amountOfPrefabs; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);

                PoolItem item = obj.GetComponent<PoolItem>();
                item.spawner = this;
                item.originalPrefab = prefab;

                cola.Enqueue(item);
            }

            pool.Add(prefab, cola);
        }
    }


    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Spawn()
    {
        if (FM.CatchthemScripts.GameManagement.GameManager.Instance.state != GameState.Playing) return;
        
        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
        Queue<PoolItem> cola = pool[prefab];

        PoolItem item;

        if (cola.Count > 0)
        {
            item = cola.Dequeue();
        }
        else
        {
            // Crear uno extra si se sobrepasa el pool
            GameObject obj = Instantiate(prefab, transform);

            item = obj.GetComponent<PoolItem>();
            item.spawner = this;
            item.originalPrefab = prefab;
        }

        // Posición aleatoria
        float x = Random.Range(spawnMinX, spawnMaxX);
        float y = transform.position.y;

        item.transform.position = new Vector3(x, y, 0f);
        item.gameObject.SetActive(true);
    }


    public void BackToPool(PoolItem  item)
    {
        item.gameObject.SetActive(false);

        // Ahora SIEMPRE sabemos a cuál pool pertenece
        pool[item.originalPrefab].Enqueue(item);
    }
    
    // Para detener la corrutina usamos la suscripción
    private void OnEnable()
    {
        GameEvents.OnGameStop += StopSpawner;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStop -= StopSpawner;
    }

    private void StopSpawner()
    {
        StopAllCoroutines();
    }
}}