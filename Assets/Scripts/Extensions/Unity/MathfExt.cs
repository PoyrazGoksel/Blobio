using UnityEngine;

namespace Extensions.Unity
{
    public static class MathfExt
    {
        public static float RoundToOneDec(float number)
        {
            return Mathf.Round(number * 10f) / 10f;
        }
    }
}