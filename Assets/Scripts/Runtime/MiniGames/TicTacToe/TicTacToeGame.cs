using EEA.MiniGames.Common;
using EEA.Services;
using EEA.Services.Events;
using EEA.Services.SceneServices;
using System;
using UnityEngine;

namespace EEA.MiniGames.TicTacToe
{
    public class TicTacToeGame : MonoBehaviour, IMiniGame
    {
        [SerializeField] private MiniGameData _ticTacToeData;
        [SerializeField] private TicTacToeView _view;

        private TicTacToeBoard _board;
        private EventBus _eventBus = new();

        public string Id => _ticTacToeData.Id;
        public string DisplayName => _ticTacToeData.DisplayName;
        public EventBus EventBus => _eventBus;

        public void Initialize()
        {
            _board = new TicTacToeBoard();

            _view.Init(OnCellClicked);
            _eventBus.Subscribe<OnMoveMade>(HandleMoveMade);
            _eventBus.Subscribe<OnGameOver>(HandleGameOver);

            _view.ResetView();

            ServicesContainer.EventBus.Publish(new OnMiniGameStarted(_ticTacToeData.Id));
        }

        public void StartGame()
        {
            _view.UpdateStatus("Player X's turn!");
        }

        public void EndGame()
        {
            _eventBus.Clear();
            ServicesContainer.EventBus.Publish(new OnMiniGameEnded(_ticTacToeData.Id));
        }

        public void Restart()
        {
            _view.ResetView();
            _board.Reset();
        }

        private void OnCellClicked(int row, int col)
        {
            _board.MakeMove(row, col);
        }

        private void HandleMoveMade(OnMoveMade data)
        {
            _view.UpdateCell(data.Row, data.Col, data.Player);
            _view.UpdateStatus($"Player {(_board.CurrentPlayer == Player.X ? "O" : "X")}'s turn!");
        }

        private void HandleGameOver(OnGameOver data)
        {
            string message = data.Result switch
            {
                GameResult.XWins => "Player X Wins!",
                GameResult.OWins => "Player O Wins!",
                GameResult.Draw => "It's a Draw!",
                _ => ""
            };

            _view.UpdateStatus(message);
        }
    }
}
