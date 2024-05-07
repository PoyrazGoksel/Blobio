using System.Collections.Generic;
using UnityEngine;

namespace Extensions.Unity
{
    public class SphereCalculations
    {
        public static List<Vector3> CreateSphericalGrid(int pointsOnSphere)
        {
            if (pointsOnSphere < 1) return new List<Vector3> {Vector3.zero,};
            List<Vector3> gridCoords = new List<Vector3>();

            float dlong = Mathf.PI * (3 - Mathf.Sqrt(5));
            float dz = 2.0f / pointsOnSphere;
            float longt = 0;
            float z = 1 - dz / 2f;

            for (int i = 0; i < pointsOnSphere - 1; i ++)
            {
                float r = Mathf.Sqrt(1 - z * z);

                gridCoords.Add(new Vector3(Mathf.Cos(longt) * r, Mathf.Sin(longt) * r, z));

                z -= dz;
                longt += dlong;
            }

            return gridCoords;
        }
        
        public static SphereGrid GetSphereGrid
        (int pointsOnSphere, float pointRadius)
        {
            SphereGrid newSphereGrid = new SphereGrid(pointsOnSphere, pointRadius);

            float radiusBySphereCount = Mathf.Sqrt(pointsOnSphere / (4 * Mathf.PI));

            float realDiameter = pointRadius * radiusBySphereCount * 2f;

            float dlong = Mathf.PI * (3 - Mathf.Sqrt(5));
            float dz = 2.0f / pointsOnSphere;
            float longt = 0;
            float z = 1 - dz / 2f;

            for (int i = 0; i < pointsOnSphere; i ++)
            {
                float r = Mathf.Sqrt(1 - z * z);

                newSphereGrid.Add
                (new Vector3(Mathf.Cos(longt) * r, Mathf.Sin(longt) * r, z) * realDiameter);

                z -= dz;
                longt += dlong;
            }
            
            return newSphereGrid;
        }

        public struct SphereGrid
        {
            private const float AxisZero = 0f;

            public readonly List<Vector3> GridCoords;

            public readonly int PointCount;
            public readonly float PointR;
            public readonly float Diameter;
            
            public float Ymin;
            public float Ymax;

            public float Xmin;
            public float Xmax;

            public float Zmin;
            public float Zmax;

            public SphereGrid(int pointCount, float pointR)
            {
                PointCount = pointCount;
                PointR = pointR;
                Diameter = GetSphereDiameter(pointCount, pointR);
                Ymin = AxisZero;
                Ymax = AxisZero;
                Xmin = AxisZero;
                Xmax = AxisZero;
                Zmin = AxisZero;
                Zmax = AxisZero;
                GridCoords = new List<Vector3>();
            }

            public void Add(Vector3 point)
            {
                if (GridCoords.Count >= PointCount) return;

                if (point.x < Xmin)
                {
                    Xmin = point.x;
                }
                if (point.x > Xmax)
                {
                    Xmax = point.x;
                }
                
                if (point.y < Ymin)
                {
                    Ymin = point.y;
                }
                if (point.y > Ymax)
                {
                    Ymax = point.y;
                }
                
                if (point.z < Zmin)
                {
                    Zmin = point.z;
                }
                if (point.z > Zmax)
                {
                    Zmax = point.z;
                }
                
                GridCoords.Add(point);
            }

            public float XMaxLength()
            {
                return Mathf.Abs(Xmax) + Mathf.Abs(Xmin);
            }
            
            public float YMaxLength()
            {
                return Mathf.Abs(Ymax) + Mathf.Abs(Ymin);
            }
            
            public float ZMaxLength()
            {
                return Mathf.Abs(Zmax) + Mathf.Abs(Zmin);
            }

            public static float GetSphereDiameter(int pointsOnSphere, float pointRadius)
            {
                if (pointsOnSphere == 1)
                {
                    return 2f * pointRadius;
                }
                float radiusBySphereCount = Mathf.Sqrt(pointsOnSphere / (4 * Mathf.PI));

                return (pointRadius * radiusBySphereCount * 2f) + (pointRadius * 2f);
            }
        }

    }
}