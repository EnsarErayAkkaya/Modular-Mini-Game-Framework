using System.Collections.Generic;
using UnityEngine;

namespace EEA.Services.PoolServices
{
    [System.Serializable]
    internal struct PoolInitializeData
    {
        [SerializeField] internal GameObject poolItem;
        [SerializeField] internal int preload;
        [SerializeField] internal int capacity;
    }

    [CreateAssetMenu(fileName = "PoolServiceSettings", menuName = "Base Scriptable Objects/Pooling/Pool Settings", order = 0)]
    public class PoolServiceSettings : ScriptableObject
    {
        public int defaultPoolCapacity = 100;
        public int defaultPoolPreload = 0;
    }
}
