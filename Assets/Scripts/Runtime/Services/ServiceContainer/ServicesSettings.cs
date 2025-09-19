using EEA.Services.PoolServices;
using EEA.Services.SceneServices;
using EEA.Services.Windows;
using UnityEngine;

namespace EEA.Services
{
    [CreateAssetMenu(fileName = "ResolveSettings", menuName = "Base Scriptable Objects/Resolve/ResolveSettings", order = 0)]
    public class ServicesSettings : ScriptableObject
    {
        public SceneServiceSettings SceneServiceSettings;
        public PoolServiceSettings PoolServiceSettings;
        public WindowServiceSettings WindowServiceSettings;

        public bool debugLog = true;
    }
}