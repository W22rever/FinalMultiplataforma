using UnityEngine;
using MiniGame1.Enemy;

namespace MiniGame1
{
    public class ExploPooling : GenericPool<Explotion>
    {
        protected override void OnTakeFromPool(Explotion item)
        {
            base.OnTakeFromPool(item);
            item.SetPool(Pool);
        }

        public void SpawnExplotion(Transform exEnemyPosition)
        {
            Explotion explotion = Pool.Get();
            explotion.transform.position = exEnemyPosition.position;
        }
    }
}