using System.Collections.Generic;
using UnityEngine;

namespace Extensions.Unity.SurfaceInspectTool
{
    public class SurfaceDataCollectorSkin : MonoBehaviour, ISurfaceTool
    {
        /*[HideInInspector]*/[SerializeField] private List<SurfacePlotSkinned> _surfacePlots = new List<SurfacePlotSkinned>();

        private List<SurfacePlotSkinned> _surfacePlotsCopy;

        public List<SurfacePlotSkinned> GetSurfacePlots()
        {
            if (_surfacePlotsCopy == null)
            {
                _surfacePlotsCopy = new List<SurfacePlotSkinned>(_surfacePlots);
            }

            return _surfacePlotsCopy;
        }

        void ISurfaceTool.AddPlot(SurfacePlot surfacePlot)
        {
            _surfacePlots.Add((SurfacePlotSkinned)surfacePlot);
        }

        public void RemovePlot(SurfacePlot surfacePlot)
        {
            _surfacePlots.Remove((SurfacePlotSkinned)surfacePlot);
        }
    }
}