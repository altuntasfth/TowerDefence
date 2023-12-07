using UnityEngine;

namespace Pooling
{
    public class PooledObject : MonoBehaviour
    {
        private ObjectPoolBase pool;
        public ObjectPoolBase Pool { get => pool; set => pool = value; }

        public void Release()
        {
            pool.ReturnToPool(this);
        }
    }
}