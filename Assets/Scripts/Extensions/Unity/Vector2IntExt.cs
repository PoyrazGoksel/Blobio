using UnityEngine;

namespace Extensions.Unity
{
    public static class Vector2IntExt
    {
        public static Vector3 ToVector3XY(this Vector2Int thisVect2Int)
        {
            return new Vector3(thisVect2Int.x, thisVect2Int.y, 0);
        }
        
        public static Vector3 ToVector3XZ(this Vector2Int thisVect2Int)
        {
            return new Vector3(thisVect2Int.x, 0, thisVect2Int.y);
        }
    }
}