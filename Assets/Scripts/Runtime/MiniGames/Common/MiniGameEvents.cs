namespace EEA.MiniGames.Common
{
    public struct OnMiniGameStarted
    {
        public string GameId;
        public OnMiniGameStarted(string gameId)
        {
            GameId = gameId;
        }
    }
    public struct OnMiniGameEnded
    {
        public string GameId;
        public OnMiniGameEnded(string gameId)
        {
            GameId = gameId;
        }
    }
}
