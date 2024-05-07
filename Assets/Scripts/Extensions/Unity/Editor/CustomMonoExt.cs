using System.Reflection;
using Extensions.Unity.MonoHelper;
using UnityEngine;

namespace Extensions.Unity.Editor
{
    [UnityEditor.CustomEditor(typeof(MonoBehaviour), true)]
    public class CustomMonoExt : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MonoBehaviour myClass = (MonoBehaviour)target;
            MethodInfo[] methods = myClass.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (MethodInfo method in methods)
            {
                BtnAttribute btnAttribute = global::System.Attribute.GetCustomAttribute(method, typeof(BtnAttribute)) as BtnAttribute;

                if (btnAttribute != null)
                {
                    if (GUILayout.Button(method.Name))
                    {
                        method.Invoke(myClass, null);
                    }
                }
            }
        }
    }
}