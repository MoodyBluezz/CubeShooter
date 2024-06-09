using System.Collections.Generic;
using Client.ScriptableObjects;
using Client.Scripts.Controller;
using Client.Scripts.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.Scripts.Core
{
    public class CubeGenerator : MonoBehaviour, ICubeGenerator
    {
        [SerializeField] private CubeView cubeViewPrefab;

        private const float SpawnYAxis = 10f;
        public List<CubeController> CubeControllers { get; } = new();
        
        public void GenerateCubes()
        {
            var cubeCount = Random.Range(5, 15);
            
            for (int i = 0; i < cubeCount; i++)
            {
                var data = ScriptableObject.CreateInstance<CubeData>();
                data.Position = GetRandomPosition();
                data.Rotation = Quaternion.identity;
                data.Counter = i + 1;
                data.Color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                
                var cubeObject = Instantiate(cubeViewPrefab, data.Position, data.Rotation, transform);
                var controller = new CubeController(data, cubeObject);
                
                CubeControllers.Add(controller);
            }
        }

        private static Vector3 GetRandomPosition()
        {
            var x = Random.Range(-19f, 19f);
            var z = Random.Range(8f, 26f);
            return new Vector3(x, SpawnYAxis, z);
        }
    }
}