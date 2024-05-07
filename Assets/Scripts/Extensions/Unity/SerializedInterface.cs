using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extensions.Unity
{
    [Serializable]
    public class SerializedInterface<T> : ISerializationCallbackReceiver where T : class 
    {
        [OnValueChanged("AssignObject")][ShowInInspector]private T InterfaceRef;

        [HideInInspector][SerializeField] private Object AssignedObject;

        private void AssignObject()
        {
            AssignedObject = InterfaceRef as Object;
        }
        
        public T Value()
        {
            return AssignedObject as T;
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            InterfaceRef = AssignedObject as T;
        }
    }
}