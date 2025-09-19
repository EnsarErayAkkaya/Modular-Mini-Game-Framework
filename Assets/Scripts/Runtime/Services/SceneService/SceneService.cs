using EEA.Services.SceneService;
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

        public async Task<GameObject> LoadScene(string sceneKey)
        {
            try
            {
                var config = _settings.GetSceneConfig(sceneKey);

                ServicesContainer.EventBus.Publish(new OnSceneTransitionStarted() { SceneConfig = config });

                if (config.RemoveAllOtherScenes)
                {
                    Clear();
                }

                // Find prefab
                var sceneGameobject = LoadSceneResource(sceneKey);

                if (sceneGameobject == null)
                {
                    GameLogger.LogError($"Scene '{sceneGameobject.name}' not found!");
                    return null;
                }

                // Instantiate
                var currentScene = GameObject.Instantiate(sceneGameobject);
                _loadedScenes.Add(sceneKey, currentScene);

                ISceneObject sceneObject = currentScene.GetComponent<ISceneObject>();
                if (sceneObject != null)
                {
                    await sceneObject.Initialize();
                }

                ServicesContainer.EventBus.Publish(new OnSceneTransitionEnded() { SceneConfig = config });

                return currentScene;
            }
            catch (System.Exception e)
            {
                GameLogger.Log(e.Message);
                return null;
            }
        }

        public async Task RemoveScene(string scene)
        {
            try
            {
                if (_loadedScenes.TryGetValue(scene, out var sceneGO))
                {
                    ISceneObject sceneObject = sceneGO.GetComponent<ISceneObject>();

                    if (sceneObject != null)
                    {
                        await sceneObject.Clear();
                    }

                    GameObject.Destroy(sceneGO);

                    _loadedScenes.Remove(scene);
                }
            }
            catch (System.Exception e)
            {
                GameLogger.Log(e.Message);
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