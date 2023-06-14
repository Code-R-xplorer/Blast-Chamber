using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Level
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private List<int> bombLengths;

        [SerializeField] private GameObject explosionEffect;

        public int totalBombs;

        private float _platformWidth;

        private void Awake()
        {
            var bombs = new List<Bomb>();
            totalBombs = transform.GetChild(0).childCount;
            transform.GetChild(0).GetComponentsInChildren(bombs);
            for (var i = 0; i < bombLengths.Count; i++) bombs[i].timer = bombLengths[i];
            _platformWidth = GetComponent<BoxCollider2D>().size.x;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.CompareTag("Ground") || (col.collider.CompareTag("Platform") && totalBombs == 0))
                StartCoroutine(ExplodePlatform());
        }

        private IEnumerator ExplodePlatform()
        {
            AudioManager.Instance.PlayOneShot("bombExplosion");
            CinemachineShake.Instance.ShakeCamera(10f, 1.5f);
            var position = transform.position;
            var newPos = new Vector3(position.x + _platformWidth / 2, position.y, position.z);
            var effect = Instantiate(explosionEffect, newPos, transform.rotation, transform);
            yield return new WaitForSeconds(0.5f);
            Destroy(effect);
            Destroy(gameObject);
        }
    }
}