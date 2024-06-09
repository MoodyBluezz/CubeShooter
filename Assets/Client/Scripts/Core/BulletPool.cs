using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Core
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField] private Transform bulletShooter;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int poolSize = 10;

        private readonly Queue<Bullet> _bullets = new Queue<Bullet>();

        private void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var bullet = Instantiate(bulletPrefab, bulletShooter);
                bullet.gameObject.SetActive(false);
                _bullets.Enqueue(bullet);
            }
        }

        public Bullet GetBullet()
        {
            if (_bullets.Count > 0)
            {
                var bullet = _bullets.Dequeue();
                bullet.gameObject.SetActive(true);
                return bullet;
            }
            else
            {
                var bullet = Instantiate(bulletPrefab, bulletShooter);
                bullet.Initialize(this);
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }

        public void ReturnBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            bullet.bulletRigidbody.velocity = Vector3.zero;
            bullet.transform.position = bulletShooter.position;
            bullet.transform.rotation = bulletShooter.rotation;
            _bullets.Enqueue(bullet);
        }
    }
}