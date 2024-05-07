using System;
using UnityEngine;

namespace Extensions.Unity.SurfaceInspectTool
{
    public class SurfacePlotMesh : SurfacePlot
    {
        public void Construct(Transform myTrans)
        {
            _myTrans = myTrans;
        }
    }
}