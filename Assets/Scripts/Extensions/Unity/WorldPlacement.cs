using System;
using UnityEngine;

namespace Extensions.Unity
{
    [Serializable]
    public class WorldPlacement
    {
        public Vector3 Position;
        public Vector3 Rotation;

        public WorldPlacement(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.eulerAngles;
        }
        
        public WorldPlacement(WorldPlacement worldPlacement)
        {
            Position = worldPlacement.Position;
            Rotation = worldPlacement.Rotation;
        }

        public WorldPlacement(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public void PasteDataTo(Transform transform)
        {
            transform.position = Position;
            transform.eulerAngles = Rotation;
        }
    }
}