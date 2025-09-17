using EEA.Services.Events;
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
            ServicesContainer.EventBus.Subscribe<SceneTransitionStarted>(OnSceneTransitionStarted);
            ServicesContainer.EventBus.Subscribe<SceneTransitionEnded>(OnSceneTransitionCompleted);
        }

        private void OnDestroy()
        {
            ServicesContainer.EventBus.Unsubscribe<SceneTransitionStarted>(OnSceneTransitionStarted);
            ServicesContainer.EventBus.Unsubscribe<SceneTransitionEnded>(OnSceneTransitionCompleted);
        }

        private void OnSceneTransitionStarted(SceneTransitionStarted data)
        {
            if (!data.SceneConfig.ShowSceneTransition) return;

            if (_cachedRoutine != null)
            {
                StopCoroutine(_cachedRoutine);
                _cachedRoutine = null;
            }

            _cachedRoutine = StartCoroutine(TransitionEnumerator(.3f, 0, 1));
        }

        private void OnSceneTransitionCompleted(SceneTransitionEnded data)
        {
            if (!data.SceneConfig.ShowSceneTransition) return;

            if (_cachedRoutine != null)
            {
                StopCoroutine(_cachedRoutine);
                _cachedRoutine = null;
            }

            _cachedRoutine = StartCoroutine(TransitionEnumerator(.3f, 1, 0));
        }

        private IEnumerator TransitionEnumerator(float duration, float start, float alpha)
        {
            float t = 0;
            while (t < duration)
            {
                _transitionImage.alpha = Mathf.Lerp(start, alpha, t / duration);

                t += Time.deltaTime;

                yield return _waitForEndOfFrame;
            }

            _transitionImage.alpha = alpha;
        }
    }
}