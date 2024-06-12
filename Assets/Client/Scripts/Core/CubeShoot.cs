using System;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Controller;
using Client.Scripts.View;
using TMPro;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.Scripts.Core
{
    public class CubeShoot : MonoBehaviour, ICubeShooter
    {
        [SerializeField] private CubeGenerator cubeGenerator;
        [SerializeField] private BulletPool bulletPool;
        [SerializeField] private TextMeshProUGUI playerPhrase;
        [SerializeField] private float bulletSpeed = 5f;
        
        private readonly HashSet<CubeController> _hitCubes = new();
        private readonly HashSet<CubeController> _missedCubes = new();
        private CubeController _targetCube;
        private bool _isShooting;
        private Bullet _bullet;
        private bool _isMissed;
        
        public void StartShooting()
        {
            _isShooting = true;
        }

        private void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(2))
                .Where(_ => _isShooting)
                .Repeat()
                .Subscribe(_ => SelectTargetCube())
                .AddTo(this);
        }
        
        private void SelectTargetCube()
        {
            if (_hitCubes.Count >= cubeGenerator.CubeControllers.Count)
            {
                _isShooting = false;
                playerPhrase.text = $"All {_hitCubes.Count} cubes are stopped!";
                _hitCubes.Clear();
                return;
            }

            var cubes = cubeGenerator.CubeControllers;

            if (_isMissed && _missedCubes.Count > 0)
            {
                _targetCube = null;
                foreach (var missedCube in _missedCubes)
                {
                    _targetCube = missedCube;
                    break;
                }
            }
            else
            {
                do
                {
                    _targetCube = GetRandomUnHitCube(cubes);
                } while (_hitCubes.Contains(_targetCube) && _hitCubes.Count < cubes.Count);
            }

            if (_targetCube == null) return;
            
            playerPhrase.text = $"I will shoot cube number: {_targetCube.View.CubeCounter.text}";

            ShootCube();
        }
        
        private CubeController GetRandomUnHitCube(List<CubeController> cubes)
        {
            var unHitCubes = cubes.Where(c => !_hitCubes.Contains(c)).ToList();
            return unHitCubes.Count > 0 ? unHitCubes[Random.Range(0, unHitCubes.Count)] : null;
        }
        
        private void ShootCube()
        {
            _bullet = bulletPool.GetBullet();
            _bullet.Initialize(bulletPool);
            _bullet.BulletHit += OnBulletHit;
            _bullet.GetBulletCounter(_targetCube.View.CubeCounter.text);

            var bulletStartPosition = _bullet.transform.position;
            var targetPosition = _targetCube.View.transform.position;
            var targetVelocity = _targetCube.View.CubeMeshAgent.velocity;

            var timeToReach = Vector3.Distance(bulletStartPosition, targetPosition) / bulletSpeed;
            var predictedTargetPosition = targetPosition + (targetVelocity * timeToReach);
            var bulletDirection = (predictedTargetPosition - bulletStartPosition).normalized;
            var bulletVelocity = bulletDirection * bulletSpeed;

            _bullet.bulletRigidbody.velocity = bulletVelocity;
            _bullet.bulletRigidbody.angularVelocity = Vector3.zero;
            _bullet.bulletRigidbody.useGravity = false;
        }

        private void OnBulletHit(CubeView cubeView, Bullet bullet)
        {
            if (_targetCube != null && _targetCube.View != null && _targetCube.View.CubeCounter.text == cubeView.CubeCounter.text)
            {
                _isMissed = false;
                _missedCubes.Clear();
                _hitCubes.Add(_targetCube);
                cubeView.CubeMover.IsMovable = false;
            }
            else
            {
                _isMissed = true;
                _missedCubes.Add(_targetCube);
            }
            bullet.BulletHit -= OnBulletHit;
        }
    }
}