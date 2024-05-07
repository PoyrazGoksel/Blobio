using UnityEngine;

namespace Extensions.Unity
{
    public class TransformEncapsulated
    {
        public Vector3 position => _transform.position;
        public Quaternion rotation => _transform.rotation;
        public Vector3 eulerAngles => _transform.eulerAngles;
        public Vector3 localPosition => _transform.localPosition;
        public Quaternion localRotation => _transform.localRotation;
        public Vector3 localEulerAngles => _transform.localEulerAngles;
        public Vector3 forward => _transform.forward;
        private readonly Transform _transform;
        
        public TransformEncapsulated(Transform transform)
        {
            _transform = transform;
        }

        public void Update(Transform transform)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}