using EEA.Shared;
using System;
using System.Collections;
using UnityEngine;

namespace EEA.Services.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Window : MonoBehaviour
    {
        public enum WindowState { Hidden, Visible, Closed }

        [SerializeField] private float _fadeDuration = 0.35f;

        private CanvasGroup _canvasGroup;
        private string _id;

        public string Id => _id;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public WindowState State { get; private set; } = WindowState.Hidden;

        public void Init(string id = null)
        {
            State = WindowState.Hidden;
            _id = id;
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            if (State == WindowState.Visible || State == WindowState.Closed) return;

            State = WindowState.Visible;
            _canvasGroup.blocksRaycasts = true;

            StartCoroutine(AnimateIn());

            ServicesContainer.EventBus.Publish(new OnWindowOpened{ Window = this});
        }

        public virtual void Hide()
        {
            if (State != WindowState.Visible) return;

            State = WindowState.Hidden;
            _canvasGroup.blocksRaycasts = false;

            StartCoroutine(AnimateOut(null));
        }

        public virtual void Close()
        {
            if (State == WindowState.Closed) return;

            State = WindowState.Closed;

            if (_canvasGroup.alpha <= 0f)
            {
                HandleCleanup();
            }
            else
            {
                StartCoroutine(AnimateOut(() => HandleCleanup()));
            }
        }

        protected virtual IEnumerator AnimateIn()
        {
            yield return StartCoroutine(Tweens.FadeCanvasGroup(_canvasGroup, 0, 1, _fadeDuration));
        }

        protected virtual IEnumerator AnimateOut(Action onComplete)
        {
            yield return StartCoroutine(Tweens.FadeCanvasGroup(_canvasGroup, 1, 0, _fadeDuration));
            onComplete?.Invoke();
        }

        private void HandleCleanup()
        {
            ServicesContainer.EventBus.Publish(new OnWindowClosed { WindowId = this.Id});
        }
    }
}
