using EEA.Shared;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.MiniGames.TicTacToe
{

    public class TicTacToeStrikeLine : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;

        public Image LineImage => _image;
        public RectTransform RectTransform => _rectTransform;

        public IEnumerator AnimateStrikeLine(Vector2 from, Vector2 to)
        {
            _rectTransform.gameObject.SetActive(true);

            Vector2 direction = to - from;

            float length = Vector2.Distance(from, to);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rectTransform.rotation = Quaternion.Euler(0, 0, angle);

            from -= direction.normalized * (length * 0.2f);

            _rectTransform.anchoredPosition = from;

            //winLine.sizeDelta = new Vector2(length, winLine.sizeDelta.y);
            yield return StartCoroutine(Tweens.ResizeRectTransform(_rectTransform, new Vector2(length * 1.4f, _rectTransform.sizeDelta.y), 0.3f));
        }
    }
}