using EEA.Services;
using EEA.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EEA.MiniGames.TicTacToe
{
    public class TicTacToeView : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TicTacToeButton[] _grid;
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private TicTacToeStrikeLine _strikeLine;

        [Header("Board")]
        [SerializeField] private RectTransform _boardRectTransform;
        [SerializeField] private CanvasGroup _boardCanvasGroup;

        [Header("Lines")]
        [SerializeField] private RectTransform[] _horizontalLines;
        [SerializeField] private RectTransform[] _verticalLines;

        private Vector2 _horizontalLineSize;
        private Vector2 _verticalLineSize;

        public void Init(Action<int, int> onCellClicked)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int r = row, c = col;
                    GetButton(row, col).Button.onClick.AddListener(() => onCellClicked(r, c));
                }
            }

            MiniGameService.CurrentGame.EventBus.Subscribe<OnGameOver>(HandleGameOver);

            _horizontalLineSize = new Vector2(_boardRectTransform.rect.width, 25);
            _verticalLineSize = new Vector2(25, _boardRectTransform.rect.height);

            AnimeLineSize(_horizontalLineSize, _verticalLineSize);
        }

        private void HandleGameOver(OnGameOver data)
        {
            if (data.Result == GameResult.XWins || data.Result == GameResult.OWins)
            {
                if (data.WinningCells != null)
                {
                    _strikeLine.gameObject.SetActive(true);
                    StartCoroutine(PlayWinAnimation(data.Result, data.WinningCells));
                }
                else
                {
                    _strikeLine.gameObject.SetActive(false);
                }
            }
            else
            {
                _strikeLine.gameObject.SetActive(false);
            }
        }

        public void UpdateCell(int row, int col, Player player)
        {
            GetButton(row, col).Text.text = player == Player.None ? "" : player.ToString();
        }

        public void UpdateStatus(string message)
        {
            _statusText.text = message;
        }

        public void ResetView()
        {
            ResetCells();

            _statusText.text = "Player X's turn!";

            Color color = _strikeLine.LineImage.color;
            _strikeLine.LineImage.color = new Color(color.r, color.g, color.b, 1);
            _strikeLine.RectTransform.sizeDelta = new Vector2(0, _strikeLine.RectTransform.sizeDelta.y);
            _strikeLine.gameObject.SetActive(false);

            _boardCanvasGroup.alpha = 1;
        }

        private void ResetCells()
        {
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    GetButton(row, col).Text.text = "";
        }

        private IEnumerator PlayWinAnimation(GameResult gameResult, List<Vector2Int> winningCells)
        {
            List<TicTacToeButton> buttons = new();

            List<Vector3> originalAnchoredPositions = new();

            foreach (var item in winningCells)
            {
                var button = GetButton(item.x, item.y);
                buttons.Add(button);
                originalAnchoredPositions.Add(GetButton(item.x, item.y).RectTransform.anchoredPosition);
            }

            yield return new WaitForSeconds(0.1f);

            yield return StartCoroutine(_strikeLine.AnimateStrikeLine(buttons[0].RectTransform.anchoredPosition, buttons[2].RectTransform.anchoredPosition));

            yield return new WaitForSeconds(0.3f);


            StartCoroutine(Tweens.AnimateAnchoredPosition(_strikeLine.RectTransform, _strikeLine.RectTransform.anchoredPosition + (Vector2.zero - buttons[1].RectTransform.anchoredPosition), 0.3f));
            StartCoroutine(Tweens.FadeImage(_strikeLine.LineImage, 1, 0, 0.3f));

            StartCoroutine(Tweens.AnimateAnchoredPosition(buttons[0].RectTransform, Vector2.zero, 0.3f));
            StartCoroutine(Tweens.AnimateAnchoredPosition(buttons[1].RectTransform, Vector2.zero, 0.3f));
            StartCoroutine(Tweens.AnimateAnchoredPosition(buttons[2].RectTransform, Vector2.zero, 0.3f));

            yield return new WaitForSeconds(0.3f);

            foreach (var button in buttons)
            {
                button.Text.text = "";
            }

            var resultWindow = ServicesContainer.WindowService.Create<GameResultWindow>(GameResultWindow.WindowId);

            resultWindow.ShowResult((gameResult == GameResult.Draw) ? 
                "Draw!" : 
                (gameResult == GameResult.XWins) ? "X\nWins!" : "O\nWins!");

            yield return StartCoroutine(Tweens.FadeCanvasGroup(_boardCanvasGroup, 1, 0, 0.3f));

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].RectTransform.anchoredPosition = originalAnchoredPositions[i];
            }
        }

        private void AnimeLineSize(Vector2 horizontalLineSize, Vector2 verticalLineSize)
        {
            foreach (var line in _horizontalLines)
                StartCoroutine(Tweens.ResizeRectTransform(line, horizontalLineSize, 0.3f));

            foreach (var line in _verticalLines)
                StartCoroutine(Tweens.ResizeRectTransform(line, verticalLineSize, 0.3f));
        }

        private TicTacToeButton GetButton(int row, int col)
        {
            return _grid[(row * 3) + col];
        }
    }
}
