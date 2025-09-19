using UnityEngine;
namespace EEA.MiniGames.HoopBall
{
    public class HoopBallSpawnArea : MonoBehaviour
    {
        [SerializeField] private Vector3 _bottomLeft;
        [SerializeField] private Vector3 _topRight;

        public Vector2 SelectRandomPosition()
        {
            return new Vector2(
                Random.Range(_bottomLeft.x, _topRight.x),
                Random.Range(_bottomLeft.y, _topRight.y));
        }

        private void OnDrawGizmos()
        {
            Vector2 size = _topRight - _bottomLeft;
            Gizmos.DrawWireCube((_bottomLeft + _topRight) * 0.5f, size);
        }
    }
}