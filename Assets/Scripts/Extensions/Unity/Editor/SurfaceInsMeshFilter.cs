using Extensions.Unity.SurfaceInspectTool;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Extensions.Unity.Editor
{
    /// <summary>
    /// Update errors on Unity Ver. 2021 
    /// </summary>
    [EditorTool("SurfaceInspector", typeof(MeshFilter))]
    public class SurfaceInsMeshFilter : EditorTool
    {
        public static event UnityAction<GameObject, MeshFilter> OnSurfaceDataCaptured;

        private const float NormalDrawLengthDiv = 25f;
        private const float DiscDrawRadiusDiv = 10f;
        private MeshFilter _myMeshFilter;
        private Event _currentEvent;
        private MeshCollider _myTempMeshCollider;
        private readonly RaycastHit[] _sceneViewHits = new RaycastHit[5];
        private int _sceneViewHitsCount;
        private bool _didHaveMeshCollider;
        private PhysicsScene _myPhysicsScene;
        private GameObject _myGo;
        private Scene _myGoScene;
        private SurfaceDataCollectorMesh _targetSurfaceDataCollector;

        private GameObject _createdSurfacePlot;

        public override void OnActivated()
        {
            _myMeshFilter = (MeshFilter)target;

            Mesh sharedMesh = _myMeshFilter.sharedMesh;

            Physics.BakeMesh(sharedMesh.GetInstanceID(), false);

            _myGo = _myMeshFilter.gameObject;
            _myGoScene = _myGo.scene;
            _myPhysicsScene = _myGoScene.GetPhysicsScene();

            if (_myGo.TryGetComponent(out MeshCollider meshCollider))
            {
                _didHaveMeshCollider = true;
                _myTempMeshCollider = meshCollider;
            }
            else
            {
                _didHaveMeshCollider = false;
                _myTempMeshCollider = _myGo.AddComponent<MeshCollider>();
                _myTempMeshCollider.sharedMesh = sharedMesh;
            }

            if (_myGo.TryGetComponent(out SurfaceDataCollectorMesh collector))
            {
                _targetSurfaceDataCollector = collector;
            }
            else
            {
                _targetSurfaceDataCollector = ObjectFactory.AddComponent<SurfaceDataCollectorMesh>(_myGo);
            }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            _currentEvent = Event.current;

            if (_currentEvent.isMouse) {}

            switch (_currentEvent.type)
            {
                case EventType.MouseDown:
                    if (_currentEvent.button == 0)
                    {
                        OnMouseDownLeft();
                        _currentEvent.Use();
                    }
                    else if (_currentEvent.button == 1)
                    {
                        OnMouseDownRight();
                    }

                    break;
                case EventType.MouseUp:
                    if (_currentEvent.button == 0)
                    {
                        OnMouseUpLeft();
                        _currentEvent.Use();
                    }
                    else if (_currentEvent.button == 1)
                    {
                        OnMouseUpRight();
                    }

                    break;
                case EventType.MouseMove:
                    OnMouseMove();
                    break;
                case EventType.KeyDown:
                    OnKeyDown();
                    break;
                case EventType.Repaint:
                    OnRepaint();
                    break;
            }
        }

        private void OnMouseDownRight() {}

        private void OnMouseUpRight() {}

        private void OnRepaint()
        {
            if (_currentEvent.type == EventType.Repaint)
            {
                if (_sceneViewHitsCount == 0) return;

                foreach (RaycastHit sceneViewHit in _sceneViewHits)
                {
                    if (sceneViewHit.collider == _myTempMeshCollider)
                    {
                        Handles.DrawLine
                        (
                            sceneViewHit.point,
                            sceneViewHit.point + sceneViewHit.normal / NormalDrawLengthDiv
                        );

                        Handles.DrawWireDisc
                        (
                            sceneViewHit.point,
                            sceneViewHit.normal,
                            HandleUtility.GetHandleSize(sceneViewHit.point) / DiscDrawRadiusDiv
                        );
                    }
                }
            }
        }

        private void OnKeyDown() {}

        private void OnMouseMove()
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(_currentEvent.mousePosition);

            _sceneViewHitsCount = _myPhysicsScene.Raycast
            (mouseRay.origin, mouseRay.direction, _sceneViewHits);
        }

        private void OnMouseUpLeft() {}

        private void OnMouseDownLeft()
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(_currentEvent.mousePosition);

            _sceneViewHitsCount = _myPhysicsScene.Raycast
            (mouseRay.origin, mouseRay.direction, _sceneViewHits);

            if (_sceneViewHitsCount == 0) return;

            foreach (RaycastHit sceneViewHit in _sceneViewHits)
            {
                if (sceneViewHit.collider == _myTempMeshCollider)
                {
                    CreateSurfacePlot(sceneViewHit);
                }
            }
        }

        private void CreateSurfacePlot(RaycastHit sceneViewHit)
        {
            _createdSurfacePlot = ObjectFactory.CreateGameObject(_myGoScene, HideFlags.None, "SurfaceData");

            SurfacePlotMesh newPlot = ObjectFactory.AddComponent<SurfacePlotMesh>
            (_createdSurfacePlot);
            Transform plotTrans = _createdSurfacePlot.transform;

            newPlot.Construct(plotTrans);
            
            plotTrans.position = sceneViewHit.point;
            plotTrans.forward = sceneViewHit.normal;
            
            ((ISurfaceTool)_targetSurfaceDataCollector).AddPlot(newPlot);
            OnSurfaceDataCaptured?.Invoke(_createdSurfacePlot, _myMeshFilter);
        }

        public override void OnWillBeDeactivated()
        {
            if (_createdSurfacePlot)
            {
                Selection.activeObject = _createdSurfacePlot;
            }

            if (_didHaveMeshCollider == false)
            {
                _myTempMeshCollider.DestroyNow(true);
            }
        }
    }

}