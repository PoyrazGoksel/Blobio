using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Extensions.System
{
    [Serializable]
    public struct SerializableGuid : ISerializationCallbackReceiver
    {
        //public static SerializableGuid Empty = new SerializableGuid(Guid.Empty);

        [ReadOnly][SerializeField] private string _guid;
        [HideInInspector][SerializeField] private byte[] ByteData;
        [HideInInspector][SerializeField] private bool _isSet;

        public bool IsSet => _isSet;
        public Guid Value => _guidInstance;

        private Guid _guidInstance;

        public SerializableGuid(string guidStr)
        {
            _guidInstance = new Guid(guidStr);
            _guid = guidStr;
            ByteData = _guidInstance.ToByteArray();
            _isSet = true;
        }

        public static bool operator ==(SerializableGuid a, SerializableGuid b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SerializableGuid a, SerializableGuid b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is SerializableGuid)
            {
                if (((SerializableGuid)obj).Value == Value)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public bool Equals(SerializableGuid other)
        {
            return Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public SerializableGuid(Guid guid)
        {
            _guidInstance = guid;
            ByteData = _guidInstance.ToByteArray();
            _guid = _guidInstance.ToString();
            _isSet = true;
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            if (_isSet == false) return;

            if (ByteData.Length == 16)
            {
                _guidInstance = new Guid(ByteData);
            }
            else if (_isSet)
            {
                Debug.LogWarning(
                    "GUID Byte Array is corrupted!" +
                    $" ByteData.Length:{ByteData.Length}," +
                    $" _guid:{_guid}");
            }
        }
    }
}