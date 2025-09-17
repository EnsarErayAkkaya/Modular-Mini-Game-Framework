using System;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Services.Events
{
    public class EventBus : MonoBehaviour, IEventBus
    {
        private Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();


        public void Subscribe<T>(Action<T> callback)
        {
            if (_events.TryGetValue(typeof(T), out var del))
                _events[typeof(T)] = Delegate.Combine(del, callback);
            else
                _events[typeof(T)] = callback;
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            if (_events.TryGetValue(typeof(T), out var del))
            {
                var currentDel = Delegate.Remove(del, callback);
                if (currentDel == null) _events.Remove(typeof(T));
                else _events[typeof(T)] = currentDel;
            }
        }

        public void Publish<T>(T eventData)
        {
            if (_events.TryGetValue(typeof(T), out var del))
                (del as Action<T>)?.Invoke(eventData);
        }

        public void Clear() => _events.Clear();
    }
}