using UnityEngine;

namespace EEA.MiniGames.HoopBall
{
    public class HoopBallPowerGauge : MonoBehaviour
    {
        [SerializeField] private Transform _powerBar;
        [SerializeField] private float _maxLength = 5;

        private void Start()
        {
            MiniGameService.CurrentGame.EventBus.Subscribe<OnPlayerDrag>(OnPlayerDrag);
            MiniGameService.CurrentGame.EventBus.Subscribe<OnBallShoot>(OnBallShoot);
        }

        private void OnPlayerDrag(OnPlayerDrag evt)
        {
            transform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(evt.Force.y, evt.Force.x) * Mathf.Rad2Deg) - 90);

            _powerBar.localScale = new Vector3(1f, evt.Percent * _maxLength, 1f);
            _powerBar.localPosition = new Vector3(0, evt.Percent * _maxLength * 0.5f, 0);
        }

        private void OnBallShoot(OnBallShoot evt)
        {
            _powerBar.localScale = new Vector3(1f, 0f, 1f);
        }
    }
}