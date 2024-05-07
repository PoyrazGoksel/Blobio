#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Extensions.Unity
{
    public static class GameObjectExt
    {
#if UNITY_EDITOR
        /// <summary>
        /// Be careful its very slow!
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static GlobalObjectId GetGlobalID(this GameObject go)
        {
            if (go.scene.path != string.Empty)
            {
                return GlobalObjectId.GetGlobalObjectIdSlow(go);
            }
            else
            {
                EDebug.LogW("This GameObject does not belong to a scene asset!");
                return default;
            }
        }
        public static bool IsInPrefabScene(this GameObject go)
        {
            return string.IsNullOrEmpty(go.scene.path);
        }
#endif
    }
}