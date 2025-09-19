using EEA.Services.Events;
using System;
using UnityEngine;

namespace EEA.MiniGames
{
    public interface IMiniGame
    {
        string Id { get; }
        string DisplayName { get; }

        void Initialize();
        void StartGame();
        void Restart();
        void EndGame();

        EventBus EventBus { get; }
    }
}