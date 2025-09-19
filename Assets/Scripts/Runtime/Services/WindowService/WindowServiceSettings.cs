using System.Collections.Generic;
using UnityEngine;

namespace EEA.Services.Windows
{
    [System.Serializable]
    public class WindowPrefabData
    {
        public string id;
        public Window window;
    }
    [CreateAssetMenu(fileName = "WindowServiceSettings", menuName = "Base Scriptable Objects/Window/Window Service Settings")]
    public class WindowServiceSettings : ScriptableObject
    {
        public List<WindowPrefabData> windows;
    }
}
