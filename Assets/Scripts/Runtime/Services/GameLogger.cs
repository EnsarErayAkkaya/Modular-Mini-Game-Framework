using UnityEngine;

namespace EEA.Services
{
    public static class GameLogger
    {
        public static string LogHistory = "";
        public static void Log(string message)
        {
            LogHistory += message + "\n";
            Debug.Log(message);
        }
    }
}