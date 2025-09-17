using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.Services.SceneServices
{
    public class SceneService : BaseService, ISceneService
    {
        private Dictionary<string, GameObject> _loadedScenes = new Dictionary<string, GameObject>();
        private SceneServiceSettings _settings;

        public Dictionary<string, GameObject> LoadedScenes => _loadedScenes;

        public SceneService(SceneServiceSettings settings)
        {
            this._settings = settings;
        }

        public void Clear()
        {
            foreach (var scenes in _loadedScenes)
            {
                GameObject.Destroy(scenes.Value);
            }

            _loadedScenes.Clear();
        }

        public GameObject LoadScene(string sceneKey)
        {
            var config = _settings.GetSceneConfig(sceneKey);

            ServicesContainer.EventBus.Publish(new SceneTransitionStarted() { SceneConfig = config });

            if (config.RemoveAllOtherScenes)
            {
                Clear();
            }

            // Find prefab
            var sceneGameobject = LoadSceneResource(sceneKey);

            if (sceneGameobject == null)
            {
                Debug.LogError($"Scene '{sceneGameobject.name}' not found!");
                return null;
            }

            // Instantiate
            var currentScene = GameObject.Instantiate(sceneGameobject);
            _loadedScenes.Add(sceneKey, currentScene);

            ServicesContainer.EventBus.Publish(new SceneTransitionEnded() { SceneConfig = config });

            return currentScene;
        }

        public void RemoveScene(string scene)
        {
            if (_loadedScenes.TryGetValue(scene, out var go))
            {
                GameObject.Destroy(go);
                _loadedScenes.Remove(scene);
            }
        }

        private GameObject LoadSceneResource(string sceneKey)
        {
            var config = _settings.GetSceneConfig(sceneKey);

            if (config != null)
            {
                return Resources.Load<GameObject>(config.SceneKey);
            }
            else
            {
                return null;
            }
        }
    }
}