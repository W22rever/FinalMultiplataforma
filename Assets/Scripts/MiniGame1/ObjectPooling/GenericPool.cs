using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericPool<T> : MonoBehaviour where T : Component
{
    [Header("Configuración del Pool")]
    [SerializeField] private T _prefab;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 100;

    private IObjectPool<T> _pool;

    public IObjectPool<T> Pool
    {
        get
        {
            if (_pool == null) InitializePool();
            return _pool;
        }
    }

    private void InitializePool()
    {
        // CORRECCIÓN 2: Instanciamos ObjectPool (la clase de Unity), no esta misma clase abstracta
        _pool = new ObjectPool<T>(
            createFunc: CreatePooledItem,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnedToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize
        );
    }

    private T CreatePooledItem()
    {
        T instance = Instantiate(_prefab, transform);
        return instance;
    }
    

    protected virtual void OnTakeFromPool(T item)
    {
        item.gameObject.SetActive(true);
    }

    protected virtual void OnReturnedToPool(T item)
    {
        item.gameObject.SetActive(false);
    }

    protected virtual void OnDestroyPoolObject(T item)
    {
        Destroy(item.gameObject);
    }
}