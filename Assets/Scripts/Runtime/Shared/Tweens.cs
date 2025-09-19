using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.Shared
{
    public static class Tweens
    {
        public static IEnumerator ResizeRectTransform(RectTransform rectTransform, Vector2 size, float duration)
        {
            float elapsedTime = 0f;
            Vector2 startingSize = rectTransform.sizeDelta;

            while (elapsedTime < duration)
            {
                rectTransform.sizeDelta = Vector2.Lerp(startingSize, size, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rectTransform.sizeDelta = size;
        }

        public static IEnumerator AnimateAnchoredPosition(RectTransform rectTransform, Vector2 pos, float duration)
        {
            float elapsedTime = 0f;
            Vector2 startingPos = rectTransform.anchoredPosition;

            while (elapsedTime < duration)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(startingPos, pos, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rectTransform.anchoredPosition = pos;
        }

        public static IEnumerator ScaleTransform(Transform transform, Vector3 targetScale, float duration)
        {
            float elapsedTime = 0f;
            Vector3 initialScale = transform.localScale;

            while (elapsedTime < duration)
            {
                transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
        }

        public static IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float to, float duration)
        {
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(start, to, elapsedTime / duration);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = to;
        }

        public static IEnumerator FadeImage(Image image, float start, float to, float duration)
        {
            float elapsedTime = 0;

            Color startColor = image.color;
            startColor.a = start;

            Color targetColor = image.color;
            targetColor.a = to;

            while (elapsedTime < duration)
            {
                image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            image.color = targetColor;
        }
    }
}
