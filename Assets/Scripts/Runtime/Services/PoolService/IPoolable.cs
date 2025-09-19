using UnityEngine;

namespace EEA.Services.PoolServices
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
        GameObject gameObject { get; }
        Transform transform{ get; }
    }
}