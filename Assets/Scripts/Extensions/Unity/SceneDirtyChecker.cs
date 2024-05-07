#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Extensions.Unity
{
    public class SceneDirtyChecker : MonoBehaviour
    {
#if UNITY_EDITOR

        static SceneDirtyChecker()
        { 
            Undo.postprocessModifications += OnPostProcessModifications; 
        }
 
        private static UndoPropertyModification[] OnPostProcessModifications(UndoPropertyModification[] propertyModifications) 
        { 
            Debug.LogWarning($"Scene was marked Dirty by number of objects = {propertyModifications.Length}"); 
            for (int i = 0; i < propertyModifications.Length; i++) 
            { 
                Debug.LogWarning($"currentValue {propertyModifications[i].currentValue.value} target = {propertyModifications[i].currentValue.target}", propertyModifications[i].currentValue.target);
            } 
            return propertyModifications; 
        }
#endif
    }
}