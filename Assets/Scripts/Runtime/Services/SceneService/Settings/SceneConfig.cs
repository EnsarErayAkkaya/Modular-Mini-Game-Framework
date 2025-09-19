using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EEA.Services.SceneServices
{
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "Base Scriptable Objects/Scenes/Scene Config", order = 1)]
    public class SceneConfig : ScriptableObject
    {
        [Dropdown(typeof(SceneKeys))]
        public string SceneKey;
        public bool RemoveAllOtherScenes = false;
        public bool ShowSceneTransition;
    }
}
