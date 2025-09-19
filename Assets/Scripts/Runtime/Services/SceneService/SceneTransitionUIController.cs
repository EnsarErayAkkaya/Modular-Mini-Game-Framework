using EEA.Services.Events;
using EEA.Shared;
using System.Collections;
using UnityEngine;

namespace EEA.Services.SceneServices
{
    public class SceneTransitionUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _transitionImage;

        private WaitForEndOfFrame _waitForEndOfFrame = new();

        private Coroutine _cachedRoutine;

        private void Start()
        {
            ServicesContainer.EventBus.Subscribe<OnSceneTransitionStarted>(OnSceneTransitionStarted);
            ServicesContainer.EventBus.Subscribe<OnSceneTransitionEnded>(OnSceneTransitionCompleted);
        }

        private void OnDestroy()
        {
            ServicesContainer.EventBus.Unsubscribe<OnSceneTransitionStarted>(OnSceneTransitionStarted);
            ServicesContainer.EventBus.Unsubscribe<OnSceneTransitionEnded>(OnSceneTransitionCompleted);
        }

        private void OnSceneTransitionStarted(OnSceneTransitionStarted data)
        {
            if (!data.SceneConfig.ShowSceneTransition) return;

            if (_cachedRoutine != null)
            {
                StopCoroutine(_cachedRoutine);
                _cachedRoutine = null;
            }

            _cachedRoutine = StartCoroutine(Tweens.FadeCanvasGroup(_transitionImage, 0, 1, .3f));
        }

        private void OnSceneTransitionCompleted(OnSceneTransitionEnded data)
        {
            if (!data.SceneConfig.ShowSceneTransition) return;

            if (_cachedRoutine != null)
            {
                StopCoroutine(_cachedRoutine);
                _cachedRoutine = null;
            }

            _cachedRoutine = StartCoroutine(Tweens.FadeCanvasGroup(_transitionImage, 1, 0, .3f));
        }
    }
}