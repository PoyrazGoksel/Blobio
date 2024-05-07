using System;
using UnityEngine;

namespace Extensions.Unity.SurfaceInspectTool
{
    public abstract class SurfacePlot : MonoBehaviour
    {
        public Transform Transform => _myTrans;
        public Vector3 Position => _myTrans.position;
        public Vector3 Normal => _myTrans.forward;
        
        [HideInInspector][SerializeField] protected Transform _myTrans;
    }
}