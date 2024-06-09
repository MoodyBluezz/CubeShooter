using UniRx;
using UnityEngine;

namespace Client.Scripts.Core
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            
            Observable.EveryUpdate()
                .Subscribe(_ => FaceCamera())
                .AddTo(this);
        }

        private void FaceCamera()
        {
            if (_mainCamera != null)
            {
                transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
                    _mainCamera.transform.rotation * Vector3.up);
            }
        }
    }
}