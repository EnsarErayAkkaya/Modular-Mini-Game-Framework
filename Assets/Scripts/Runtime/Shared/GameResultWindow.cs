using EEA.MiniGames;
using EEA.MiniGames.Common;
using EEA.Services;
using EEA.Services.SceneServices;
using EEA.Services.Windows;
using TMPro;
using UnityEngine;

namespace EEA.Shared
{
    public class GameResultWindow : Window
    {
        [SerializeField] private TextMeshProUGUI _resultText;

        public const string WindowId = "game_result_window";

        public void ShowResult(string result)
        {
            _resultText.text = result;
            _resultText.transform.localScale = Vector3.zero;
            Show();
            StartCoroutine(Tweens.ScaleTransform(_resultText.transform, Vector3.one, 0.6f));
        }

        public void OnPlayAgainButton()
        {
            MiniGameService.CurrentGame.Restart();
            Close();
        }

        public void OnReturnHomeButton()
        {
            MiniGameService.CurrentGame.EndGame();
            Close();
        }
    }
}