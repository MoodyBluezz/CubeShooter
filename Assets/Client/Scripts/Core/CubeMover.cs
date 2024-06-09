using System;
using System.Collections;
using Client.Scripts.View;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Client.Scripts.Core
{
    public class CubeMover : MonoBehaviour, ICubeMover
    {
        [SerializeField] private Rigidbody cubeRigidbody;
        [SerializeField] private CubeView cubeView;
        [SerializeField] private Collider cubeCollider;

        private const int DoubleScale = 2;
        private const float ScaleTime = 1;
        private const float AgentSpeed = 10f;
        private const float DistanceOffset = 0.5f;

        private NavMeshAgent _agent;
        private Vector3 _targetPosition;
        private IDisposable _movementSubscription;

        public bool IsMovable { get; set; } = true;
        public CubeEater CubeEater { get; private set; }

        private void Awake()
        {
            _agent = cubeView.CubeMeshAgent;
        }

        public void StartCubesMove()
        {
            if (_movementSubscription != null)
            {
                _movementSubscription.Dispose();
            }

            _movementSubscription = Observable.EveryUpdate()
                .Subscribe(_ => MoveCubes())
                .AddTo(this);
        }

        private void MoveCubes()
        {
            if (_agent == null)
            {
                Debug.LogError("NavMeshAgent is not assigned!");
                return;
            }

            _agent.enabled = true;

            if (IsMovable)
            {
                cubeRigidbody.isKinematic = false;
                cubeCollider.enabled = true;
                _agent.isStopped = false;

                if (!_agent.pathPending && _agent.remainingDistance < DistanceOffset)
                {
                    SetNewDestination();
                }
            }
            else
            {
                cubeRigidbody.isKinematic = true;
                cubeCollider.enabled = false;
                _agent.isStopped = true;
            }
        }

        private void SetNewDestination()
        {
            _targetPosition = GenerateRandomPosition();
            _agent.SetDestination(_targetPosition);
        }

        private Vector3 GenerateRandomPosition()
        {
            var agentPosition = transform.position;
            var moveRandomPosX = Random.Range(agentPosition.x - 10, agentPosition.x + 10);
            var moveRandomPosZ = Random.Range(agentPosition.z - 10, agentPosition.z + 10);
            return new Vector3(moveRandomPosX, agentPosition.y, moveRandomPosZ);
        }

        public void ScaleCube()
        {
            SetCubeEater();
            var cubeTransform = transform;
            var targetScale = cubeTransform.localScale * DoubleScale;
            StartCoroutine(ScaleOverTime(cubeTransform, targetScale, ScaleTime));
        }

        private IEnumerator ScaleOverTime(Transform target, Vector3 toScale, float duration)
        {
            var currentTime = 0f;
            var initialScale = target.localScale;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                target.localScale = Vector3.Lerp(initialScale, toScale, currentTime / duration);
                yield return null;
            }

            target.localScale = toScale;
        }

        private void SetCubeEater()
        {
            if (GetComponent<CubeEater>() == null)
                CubeEater = gameObject.AddComponent<CubeEater>();
            
            _agent.speed = AgentSpeed;
        }

        public void ResetMobility()
        {
            IsMovable = true;
        }

        private void CleanUp()
        {
            if (cubeView.CubeMeshAgent != null)
            {
                Destroy(cubeView.CubeMeshAgent);
            }
        }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}