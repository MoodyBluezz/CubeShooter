using Client.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private Button createButton;
        [SerializeField] private Button moveButton;
        [SerializeField] private Button shootButton;
        [SerializeField] private Button destroyButton;
        
        private ICubeGenerator _cubeGenerator;
        private ICubeShooter _cubeShooter;
        private CubeController _cubeEater;

        public void Initialize(ICubeGenerator cubeGenerator, ICubeShooter cubeShooter)
        {
            _cubeGenerator = cubeGenerator;
            _cubeShooter = cubeShooter;
        }

        private void OnEnable()
        {
            createButton.onClick.AddListener(OnCreateButtonClicked);
            moveButton.onClick.AddListener(OnMoveButtonClicked);
            shootButton.onClick.AddListener(OnShootButtonClicked);
            destroyButton.onClick.AddListener(OnDestroyButtonClicked);
        }

        private void OnDisable()
        {
            createButton.onClick.AddListener(OnCreateButtonClicked);
            moveButton.onClick.AddListener(OnMoveButtonClicked);
            shootButton.onClick.AddListener(OnShootButtonClicked);
            destroyButton.onClick.AddListener(OnDestroyButtonClicked);
        }

        private void OnCreateButtonClicked()
        {
            _cubeGenerator.GenerateCubes();
            createButton.interactable = false;
        }

        private void OnMoveButtonClicked()
        {
            foreach (var cubeController in _cubeGenerator.CubeControllers)
            {
                cubeController.View.CubeMover.StartCubesMove();
            }
            moveButton.interactable = false;
        }
    
        private void OnShootButtonClicked()
        {
            _cubeShooter.StartShooting();
            shootButton.interactable = false;
        }
    
        private void OnDestroyButtonClicked()
        {
            foreach (var cubeController in _cubeGenerator.CubeControllers)
            {
                cubeController.View.CubeMover.ResetMobility();
                cubeController.View.CubeMover.StartCubesMove();
            }

            SetCubeEater();
            destroyButton.interactable = false;
        }

        private void SetCubeEater()
        {
            if (_cubeGenerator.CubeControllers.Count == 0) return;
            var randomCube = Random.Range(0,_cubeGenerator.CubeControllers.Count);
            _cubeEater = _cubeGenerator.CubeControllers[randomCube];
            _cubeEater.View.CubeMover.ScaleCube();
            _cubeEater.View.CubeMover.CubeEater.StartCubeEater(_cubeEater);
        }
    }
}