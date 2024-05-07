using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions.Unity.SurfaceInspectTool
{
    public class SurfaceDataCollectorMesh : MonoBehaviour, ISurfaceTool
    {
        /*HideInInspector]*/[SerializeField] private List<SurfacePlotMesh> _surfacePlots = new List<SurfacePlotMesh>();

        private List<SurfacePlotMesh> _surfacePlotsCopy;

        public List<SurfacePlotMesh> GetSurfacePlots()
        {
            if (_surfacePlotsCopy == null)
            {
                _surfacePlotsCopy = new List<SurfacePlotMesh>(_surfacePlots);
            }

            return _surfacePlotsCopy;
        }

        void ISurfaceTool.AddPlot(SurfacePlot surfacePlot)
        {
            _surfacePlots.Add((SurfacePlotMesh)surfacePlot);
        }

        public void RemovePlot(SurfacePlot surfacePlot)
        {
            _surfacePlots.Remove((SurfacePlotMesh)surfacePlot);
        }
    }
}