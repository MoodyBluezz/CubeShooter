using Client.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Client.Scripts.View
{
    public class CubeView : MonoBehaviour
    {
        [field: SerializeField] public NavMeshAgent CubeMeshAgent { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CubeCounter { get; private set; }
        [field: SerializeField] public CubeMover CubeMover { get; set; }

        [SerializeField] private Renderer cubeRenderer;
        
        public void SetColor(Color color)
        {
            cubeRenderer.material.color = color;
        }

        public void UpdateCounter(int counter)
        {
            CubeCounter.text = counter.ToString();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}