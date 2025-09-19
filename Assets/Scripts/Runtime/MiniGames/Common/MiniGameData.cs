using EEA.Services.SceneServices;
using UnityEngine;

namespace EEA.MiniGames
{
    [CreateAssetMenu(fileName = "MiniGameData", menuName = "MiniGames/MiniGameData", order = 1)]
    public class MiniGameData : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public Sprite MiniGameIcon;
        public SceneConfig SceneConfig;
    }
}