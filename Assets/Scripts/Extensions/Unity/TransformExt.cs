using UnityEngine;

namespace Extensions.Unity
{
    public static class TransformExt
    {
        private const float ReverseVectorMulti = -1f;

        public static Vector3 EulWithSign(this Transform trans)
        {
            Vector3 eul = trans.eulerAngles;

            for (int i = 0; i < 3; i ++)
            {
                float thisAxis = eul[i];

                if (thisAxis > 180f)
                {
                    eul[i] = thisAxis - 360f;
                }
            }

            return eul;
        }

        public static Transform X(this Transform trans, float x)
        {
            Vector3 pos = trans.position;
            pos.x = x;
            trans.position = pos;
            return trans;
        }
        
        public static Transform Y(this Transform trans, float y)
        {
            Vector3 pos = trans.position;
            pos.y = y;
            trans.position = pos;
            return trans;
        }
        
        public static Transform Z(this Transform trans, float z)
        {
            Vector3 pos = trans.position;
            pos.z = z;
            trans.position = pos;
            return trans;
        }
        
        public static Transform EulX(this Transform trans, float x)
        {
            Vector3 pos = trans.eulerAngles;
            pos.x = x;
            trans.eulerAngles = pos;
            return trans;
        }
        
        public static Transform EulY(this Transform trans, float y)
        {
            Vector3 pos = trans.eulerAngles;
            pos.y = y;
            trans.eulerAngles = pos;
            return trans;
        }
        
        public static Transform EulZ(this Transform trans, float z)
        {
            Vector3 pos = trans.eulerAngles;
            pos.z = z;
            trans.eulerAngles = pos;
            return trans;
        }
        
        public static bool TryGetComponentInChildren<T>(this Transform myTrans, out T component)
        {
            for (int i = 0; i < myTrans.childCount; i++)
            {
                if (myTrans.GetChild(i).TryGetComponent(out T component1))
                {
                    component = component1;
                    return true;
                }
            }

            component = default;
            return false;
        }
        
        public static void Copy(this Transform transform, Transform transToCopy)
        {
            transform.SetParent(transToCopy.parent);
            transform.position = transToCopy.position;
            transform.rotation = transToCopy.rotation;
        }
        
        public static void CopyWorld(this Transform transform, Transform transToCopy)
        {
            transform.position = transToCopy.position;
            transform.rotation = transToCopy.rotation;
        }
        
        public static Vector3 Backward(this Transform trans)
        {
            return trans.forward * ReverseVectorMulti;
        }

        public static void MoveBackDir(this Transform trans, Transform from, float amount)
        {
            trans.position += from.Backward() * amount;
        }
        
        public static void MoveFrontDir(this Transform trans, Transform from, float amount)
        {
            trans.position += from.forward * amount;
        }
        
        public static void MoveBackDirLoc(this Transform trans, Transform from, float amount)
        {
            trans.localPosition += from.Backward() * amount;
        }
        
        public static void MoveFrontDirLoc(this Transform trans, Transform from, float amount)
        {
            trans.localPosition += from.forward * amount;
        }
        
        public static void LocalForward(this Transform transform, Vector3 direction)
        {
            // Normalize the direction to make sure it's a unit vector
            direction.Normalize();

            // Calculate the rotation that aligns the local forward with the target direction
            Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, direction) * transform.rotation;

            // Set the local rotation of the transform
            transform.localRotation = targetRotation;
        }
        
        public static void LocalRight(this Transform transform, Vector3 direction)
        {
            // Normalize the direction to make sure it's a unit vector
            direction.Normalize();

            // Calculate the rotation that aligns the local forward with the target direction
            Quaternion targetRotation = Quaternion.FromToRotation(transform.right, direction) * transform.rotation;

            // Set the local rotation of the transform
            transform.localRotation = targetRotation;
        }
        
        
        public static Vector3 LocalRight(this Transform targetTransform)
        {
            return targetTransform.localToWorldMatrix.MultiplyVector(Vector3.right).normalized;
        }
        
        public static void LocalForwardRight(this Transform transform, Vector3 forward, Vector3 right)
        {
            // Normalize the input vectors to make sure they are unit vectors
            forward.Normalize();
            right.Normalize();

            // Ensure the forward and right vectors are orthogonal by recalculating the right vector
            Vector3 correctedRight = Vector3.Cross(Vector3.Cross(forward, right), forward).normalized;

            // Calculate the rotation that aligns the local forward and right with the target directions
            Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.Cross(correctedRight, forward));

            // Set the local rotation of the transform
            transform.localRotation = targetRotation;
        }
        
        public static void GoPosAndMoveFront(this Transform trans, Transform from, float amount)
        {
            trans.position = from.position + from.forward * amount;
        }
        
        public static void MoveFrontAndFace(this Transform trans, Transform from, float amount)
        {
            Vector3 fromPosition = from.position;
            trans.position = fromPosition + from.forward * amount;
            trans.forward = from.Backward();
        }

        public static WorldPlacement GetWorldData(this Transform transform)
        {
            return new WorldPlacement(transform);
        }
        
        public static void RestoreWorldData(this Transform transform, WorldPlacement worldPlacement)
        {
            worldPlacement.PasteDataTo(transform);
        }
        
        public static Vector3 Revert(this Vector3 v)
        {
            return ReverseVectorMulti * v;
        }
    }
}