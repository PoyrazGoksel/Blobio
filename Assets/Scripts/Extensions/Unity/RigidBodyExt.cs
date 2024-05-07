using UnityEngine;

namespace Extensions.Unity
{
    public static class RigidBodyExt
    {
        public static void SetPhysicsActive(this Rigidbody rigidbody, bool isActive, bool useGravity = true)
        {
            rigidbody.isKinematic = !isActive;
            rigidbody.useGravity = isActive && useGravity;
        }

        public static RigidBodyData Freeze(this Rigidbody rigidbody)
        {
            RigidBodyData rigidBodyData = new (rigidbody);
            
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            
            return rigidBodyData;
        }

        public static void UnFreeze(this Rigidbody rigidbody, RigidBodyData rigidBodyData)
        {
            rigidBodyData.Recover(rigidbody);
        }
        
        public readonly struct RigidBodyData
        {
            public readonly bool IsKinematic;
            public readonly bool UseGravity;
            public readonly Vector3 Velocity;
            public readonly Vector3 AngularVelocity;

            public RigidBodyData(Rigidbody rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                UseGravity = rigidbody.useGravity;
                Velocity = rigidbody.velocity;
                AngularVelocity = rigidbody.angularVelocity;
            }

            public void Recover(Rigidbody rigidbody)
            {
                rigidbody.isKinematic = IsKinematic;
                rigidbody.useGravity = UseGravity;
                rigidbody.velocity = Velocity;
                rigidbody.angularVelocity = AngularVelocity;
            }
        }
        
        public static Rigidbody X(this Rigidbody trans, float x)
        {
            Vector3 pos = trans.position;
            pos.x = x;
            trans.position = pos;
            return trans;
        }
        
        public static Rigidbody Y(this Rigidbody trans, float y)
        {
            Vector3 pos = trans.position;
            pos.y = y;
            trans.position = pos;
            return trans;
        }
        
        public static Rigidbody Z(this Rigidbody trans, float z)
        {
            Vector3 pos = trans.position;
            pos.z = z;
            trans.position = pos;
            return trans;
        }

    }
}