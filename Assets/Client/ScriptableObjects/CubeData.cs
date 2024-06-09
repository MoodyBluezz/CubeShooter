using UnityEngine;

namespace Client.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CubeData", menuName = "ScriptableObjects/CubeData", order = 1)]
    public class CubeData : ScriptableObject
    {
        [field: SerializeField] public int Counter;
        [field: SerializeField] public Vector3 Position;
        [field: SerializeField] public Quaternion Rotation;
        [field: SerializeField] public Color Color;
    }
}