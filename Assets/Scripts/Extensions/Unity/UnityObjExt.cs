using UnityEngine;

namespace Extensions.Unity
{
    public static class UnityObjExt
    {
        public static void Destroy(this Object o)
        {
            Object.Destroy(o);
        }
        
        public static void DestroyNow(this Object o, bool allowDestroyingAssets = false)
        {
            Object.DestroyImmediate(o, allowDestroyingAssets);
        }
    }
}