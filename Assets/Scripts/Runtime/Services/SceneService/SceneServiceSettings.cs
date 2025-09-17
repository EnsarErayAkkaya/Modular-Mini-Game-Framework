using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EEA.Services.SceneServices
{
    [CreateAssetMenu(fileName = "SceneServiceSettings", menuName = "Base Scriptable Objects/Scenes/Scene Service Settings", order = 0)]
    public sealed class SceneServiceSettings : ScriptableObject
    {
        public List<SceneConfig> SceneConfigs;

        public SceneConfig GetSceneConfig(string sceneType)
        {
            return SceneConfigs.FirstOrDefault(s => s.SceneKey == sceneType);
        }
    }
}
