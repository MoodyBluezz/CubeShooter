using Client.Scripts.View;
using UniRx;
using UnityEngine;
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
        private const float AgentRadius = 0.2f;
        private const float DistanceOffset = 0.5f;
        
        public bool IsMovable { get; set; } = true;
        public CubeEater CubeEater { get; private set; }

        public void StartCubesMove()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => MoveCubes())
                .AddTo(this);
        }
        
        private void MoveCubes()
        {
            var agent = cubeView.CubeMeshAgent;
            agent.enabled = true;

            if (IsMovable)
            {
                cubeRigidbody.isKinematic = false;
                cubeCollider.enabled = true;
                agent.isStopped = false;
                if (!agent.pathPending && agent.remainingDistance < DistanceOffset)
                {
                    agent.SetDestination(AgentDestination());
                }
            }
            else
            {
                cubeRigidbody.isKinematic = true;
                cubeCollider.enabled = false;
                agent.radius = AgentRadius;
                agent.isStopped = true;
            }
        }

        public void ScaleCube()
        {
            SetCubeEater();
            var cubeTransform = transform;
            cubeTransform.localScale *= DoubleScale;
            transform.localScale = Vector3.Lerp(transform.localScale, cubeTransform.localScale, ScaleTime * Time.deltaTime);
        }

        private Vector3 AgentDestination()
        {
            var agentPosition = transform.position;
            var moveRandomPosX = Random.Range(agentPosition.x - 10, agentPosition.x + 10);
            var moveRandomPosZ = Random.Range(agentPosition.z - 10, agentPosition.z + 10);
            return new Vector3(moveRandomPosX, agentPosition.y, moveRandomPosZ);
        }

        
        private void SetCubeEater()
        {
            if (GetComponent<CubeEater>() == null)
                CubeEater = gameObject.AddComponent<CubeEater>();
            
            cubeView.CubeMeshAgent.speed = AgentSpeed;
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