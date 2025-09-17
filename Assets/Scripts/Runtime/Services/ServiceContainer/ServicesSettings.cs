using EEA.Services.SceneServices;
using UnityEngine;

namespace EEA.Services
{
    [CreateAssetMenu(fileName = "ResolveSettings", menuName = "Base Scriptable Objects/Resolve/ResolveSettings", order = 0)]
    public class ServicesSettings : ScriptableObject
    {
        public SceneServiceSettings sceneServiceSettings;

        public bool debugLog = true;
    }
}