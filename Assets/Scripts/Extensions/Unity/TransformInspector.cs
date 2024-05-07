#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace Extensions.Unity
{
#if UNITY_EDITOR
    public static class TransformInspector
    {
        public static WorldPlacement LastCopiedWorldPlacement;

        [MenuItem("CONTEXT/Transform/Extract World Data")]
        private static void ExtractWorldData(MenuCommand command)
        {
            Transform transform = (Transform) command.context;
            
            LastCopiedWorldPlacement = new WorldPlacement(transform);
        }
    }
#endif
}