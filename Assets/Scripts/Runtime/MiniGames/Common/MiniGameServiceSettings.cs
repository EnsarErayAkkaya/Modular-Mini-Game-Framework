using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.MiniGames
{
    [CreateAssetMenu(fileName = "MiniGameServiceSettings", menuName = "MiniGames/MiniGameServiceSettings")]
    public class MiniGameServiceSettings : ScriptableObject
    {
        public List<MiniGameData> MiniGames;
    }
}