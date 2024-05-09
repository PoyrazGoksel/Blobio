using UnityEngine;

namespace Utils
{
    public class SlimeF
    {
        public static Vector3 CalcRigTransLocalScale(int size, out float newSizeOffset)
        {
            newSizeOffset = (size * 0.1f);
            Vector3 newSizeOffsetVector = Vector3.one * newSizeOffset;
            return Vector3.one + newSizeOffsetVector;
        }
    }
}