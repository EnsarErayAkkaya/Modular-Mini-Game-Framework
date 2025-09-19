using UnityEngine;

namespace EEA.Services.Windows
{
    public interface IWindowService
    {
        WindowServiceSettings Settings { get; }
        Canvas Canvas { get; }

        Window Create(string id);
        T Create<T>(string id) where T : Window;
        void Remove(Window window);
        bool IsWindowOpen(string id, out Window foundWindow);
    }
}