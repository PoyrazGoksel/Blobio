using UnityEngine;

namespace Utils
{
    public class SlimeF
    {
        public static Vector3 CalcRigTransLocalScale(int size) => Vector3.one + (Vector3.one * (size * 0.1f));
    }
}