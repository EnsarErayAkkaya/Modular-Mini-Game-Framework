using TMPro;
using UnityEngine;

namespace EEA.MiniGames.HoopBall
{
    public class HoopBallView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _highScoreText;

        public void UpdateScore(int score, int highScore)
        {
            _scoreText.text = score.ToString();
            _highScoreText.text = highScore.ToString();
        }

        public void ResetView(int highScore)
        {
            _scoreText.text = "0";
            _highScoreText.text = highScore.ToString();
        }
    }
}