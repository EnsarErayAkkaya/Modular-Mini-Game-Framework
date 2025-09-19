using EEA.MiniGames.Common;
using EEA.MiniGames.TicTacToe;
using EEA.Services;
using EEA.Services.Events;
using EEA.Services.SaveServices;
using EEA.Shared;
using System;
using UnityEditor.Media;
using UnityEngine;

namespace EEA.MiniGames.HoopBall
{
    public class HoopBallGame : MonoBehaviour, IMiniGame
    {
        [SerializeField] private MiniGameData _hoopBallData;
        [SerializeField] private HoopBallSpawnArea _spawnArea;
        [SerializeField] private HoopBallBall _currentBall;
        [SerializeField] private HoopBallInput _input;

        [SerializeField] private HoopBallView _view;

        private EventBus _eventBus = new();

        private int _score = 0;
        private int _highScore = 0;

        private const string HighScoreKey = "_high_score";
        private string HoopBallHighScoreString => Id + HighScoreKey;

        public string Id => _hoopBallData.Id;
        public string DisplayName => _hoopBallData.DisplayName;
        public EventBus EventBus => _eventBus;
        public void Initialize()
        {
            _score = 0;
            _highScore = ServicesContainer.SaveService.Raw.LoadInt(HoopBallHighScoreString);

            _eventBus.Subscribe<OnBallIdle>(OnBallIdle);
            _eventBus.Subscribe<OnBallShoot>(OnBallShoot);
            _eventBus.Subscribe<OnBallScored>(OnBallScored);

            ServicesContainer.EventBus.Publish(new OnMiniGameStarted(_hoopBallData.Id));

            _view.ResetView(_highScore);
        }

        private void OnBallShoot(OnBallShoot data)
        {
            if (data.Success)
                _currentBall.Launch(data.Force);
        }

        private void OnBallIdle(OnBallIdle data)
        {
            SpawnBall();
            if (_score > 0 && !data.HasScored)
            {
                _input.ToggleInput(false);
                ShowResultWindow($"Score\n{_score}");
            }
        }

        private void ShowResultWindow(string text)
        {
            var resultWindow = ServicesContainer.WindowService.Create<GameResultWindow>(GameResultWindow.WindowId);

            resultWindow.ShowResult(text);
        }

        private void SpawnBall()
        {
            Vector2 spawnPos = _spawnArea.SelectRandomPosition();
            _currentBall.Reset();
            _currentBall.transform.position = spawnPos;
        }

        public void StartGame()
        {
            ResetGame();
        }

        public void EndGame()
        {
            _eventBus.Clear();
            ServicesContainer.EventBus.Publish(new OnMiniGameEnded(_hoopBallData.Id));
        }

        public void Restart()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            _score = 0;
            _view.ResetView(_highScore);
            SpawnBall();
            _input.ToggleInput(true);
        }

        private void OnBallScored(OnBallScored evt)
        {
            _score += 1;

            if (_score > _highScore)
            {
                _highScore = _score;
                ServicesContainer.SaveService.Raw.Save(HoopBallHighScoreString, _highScore.ToString());
            }

            _view.UpdateScore(_score, _highScore);
        }
    }
}