using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions.Unity
{
    [Serializable]
    public abstract class UnityDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] public List<TKey> _tKeys = new();
        [SerializeField] public List<TValue> _tValues = new();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _tKeys = Keys.ToList();
            _tValues = Values.ToList();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();

            if (_tKeys != null)
            {
                for (int i = 0; i < _tKeys.Count; i++)
                {
                    Add(_tKeys[i], _tValues[i]);
                }
            }
        }
    }
}