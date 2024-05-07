using UnityEngine;

namespace Extensions.Unity
{
    public static class Vector3Ext
    {
        public static bool IsBothEulOpposite(this Vector3 thisVector3, Vector3 other)
        {
            Quaternion firstQuaternion = Quaternion.Euler(thisVector3);
            Quaternion secondQuaternion = Quaternion.Euler(other);

            float dotProduct = Quaternion.Dot(firstQuaternion, secondQuaternion);
            
            return dotProduct < 0;
        }

        public static Vector3 Abs(this Vector3 vector3)
        {
            return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
        }

        public static Vector3 Pow(this Vector3 vector3, float pow)
        {
            for (int i = 0; i < 2; i ++)
            {
                vector3[i] = Mathf.Pow(vector3[i], pow);
            }

            return vector3;
        }
        
        public static Vector2 Abs(this Vector2 vector2)
        {
            return new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
        }
        
        public static Vector3Int RoundToInt(this Vector3 vector3)
        {
            Vector3Int returnVal = Vector3Int.zero;
            for (int i = 0; i < 3; i ++)
            {
                returnVal[i] = Mathf.RoundToInt(vector3[i]);
            }

            return returnVal;
        }

        public static int GetMaxAxisIndex(this Vector3 vector3)
        {
            vector3 = vector3.Abs();
            int axis = 0;
            float val = vector3[0];
            for (int i = 0; i < 3; i ++)
            {
                float axisVal = vector3[i];

                if (val < axisVal)
                {
                    axis = i;
                    val = axisVal;
                }
            }

            return axis;
        }

        public static Vector3 Right(Vector3 dir, Vector3 upVec)
        {
            return Vector3.Cross(upVec, dir).normalized;
        }
        
        public static Vector3Int Abs(this Vector3Int vector3Int)
        {
            return new Vector3Int(Mathf.Abs(vector3Int.x), Mathf.Abs(vector3Int.y), Mathf.Abs(vector3Int.z));
        }
        
        public static int GetMaxAxisIndex(this Vector3Int vector3Int)
        {
            vector3Int = vector3Int.Abs();
            int axis = 0;
            float val = vector3Int[0];
            for (int i = 0; i < 3; i ++)
            {
                float axisVal = vector3Int[i];
                if (val < axisVal)
                {
                    axis = i;
                    val = axisVal;
                }
            }

            return axis;
        }
        
        public static int GetDi(this Vector3Int vector3Int)
        {
            vector3Int = vector3Int.Abs();
            int axis = 0;
            float val = vector3Int[0];
            for (int i = 0; i < 3; i ++)
            {
                float axisVal = vector3Int[i];
                if (val < axisVal)
                {
                    axis = i;
                    val = axisVal;
                }
            }

            return axis;
        }
    }
}