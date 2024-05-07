using System;
using UnityEngine;

namespace Extensions.Unity.SurfaceInspectTool
{
    public class SurfacePlotSkinned : SurfacePlot
    {
        public Transform AttachedBone => _myBone;
        
        [HideInInspector][SerializeField] private Transform _myBone;

        public void Construct(Transform myTrans, Transform bone)
        {
            _myTrans = myTrans;
            _myBone = bone;
        }
    }
}