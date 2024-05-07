#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace Extensions.Unity
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(WorldPlacement))]
    public class WorldPlacementDrawer: PropertyDrawer
    {
        private const float ExpandedHeight = 60f;
        private const string CopyText = "Copy";
        private const string PasteText = "Paste";
        private const string ResetText = "Reset";
        private const string PosPropName = "Position";
        private const string RotPropName = "Rotation";

        private SerializedProperty _positionProp;
        private SerializedProperty _rotationProp;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _positionProp = property.FindPropertyRelative(PosPropName);
            _rotationProp = property.FindPropertyRelative(RotPropName);
            
            Event e = Event.current;
            
            if (e.type == EventType.MouseDown &&
                e.button == 1 &&
                position.Contains(e.mousePosition))
            {
                GenericMenu context = new GenericMenu();
                context.AddItem
                (
                    new GUIContent(CopyText),
                    false,
                    () =>
                    {
                        TransformInspector.LastCopiedWorldPlacement = new WorldPlacement
                        (
                            _positionProp.vector3Value,
                            _rotationProp.vector3Value
                        );
                    }
                );

                context.AddItem
                (
                    new GUIContent(PasteText),
                    false,
                    () =>
                    {
                        if (TransformInspector.LastCopiedWorldPlacement == null)
                        {
                            return;
                        }

                        _positionProp.vector3Value =
                        TransformInspector.LastCopiedWorldPlacement.Position;
                        _rotationProp.vector3Value =
                        TransformInspector.LastCopiedWorldPlacement.Rotation;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                );
                
                context.AddItem
                (
                    new GUIContent(ResetText),
                    false,
                    () =>
                    {
                        _positionProp.vector3Value = Vector3.zero;
                        _rotationProp.vector3Value = Vector3.zero;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                );
              
                context.ShowAsContext();
                e.Use();
            }
            
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ExpandedHeight;
        }
    }
#endif
}