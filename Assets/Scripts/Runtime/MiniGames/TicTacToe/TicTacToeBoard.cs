using System.Collections.Generic;
using UnityEngine;

namespace EEA.MiniGames.TicTacToe
{
    public enum Player { None, X, O }
    public enum GameResult { InProgress, Draw, XWins, OWins }

    public class TicTacToeBoard
    {
        private readonly Player[,] _board;
        public Player CurrentPlayer { get; private set; } = Player.X;
        public GameResult Result { get; private set; } = GameResult.InProgress;

        private OnMoveMade _cachedOnMoveMade = new();

        public TicTacToeBoard()
        {
            _board = new Player[3, 3];
        }

        public void Reset()
        {
            CurrentPlayer = Player.X;
            Result = GameResult.InProgress;

            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    _board[r, c] = Player.None;
        }

        public bool MakeMove(int row, int col)
        {
            if (Result != GameResult.InProgress || _board[row, col] != Player.None)
                return false;

            _board[row, col] = CurrentPlayer;

            _cachedOnMoveMade.Row = row;
            _cachedOnMoveMade.Col = col;
            _cachedOnMoveMade.Player = CurrentPlayer;
            MiniGameService.CurrentGame.EventBus.Publish(_cachedOnMoveMade);

            if (CheckWin(CurrentPlayer, out List<Vector2Int> cells))
            {
                Result = CurrentPlayer == Player.X ? GameResult.XWins : GameResult.OWins;
                MiniGameService.CurrentGame.EventBus.Publish(new OnGameOver(Result, cells));
            }
            else if (IsDraw())
            {
                Result = GameResult.Draw;
                MiniGameService.CurrentGame.EventBus.Publish(new OnGameOver(Result, cells));
            }
            else
            {
                CurrentPlayer = (CurrentPlayer == Player.X) ? Player.O : Player.X;
            }

            return true;
        }

        private bool CheckWin(Player player, out List<Vector2Int> cells)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_board[i, 0] == player && _board[i, 1] == player && _board[i, 2] == player)
                {
                    cells = new List<Vector2Int> { new Vector2Int(i, 0), new Vector2Int(i, 1), new Vector2Int(i, 2) };
                    return true;
                }

                if (_board[0, i] == player && _board[1, i] == player && _board[2, i] == player)
                {
                    cells = new List<Vector2Int> { new Vector2Int(0, i), new Vector2Int(1, i), new Vector2Int(2, i) };
                    return true;
                }
            }
            if (_board[0, 0] == player && _board[1, 1] == player && _board[2, 2] == player)
            {
                cells = new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(2, 2) };
                return true;
            }
            if (_board[0, 2] == player && _board[1, 1] == player && _board[2, 0] == player)
            {
                cells = new List<Vector2Int> { new Vector2Int(0, 2), new Vector2Int(1, 1), new Vector2Int(2, 0) };
                return true;
            }

            cells = null;
            return false;
        }

        private bool IsDraw()
        {
            foreach (var cell in _board)
                if (cell == Player.None)
                    return false;
            return true;
        }
    }
}
