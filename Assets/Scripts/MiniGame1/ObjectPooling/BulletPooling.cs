using System;
using UnityEngine;

public class BulletPooling : GenericPool<Proyectile>
{
    [Header("Bullet Pooling Settings")]
    [SerializeField] private Transform _SpawnBullet;

    protected override void OnTakeFromPool(Proyectile item)
    {
        base.OnTakeFromPool(item);
        item.SetPool(Pool);
    }

    public void SpawnBullet()
    {
        Proyectile bullet = Pool.Get();
        bullet.transform.position = _SpawnBullet.position;
    }
}