using EEA.Services.Events;
using EEA.Services.PoolServices;
using EEA.Services.SaveServices;
using EEA.Services.SceneServices;
using EEA.Services.Windows;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Services
{
    public class ServicesContainer : MonoBehaviour
    {
        [SerializeField] private ServicesSettings _settings;

        private SaveService _saveService;
        private SceneServices.SceneService _sceneService;
        private EventBus _eventBus;
        private PoolServices.PoolService _poolService;
        private WindowService _windowService;

        private List<ITickable> _tickables = new();

        private static ServicesContainer _instance;

        public ServicesSettings Settings => _settings;
        public static ISceneService SceneService => _instance._sceneService;
        public static IEventBus EventBus => _instance._eventBus;
        public static ISaveService SaveService => _instance._saveService;
        public static IPoolService GlobalPool => _instance._poolService;
        public static IWindowService WindowService => _instance._windowService;
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
            _eventBus = BindServiceInterfaces(new EventBus());
            _sceneService = BindServiceInterfaces(new SceneServices.SceneService(_settings.SceneServiceSettings));
            _saveService = BindServiceInterfaces(new SaveService(new EncryptedSaveHandler()));
            _poolService = BindServiceInterfaces(new PoolServices.PoolService(_settings.PoolServiceSettings));
            _windowService = BindServiceInterfaces(new WindowService(_settings.WindowServiceSettings));
        }

        private void Update()
        {
            foreach (var t in _tickables)
            {
                t.Tick();
            }
        }

        private T BindServiceInterfaces<T>(T baseService) where T : BaseService
        {
            if (baseService is ITickable)
            {
                _tickables.Add((ITickable)baseService);
            }

            return (T)baseService;
        }
    }
}