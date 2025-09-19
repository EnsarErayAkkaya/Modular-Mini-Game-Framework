using UnityEngine;
namespace EEA.MiniGames.HoopBall
{
    public struct OnBallShoot
    {
        public bool Success;
        public Vector3 Force;
    }

    public struct OnPlayerDrag
    {
        public float Percent;
        public Vector3 Force;
    }

    public struct OnBallIdle
    {
        public bool HasScored;
    }

    public struct OnBallScored
    { }

    public struct OnScoreChanged
    {
        public int Score;
    }
}