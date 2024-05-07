using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Extensions.Unity
{
    public static class EditorSceneManagerExt
    {
#if UNITY_EDITOR
        public static bool IsSceneInBuildSettings(string scenePath)
        {
            return EditorBuildSettings.scenes.Any(elem => elem.path == scenePath);
        }
#endif
    }
}