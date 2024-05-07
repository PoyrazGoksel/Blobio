using System;
using UnityEngine;

namespace Extensions.Unity
{
    [Serializable]
    public struct ColorRGBUshortDat
    {
        public const ushort FloatToUshort = 10000;

        public ushort R;
        public ushort G;
        public ushort B;

        public ColorRGBUshortDat(Color colorRGB)
        {
            R = colorRGB[0] < 0 ?  default : (ushort) (colorRGB.r * FloatToUshort);
            G = colorRGB[1] < 0 ?  default : (ushort) (colorRGB.g * FloatToUshort);
            B = colorRGB[2] < 0 ?  default : (ushort) (colorRGB.b * FloatToUshort);
        }

        public Color GetColor()
        {
            return new Color((float)R / FloatToUshort, (float)G / FloatToUshort, (float)B / FloatToUshort, 1f);
        }
    }
}