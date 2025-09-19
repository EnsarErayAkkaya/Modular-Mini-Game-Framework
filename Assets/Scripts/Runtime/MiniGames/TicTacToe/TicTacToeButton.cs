using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.MiniGames.TicTacToe
{
    public class TicTacToeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;

        private RectTransform _rectTransform;

        public Button Button => _button;
        public TextMeshProUGUI Text => _text;
        public RectTransform RectTransform => _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}
