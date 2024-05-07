using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Extensions.Unity.Editor
{
    /// <summary>
    /// This class is used to open prefab by double clicking on it in the hierarchy window.
    /// </summary>
    [InitializeOnLoad]
    public class OpenPrefabDoubleClick
    {
        static bool leftMouseDown = false;
        static bool rightMouseDown = false;
        
        static OpenPrefabDoubleClick()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
            Selection.selectionChanged += OnSelectionChanged;
            SceneView.beforeSceneGui += OnSceneGUI;
        }
        
        private static void OnSelectionChanged()
        {
            leftMouseDown = false;
            rightMouseDown = false;
        }
        
        static void OnSceneGUI(SceneView sceneView)
        {
            if (!OpenPrefabPreference.IsEnabledExitShort())
                return;
            
            // Capture the Mouse Down event for left and right click
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0 && rightMouseDown)
                    leftMouseDown = true;
        
                if (Event.current.button == 1)
                    rightMouseDown = true;
            }

            // Capture the Mouse Up event for left and right click
            if (Event.current.type == EventType.MouseUp)
            {
                if (Event.current.button == 0)
                    leftMouseDown = false;

                if (Event.current.button == 1)
                    rightMouseDown = false;
            }

            // Check if both buttons were pressed at the same time
            if (leftMouseDown && rightMouseDown)
            {
                if (PrefabStageUtility.GetCurrentPrefabStage() != null)
                {
                    StageUtility.GoToMainStage();
                    Event.current.Use();
                }

                // Reset the flags
                leftMouseDown = false;
                rightMouseDown = false;
            }
        }

        
        static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            if (! OpenPrefabPreference.IsEnabledEnterShort()) return;

            if (Event.current != null && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
            {
                if (selectionRect.Contains(Event.current.mousePosition) == false)
                {
                    return;
                }
                GameObject clickedObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

                if (clickedObject)
                {
                    GameObject correspondingObjectFromSource = PrefabUtility.GetCorrespondingObjectFromSource(clickedObject);

                    if (clickedObject != null && correspondingObjectFromSource)
                    {
                        PrefabStageUtility.OpenPrefab(AssetDatabase.GetAssetPath(correspondingObjectFromSource));
                        Event.current.Use();
                    }
                }
            }            
        }
    }
}