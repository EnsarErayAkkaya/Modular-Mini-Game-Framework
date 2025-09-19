using UnityEngine;

namespace EEA.MiniGames.HoopBall
{
    public class HoopBallBall : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;
        private Rigidbody2D _rb;
        private bool _scored = false;
        private bool _hasShoot = false;

        private float _hoopUpColliderTouchTime;
        private float _shootTime;

        private OnBallScored _cachedOnBallScored;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            Reset();
        }

        public void Launch(Vector3 force)
        {
            _trail.enabled = true;
            _scored = false;
            _rb.isKinematic = false;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = 0;
            _shootTime = Time.time;
            _hasShoot = true;
            _rb.AddForce(force, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Hoop scoring zone should have tag "HoopTrigger"
            if (!_scored && other.TryGetComponent<HoopBallHoop>(out var hoop))
            {
                if (hoop.IsUpCollider)
                {
                    _hoopUpColliderTouchTime = Time.time;
                }
                else
                {
                    if (Time.time - _hoopUpColliderTouchTime < 1)
                    {
                        _scored = true;
                        MiniGameService.CurrentGame.EventBus.Publish(_cachedOnBallScored);

                    }
                }
            }
        }

        private void Update()
        {
            if (_hasShoot && (_rb.IsSleeping() || Time.time - _shootTime > 5f))
            {
                _trail.enabled = false;
                MiniGameService.CurrentGame.EventBus.Publish(new OnBallIdle { HasScored = _scored});
            }
        }

        public void Reset()
        {
            _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = 0;
            _scored = false;
            _hasShoot = false;

            _hoopUpColliderTouchTime = 0;
        }
    }
}