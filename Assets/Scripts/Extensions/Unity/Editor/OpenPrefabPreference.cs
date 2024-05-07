using UnityEditor;

namespace Extensions.Unity.Editor
{
    public class OpenPrefabPreference
    {
        const string prefKey = "EnableDoubleclickOpenPrefab";
        const string prefKey1 = "EnableLeftRightClickToExitPrefab";
    
        [PreferenceItem("Scene Hierarchy Prefab Shortcut")]
        public static void PreferencesGUI()
        {
            bool isEnabledEnter = EditorPrefs.GetBool(prefKey, true);
            isEnabledEnter = EditorGUILayout.Toggle("Double Click to Open Prefab", isEnabledEnter);
            EditorPrefs.SetBool(prefKey, isEnabledEnter);
        
            bool isEnabledExit = EditorPrefs.GetBool(prefKey1, true);
            isEnabledExit = EditorGUILayout.Toggle("Right + Left Click to Exit Prefab", isEnabledExit);
            EditorPrefs.SetBool(prefKey1, isEnabledExit);
        }

        public static bool IsEnabledEnterShort()
        {
            return EditorPrefs.GetBool(prefKey, true);
        }
    
        public static bool IsEnabledExitShort()
        {
            return EditorPrefs.GetBool(prefKey1, true); 
        }
    }
}