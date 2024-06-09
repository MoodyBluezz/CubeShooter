using System;
using System.Collections;
using Client.Scripts.View;
using UnityEngine;

namespace Client.Scripts.Core
{
    public class Bullet : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody bulletRigidbody;
        private CubeView _cubeView;
        private string _counter = "";
        private BulletPool _bulletPool;
        public event Action<CubeView, Bullet> BulletHit;

       
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out _cubeView))
            {
                BulletHit?.Invoke(_cubeView, this);
                _bulletPool.ReturnBullet(this);
            }
            else
            {
                OnBulletMissed();
            }
        }

        public void GetBulletCounter(string counter)
        {
            _counter = counter;
        }
        
        public void Initialize(BulletPool bulletPool)
        {
            _bulletPool = bulletPool;
        }
        
        private void OnBulletMissed()
        {
            StartCoroutine(ReturnBulletAfterDelay(1f));
        }
        
        private IEnumerator ReturnBulletAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            _bulletPool.ReturnBullet(this);
        }
    }
}