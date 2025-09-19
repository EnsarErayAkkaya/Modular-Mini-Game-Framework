using EEA.MiniGames;
using EEA.Services;
using EEA.Shared;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.Menu
{
    public class MiniGameCard : MonoBehaviour, ISnappyScrollElement
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _playButton;

        private MiniGameData _miniGameData;
        private Coroutine _cachedCoroutine;

        public RectTransform RectTransform => (RectTransform)transform;

        public void Initialize(MiniGameData miniGameData)
        {
            this._miniGameData = miniGameData;

            _titleText.text = miniGameData.DisplayName;
            _iconImage.sprite = miniGameData.MiniGameIcon;
        }

        public void OnButtonClick()
        {
            ServicesContainer.SceneService.RemoveScene(Services.SceneServices.SceneKeys.MenuScene);

            _ = MiniGameService.Instance.LoadGame(_miniGameData.Id);
            //ServicesContainer.SceneService.LoadScene(_miniGameData.SceneConfig.SceneKey);
        }

        public void OnFocus()
        {
            if (_cachedCoroutine != null)
            {
                StopCoroutine(_cachedCoroutine);
                _cachedCoroutine = null;
            }

            _cachedCoroutine = StartCoroutine(Tweens.ScaleTransform(_playButton.transform, Vector3.one, 0.3f));
        }

        public void OnUnfocus()
        {
            if (_cachedCoroutine != null)
            {
                StopCoroutine(_cachedCoroutine);
                _cachedCoroutine = null;
            }

            _cachedCoroutine = StartCoroutine(Tweens.ScaleTransform(_playButton.transform, Vector3.zero, 0.3f));
        }
    }
}