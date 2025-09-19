using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EEA.Services.Windows
{
    public class WindowService : BaseService, IWindowService
    {
        private WindowServiceSettings _settings; 
        private Canvas _canvas;
        private List<Window> _windows = new();

        public WindowServiceSettings Settings => _settings;
        public Canvas Canvas
        {
            get
            {
                if (_canvas == null)
                {
                    _canvas = GameObject.FindObjectOfType<WindowCanvas>().GetComponent<Canvas>();
                }

                return _canvas;
            }
        }

        public WindowService(WindowServiceSettings settings)
        {
            this._settings = settings;

            _windows = new List<Window>();

            ServicesContainer.EventBus.Subscribe<OnWindowClosed>(OnWindowClosed);
        }

        public Window Create(string id)
        {
            var windowPrefab = _settings.windows.FirstOrDefault(s => s.id == id).window.gameObject;
            var window = Create(windowPrefab, id);
            return window;
        }
        public T Create<T>(string id) where T : Window
        {
            Window popup = Create(id);
            return (T)Convert.ChangeType(popup, typeof(T));
        }

        private Window Create(GameObject prefab, string id)
        {
            Window window = ServicesContainer.GlobalPool.Spawn(prefab).GetComponent<Window>();
            window.transform.parent = Canvas.transform;
            window.Init(id);

            _windows.Add(window);

            return window;
        }

        public void Remove(Window window)
        {
            _windows.Remove(window);
        }

        private void OnWindowClosed(OnWindowClosed closed)
        {
            var window = _windows.FirstOrDefault(w => w.Id == closed.WindowId);
            ServicesContainer.GlobalPool.Despawn(window.gameObject);
            Remove(window);
        }

        public bool IsWindowOpen(string id, out Window foundWindow)
        {
            foreach (var window in _windows)
            {
                if (window.Id == id)
                {
                    foundWindow = window;

                    return true;
                }
            }

            foundWindow = null;

            return false;
        }
    }
}
