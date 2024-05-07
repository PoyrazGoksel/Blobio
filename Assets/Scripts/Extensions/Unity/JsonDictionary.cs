using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions.Unity
{
    [Serializable]
    public class JsonDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        [SerializeField] public TKey[] _tKeys;
        [SerializeField] public TValue[] _tValues;

        public void OnBeforeSerialize()
        {
            _tKeys = Keys.ToArray();
            _tValues = Values.ToArray();
        }

        public void OnAfterDeserialize()
        {
            Clear();

            if (_tKeys != null)
            {
                for (int i = 0; i < _tKeys.Length; i++)
                {
                    Add(_tKeys[i], _tValues[i]);
                }
            }
        }
    }
}