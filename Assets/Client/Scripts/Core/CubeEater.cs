using Client.Scripts.Controller;
using Client.Scripts.View;
using UniRx;
using UnityEngine;

namespace Client.Scripts.Core
{
    public class CubeEater : MonoBehaviour
    {
        private CubeGenerator _cubeGenerator;
        private CubeController _closestCube;
        private CubeController _mainCube;

        private void Awake()
        {
            _cubeGenerator = GetComponentInParent<CubeGenerator>();
        }

        public void StartCubeEater(CubeController mainCube)
        {
            _mainCube = mainCube;
            Observable.EveryUpdate()
                .Subscribe(_ => DestroyCubesRoutine(_mainCube))
                .AddTo(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent<CubeView>(out var cubeView)) return;
            if (_closestCube.View.CubeCounter.text != cubeView.CubeCounter.text) return;
            Destroy(_closestCube.View.gameObject);
            _cubeGenerator.CubeControllers.Remove(_closestCube);
        }

        private void DestroyCubesRoutine(CubeController cubeController)
        {
            if (_mainCube == null || _cubeGenerator.CubeControllers.Count <= 1)
                return;

            _closestCube = GetClosestCube(_mainCube);

            if (_closestCube == null)
                return;
            
            var agent = cubeController.View.CubeMeshAgent;
            if (agent != null && agent.isOnNavMesh)
            {
                agent.SetDestination(_closestCube.View.transform.position);
            }
            else
            {
                Debug.LogWarning("Agent is not on NavMesh or is null");
            }
        }

        private CubeController GetClosestCube(CubeController currentCube)
        {
            CubeController closestCube = null;
            var minDistance = float.MaxValue;
            foreach (var cube in _cubeGenerator.CubeControllers)
            {
                if (cube == currentCube) continue;
                var distance = Vector3.Distance(currentCube.View.transform.position, cube.View.transform.position);
                if (!(distance < minDistance)) continue;
                minDistance = distance;
                closestCube = cube;
            }
            return closestCube;
        }
    }
}
