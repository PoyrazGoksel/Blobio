using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions.Unity
{
    [Serializable]
    public class UnityDicList<T> : List<T>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<T> _myList = new List<T>();

        public void OnBeforeSerialize()
        {
            _myList = new List<T>(this);
        }

        public void OnAfterDeserialize()
        {
            Clear();

            foreach (T item in _myList)
            {
                Add(item);
            }
        }
    }
}