using Client.ScriptableObjects;
using UnityEngine;

namespace Client.Scripts.Model
{
    public class CubeModel
    {
        public int Counter { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Color Color { get; private set; }

        public CubeModel(CubeData data)
        {
            Position = data.Position;
            Rotation = data.Rotation;
            Counter = data.Counter;
            Color = data.Color;
        }
    }
}