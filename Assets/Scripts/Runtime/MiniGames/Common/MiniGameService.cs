using EEA.MiniGames.Common;
using EEA.Services;
using EEA.Services.SceneServices;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.MiniGames
{
    public class MiniGameService : MonoBehaviour
    {
        [SerializeField] private MiniGameServiceSettings _settings;
        private IMiniGame currentGame;

        public MiniGameServiceSettings Settings => _settings;

        public static MiniGameService Instance { get; private set; }
        public static IMiniGame CurrentGame => Instance.currentGame;

        private void Start()
        {
            Instance = this;
            ServicesContainer.EventBus.Subscribe<OnMiniGameEnded>(OnMiniGameEnded);
            ServicesContainer.EventBus.Subscribe<OnMiniGameStarted>(OnMiniGameStarted);
        }

        public async Task<IMiniGame> LoadGame(string gameId)
        {
            var miniGameData = _settings.MiniGames.FirstOrDefault(g => g.Id == gameId);
            if (miniGameData == null)
            {
                GameLogger.Log($"MiniGame {gameId} not found!");
                return null;
            }

            GameObject miniGameScene = await ServicesContainer.SceneService.LoadScene(miniGameData.SceneConfig.SceneKey);

            currentGame = miniGameScene.GetComponent<IMiniGame>();

            currentGame?.Initialize();
            return currentGame;
        }
        public void OnMiniGameEnded(OnMiniGameEnded obj)
        {
            _ = EndGame();
        }

        public void OnMiniGameStarted(OnMiniGameStarted obj)
        {
            StartGame();
        }

        public void StartGame() => currentGame?.StartGame();


        public async Task EndGame()
        {
            if (currentGame == null) return;

            var miniGameData = _settings.MiniGames.FirstOrDefault(g => g.Id == currentGame.Id);
            if (miniGameData == null)
            {
                return;
            }

            await ServicesContainer.SceneService.RemoveScene(miniGameData.SceneConfig.SceneKey);

            _ = ServicesContainer.SceneService.LoadScene(SceneKeys.MenuScene);

            currentGame = null;
        }
    }
}