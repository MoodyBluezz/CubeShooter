using Client.Scripts.Core;
using UnityEngine;

namespace Client.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CubeGenerator cubeGenerator;
        [SerializeField] private CubeShoot cubeShoot;
        [SerializeField] private UIView uiView;

        private void Start()
        {
            uiView.Initialize(cubeGenerator, cubeShoot);
        }
    }
}