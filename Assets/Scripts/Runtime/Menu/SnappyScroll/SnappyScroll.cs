using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EEA.Menu
{
    public class SnappyScroll : MonoBehaviour, IEndDragHandler
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private float snapSpeed = 10f;
        [SerializeField] private float velocityThreshold = 400f; // velocity cutoff for swipe

        private List<RectTransform> items = new List<RectTransform>();
        private Dictionary<RectTransform, ISnappyScrollElement> itemElements = new Dictionary<RectTransform, ISnappyScrollElement>();

        private int currentIndex = 0;
        private bool isSnapping = false;
        private Vector2 targetPosition;
        private int previousIndex = -1;

        public void Setup()
        {
            // Cache all child items (mini-game cards)
            foreach (Transform child in scrollRect.content)
            {
                if (child is RectTransform rect)
                    items.Add(rect);

                if (child.TryGetComponent<ISnappyScrollElement>(out var element))
                {
                    itemElements[child as RectTransform] = element;
                }
            }

            SnapTo(currentIndex, instant: true);
        }

        private void Update()
        {
            if (isSnapping)
            {
                scrollRect.content.anchoredPosition = Vector2.Lerp(
                    scrollRect.content.anchoredPosition,
                    targetPosition,
                    Time.deltaTime * snapSpeed
                );

                if (Vector2.Distance(scrollRect.content.anchoredPosition, targetPosition) < 0.1f)
                {
                    scrollRect.content.anchoredPosition = targetPosition;
                    isSnapping = false;
                    scrollRect.velocity = Vector2.zero; // stop inertia
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            float velocityX = scrollRect.velocity.x;

            if (Mathf.Abs(velocityX) > velocityThreshold)
            {
                // Fast swipe ? move to next/previous
                if (velocityX < 0 && currentIndex < items.Count - 1)
                    currentIndex++;
                else if (velocityX > 0 && currentIndex > 0)
                    currentIndex--;
            }
            else
            {
                // Slow drag ? snap to nearest
                currentIndex = GetNearestIndex();
            }

            SnapTo(currentIndex);
        }

        private void SnapTo(int index, bool instant = false)
        {
            Vector2 newPos = GetSnapPosition(index);

            if (instant)
            {
                scrollRect.content.anchoredPosition = newPos;
                isSnapping = false;
            }
            else
            {
                targetPosition = newPos;
                isSnapping = true;
            }

            //Debug.Log($"Snapping to index: {index}, previous: {previousIndex}");

            if (previousIndex != index)
            {
                itemElements[items[index]].OnFocus();

                if (previousIndex != -1)
                {
                    itemElements[items[previousIndex]].OnUnfocus();
                }

                previousIndex = index;
            }
        }

        private Vector2 GetSnapPosition(int index)
        {
            float elementWidth = items[index].rect.width;
            float spacing = scrollRect.content.GetComponent<HorizontalLayoutGroup>()?.spacing ?? 0f;

            float offset = (elementWidth + spacing) * index;
            return new Vector2(-offset, scrollRect.content.anchoredPosition.y);
        }

        private int GetNearestIndex()
        {
            float closestDistance = float.MaxValue;
            int nearestIndex = currentIndex;

            for (int i = 0; i < items.Count; i++)
            {
                float dist = Mathf.Abs(scrollRect.content.anchoredPosition.x - GetSnapPosition(i).x);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    nearestIndex = i;
                }
            }

            return nearestIndex;
        }

        public int GetCurrentIndex() => currentIndex;
    }
}