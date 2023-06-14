using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Level
{
    public class Bomb : MonoBehaviour
    {
        public int timer = 10;

        private Color _bombColor = Color.green;

        private float _timerStart;

        private SpriteRenderer _spriteRenderer;

        [FormerlySerializedAs("_hingeJoint2D")] [SerializeField]
        private HingeJoint2D hingeJoint2D;

        [SerializeField] private GameObject explosionEffect;

        private Platform _platform;

        private bool _exploded;

        private void Start()
        {
            GameEvents.Instance.OnPlayerInput += DecreaseTimer;
            _timerStart = timer;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _platform = transform.parent.parent.GetComponent<Platform>();
        }

        private void Update()
        {
            var timePercent = timer / _timerStart * 100f;
            if (timePercent >= 50.0f)
                _bombColor.r = 2.0f - 2.0f * timePercent / 100.0f;
            else if (timePercent > 0.0f) // don't go below zero
                _bombColor.g = 2.0f * timePercent / 100.0f;

            _spriteRenderer.color = _bombColor;
        }

        private void DecreaseTimer()
        {
            if (_exploded) return;
            if (timer > 0)
            {
                timer--;
            }
            else
            {
                _exploded = true;
                _platform.totalBombs--;
                StartCoroutine(BombExplode());
            }
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnPlayerInput -= DecreaseTimer;
        }

        private IEnumerator BombExplode()
        {
            _spriteRenderer.enabled = false;
            AudioManager.Instance.PlayOneShot("bombExplosion");
            CinemachineShake.Instance.ShakeCamera(10f, 1f);
            var effect = Instantiate(explosionEffect, transform);
            yield return new WaitForSeconds(0.5f);
            hingeJoint2D.enabled = false;
            Destroy(effect);
            Destroy(gameObject);
        }
    }
}