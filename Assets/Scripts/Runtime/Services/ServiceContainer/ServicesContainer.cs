using EEA.Services.Events;
using EEA.Services.SaveServices;
using EEA.Services.SceneServices;
using UnityEngine;

namespace EEA.Services
{
    public class ServicesContainer : MonoBehaviour
    {
        [SerializeField] private ServicesSettings _settings;

        private SaveService _saveService;
        private SceneService _sceneService;
        private EventBus _eventBus;
        //private WindowService windowService;

        private static ServicesContainer _instance;

        public ServicesSettings Settings => _settings;
        public static ISceneService SceneService => _instance._sceneService;
        public static IEventBus EventBus => _instance._eventBus;
        public static ISaveService SaveService => _instance._saveService;
        //public static IWindowService WindowService => instance.windowService;
        public static ServicesContainer Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance);
            }

            _instance = this;
        }
        
        public void Initialize()
        {
            _eventBus = new EventBus();
            _sceneService = new SceneService(_settings.sceneServiceSettings);
            _saveService = new SaveService(new EncryptedSaveHandler());
        }
    }
}