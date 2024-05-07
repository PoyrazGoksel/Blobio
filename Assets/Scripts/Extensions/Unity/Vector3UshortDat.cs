using System;
using UnityEngine;

namespace Extensions.Unity
{
    [Serializable]
    public struct Vector3UshortDat
    {
        public ushort X;
        public ushort Y;
        public ushort Z;

        public Vector3UshortDat(Vector3Int vector3Int)
        {
            if (vector3Int[0] < 0 ||
                vector3Int[1] < 0 ||
                vector3Int[2] < 0)
            {
                Debug.LogWarning("Alert need a guard");
            }

            X = (ushort)vector3Int.x;
            Y = (ushort)vector3Int.y;
            Z = (ushort)vector3Int.z;
        }

        public Vector3Int GetVector3Int()
        {
            return new Vector3Int(X, Y, Z);
        }
    }
}