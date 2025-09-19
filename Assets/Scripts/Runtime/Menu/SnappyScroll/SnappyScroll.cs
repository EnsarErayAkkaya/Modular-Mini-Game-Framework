using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EEA.Menu
{
    public class SnappyScroll : MonoBehaviour, IEndDragHandler
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private float _snapSpeed = 10f;
        [SerializeField] private float _velocityThreshold = 400f; // velocity cutoff for swipe

        private List<RectTransform> _items = new List<RectTransform>();
        private Dictionary<RectTransform, ISnappyScrollElement> _itemElements = new Dictionary<RectTransform, ISnappyScrollElement>();

        private int _currentIndex = 0;
        private bool _isSnapping = false;
        private Vector2 _targetPosition;
        private int _previousIndex = -1;

        public void Setup()
        {
            // Cache all child items (mini-game cards)
            foreach (Transform child in _scrollRect.content)
            {
                if (child is RectTransform rect)
                    _items.Add(rect);

                if (child.TryGetComponent<ISnappyScrollElement>(out var element))
                {
                    _itemElements[child as RectTransform] = element;
                }
            }

            SnapTo(_currentIndex, instant: true);
        }

        private void Update()
        {
            if (_isSnapping)
            {
                _scrollRect.content.anchoredPosition = Vector2.Lerp(
                    _scrollRect.content.anchoredPosition,
                    _targetPosition,
                    Time.deltaTime * _snapSpeed
                );

                if (Vector2.Distance(_scrollRect.content.anchoredPosition, _targetPosition) < 0.1f)
                {
                    _scrollRect.content.anchoredPosition = _targetPosition;
                    _isSnapping = false;
                    _scrollRect.velocity = Vector2.zero; // stop inertia
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            float velocityX = _scrollRect.velocity.x;

            if (Mathf.Abs(velocityX) > _velocityThreshold)
            {
                // Fast swipe ? move to next/previous
                if (velocityX < 0 && _currentIndex < _items.Count - 1)
                    _currentIndex++;
                else if (velocityX > 0 && _currentIndex > 0)
                    _currentIndex--;
            }
            else
            {
                // Slow drag ? snap to nearest
                _currentIndex = GetNearestIndex();
            }

            SnapTo(_currentIndex);
        }

        private void SnapTo(int index, bool instant = false)
        {
            Vector2 newPos = GetSnapPosition(index);

            if (instant)
            {
                _scrollRect.content.anchoredPosition = newPos;
                _isSnapping = false;
            }
            else
            {
                _targetPosition = newPos;
                _isSnapping = true;
            }

            //GameLogger.Log($"Snapping to index: {index}, previous: {previousIndex}");

            if (_previousIndex != index)
            {
                _itemElements[_items[index]].OnFocus();

                if (_previousIndex != -1)
                {
                    _itemElements[_items[_previousIndex]].OnUnfocus();
                }

                _previousIndex = index;
            }
        }

        private Vector2 GetSnapPosition(int index)
        {
            float elementWidth = _items[index].rect.width;
            float spacing = _scrollRect.content.GetComponent<HorizontalLayoutGroup>()?.spacing ?? 0f;

            float offset = (elementWidth + spacing) * index;
            return new Vector2(-offset, _scrollRect.content.anchoredPosition.y);
        }

        private int GetNearestIndex()
        {
            float closestDistance = float.MaxValue;
            int nearestIndex = _currentIndex;

            for (int i = 0; i < _items.Count; i++)
            {
                float dist = Mathf.Abs(_scrollRect.content.anchoredPosition.x - GetSnapPosition(i).x);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    nearestIndex = i;
                }
            }

            return nearestIndex;
        }

        public int GetCurrentIndex() => _currentIndex;
    }
}