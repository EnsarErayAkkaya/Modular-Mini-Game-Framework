
using System.Collections.Generic;
using UnityEngine;

namespace EEA.MiniGames.TicTacToe
{
    public struct OnMoveMade
    {
        public int Row;
        public int Col;
        public Player Player;

        public OnMoveMade(int row, int col, Player player)
        {
            Row = row;
            Col = col;
            Player = player;
        }
    }

    public struct OnGameOver
    {
        public GameResult Result;
        public List<Vector2Int> WinningCells;

        public OnGameOver(GameResult result, List<Vector2Int> winningCells)
        {
            Result = result;
            WinningCells = winningCells;
        }
    }
}