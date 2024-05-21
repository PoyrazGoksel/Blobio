using UnityEngine;

namespace Extensions.Unity
{
    public static class MathfExt
    {
        private const float OneDec = 10f;
        private const float MaxEulAngle = 360f;

        public static float RoundToOneDec(float number)
        {
            return Mathf.Round(number * OneDec) / OneDec;
        }

        public static float ToEul(this float thisFloat)
        {
            thisFloat %= MaxEulAngle;

            return thisFloat;
        }
        
        public static float ToShortestAngle(this float thisFloat)
        {
            thisFloat %= 360f;

            if (thisFloat > 180f)
            {
                thisFloat -= 360f;
            }
            else if (thisFloat < -180f)
            {
                thisFloat += 360f;
            }

            return thisFloat;
        }
        
        public static bool IsEulBtwn(this float thisEul, float min, float max)
        {
            thisEul = thisEul.ToEul();
            max = max.ToEul();
            min = min.ToEul();
            
            min -= thisEul;
            max -= thisEul;
            
            thisEul = 0;

            return IsBtwn(thisEul, min, max);
        }

        public static bool IsEulOnLeft(this float thisEul, float isOnLeft)
        {
            thisEul = thisEul.ToEul();
            isOnLeft = isOnLeft.ToEul();

            return isOnLeft - thisEul < 0f;
        }
        
        public static bool IsEulOnRight(this float thisEul, float isOnLeft)
        {
            thisEul = thisEul.ToEul();
            isOnLeft = isOnLeft.ToEul();
            
            return isOnLeft - thisEul > 0f;
        }
        
        public static bool IsBtwn(this float thisFloat, float min, float max)
        {
            return thisFloat >= min && thisFloat <= max;
        }
    }
}