using UnityEngine;

namespace Extensions.Unity
{
    public static class ColorExt
    {
        public static float ChannelTotal(this Color color)
        {
            return color.r + color.g + color.b + color.a;
        }
        
        public static float RGBTot(this Color color)
        {
            return color.r + color.g + color.b;
        }
        
        public static Vector3 RGB(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }
        
        public static float DepthChannelTot(this Color color)
        {
            return (color.r + color.g);
        }
        
        public static Vector2 DepthChannel(this Color color)
        {
            return new Vector2(color.r, color.g);
        }
    }
}