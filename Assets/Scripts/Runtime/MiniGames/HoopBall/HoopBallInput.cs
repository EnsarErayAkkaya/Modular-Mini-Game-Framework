using UnityEngine;

namespace EEA.MiniGames.HoopBall
{
    public class HoopBallInput : MonoBehaviour
    {
        [SerializeField] private float _maxPower = 15f;

        // Input state
        private Vector2 _dragStartScreen;
        private bool _dragging;
        private bool _isEnabled;

        private OnPlayerDrag _cachedOnPlayerDrag;

        private void Update()
        {
            if (!_isEnabled) return;

            if (Input.GetMouseButtonDown(0))
            {
                _dragging = true;
                _dragStartScreen = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0) && _dragging)
            {
                var dragEnd = (Vector2)Input.mousePosition;
                HandleRelease(_dragStartScreen, dragEnd);
                _dragging = false;
            }

            // While dragging show power
            if (_dragging)
            {
                Vector2 curr = Input.mousePosition;
                
                _cachedOnPlayerDrag.Percent = ComputePowerPercent(_dragStartScreen, curr);
                _cachedOnPlayerDrag.Force = ComputeDirection(_dragStartScreen, curr);
                MiniGameService.CurrentGame.EventBus.Publish(_cachedOnPlayerDrag);
            }
        }

        private void HandleRelease(Vector2 start, Vector2 end)
        {
            Vector2 delta = start - end; // drag vector on screen
            
            float percent = Mathf.Clamp(delta.magnitude / (Screen.height * 0.4f), 0f, 1f);

            if (percent < 0.1f)
            {
                MiniGameService.CurrentGame.EventBus.Publish(new OnBallShoot { Force = Vector3.zero, Success = false});
                return;
            }

            float power = Mathf.Clamp(delta.magnitude / (Screen.height * 0.4f), 0f, 1f) * _maxPower;

            Vector3 launch = delta.normalized * power; // add vertical boost

            MiniGameService.CurrentGame.EventBus.Publish(new OnBallShoot { Force = launch, Success = true });
        }

        private float ComputePowerPercent(Vector2 start, Vector2 curr)
        {
            Vector2 delta = start - curr;

            return Mathf.Clamp(delta.magnitude / (Screen.height * 0.4f), 0f, 1f);
        }

        private Vector2 ComputeDirection(Vector2 start, Vector2 end)
        {
            return (start - end).normalized;
        }

        public void ToggleInput(bool enable)
        {
            _isEnabled = enable;
        }
    }
}