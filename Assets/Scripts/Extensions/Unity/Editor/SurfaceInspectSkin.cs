using System.Collections.Generic;
using System.Linq;
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
    [EditorTool("SurfaceInspector", typeof(SkinnedMeshRenderer))]
    public class SurfaceInspectSkin : EditorTool
    {
        public static event UnityAction<GameObject, SkinnedMeshRenderer> OnSurfaceDataCaptured;

        private const float NormalDrawLengthDiv = 25f;
        private const float DiscDrawRadiusDiv = 10f;
        private SkinnedMeshRenderer _mySkinnedMeshRenderer;
        private Event _currentEvent;
        private MeshCollider _myTempMeshCollider;
        private readonly RaycastHit[] _sceneViewHits = new RaycastHit[5];
        private int _sceneViewHitsCount;
        private bool _didHaveMeshCollider;
        private PhysicsScene _myPhysicsScene;
        private GameObject _myGo;
        private Scene _myGoScene;
        private List<Transform> _myMeshBones;
        private SurfaceDataCollectorSkin _targetSurfaceDataCollector;

        private GameObject _createdSurfacePlot;

        public override void OnActivated()
        {
            _mySkinnedMeshRenderer = (SkinnedMeshRenderer)target;
            _myMeshBones = _mySkinnedMeshRenderer.bones.ToList();

            Mesh sharedMesh = _mySkinnedMeshRenderer.sharedMesh;

            Physics.BakeMesh(sharedMesh.GetInstanceID(), false);

            _myGo = _mySkinnedMeshRenderer.gameObject;
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

            if (_myGo.TryGetComponent(out SurfaceDataCollectorSkin collector))
            {
                _targetSurfaceDataCollector = collector;
            }
            else
            {
                _targetSurfaceDataCollector = ObjectFactory.AddComponent<SurfaceDataCollectorSkin>(_myGo);
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
                        EditorGUIUtility.AddCursorRect(new Rect(0f, 0f ,0f ,0f),MouseCursor.Arrow);
                        Handles.lighting = true;

                        float handleSize = HandleUtility.GetHandleSize(sceneViewHit.point);

                        Handles.ArrowHandleCap(-1, sceneViewHit.point, Quaternion.LookRotation(sceneViewHit.normal),handleSize, EventType.Repaint);
                        
                        Handles.DrawLine
                        (
                            sceneViewHit.point,
                            sceneViewHit.point + sceneViewHit.normal * handleSize
                        );
                        
                        Handles.DrawWireDisc
                        (
                            sceneViewHit.point,
                            sceneViewHit.normal,
                            handleSize / DiscDrawRadiusDiv
                        );

                        
                        // Handles.DrawSolidDisc
                        // (
                        //     sceneViewHit.point + sceneViewHit.normal * handleSize,
                        //     sceneViewHit.normal,
                        //     handleSize / DiscDrawRadiusDiv
                        // );
                    }
                }
            }
        }

        private void OnKeyDown() {}

        private void OnMouseMove()
        {
            Vector2 offSetMousePos = _currentEvent.mousePosition;

            offSetMousePos.y -= 20f;
            
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(offSetMousePos);

            _sceneViewHitsCount = _myPhysicsScene.Raycast
            (mouseRay.origin, mouseRay.direction, _sceneViewHits);
        }

        private void OnMouseUpLeft() {}

        private void OnMouseDownLeft()
        {
            Vector2 offSetMousePos = _currentEvent.mousePosition;

            offSetMousePos.y -= 20f;

            Ray mouseRay = HandleUtility.GUIPointToWorldRay(offSetMousePos);

            RaycastHit availHit = new RaycastHit();
            
            _sceneViewHitsCount = _myPhysicsScene.Raycast
            (mouseRay.origin, mouseRay.direction, _sceneViewHits);
            
            foreach (RaycastHit sceneViewHit in _sceneViewHits)
            {
                if (sceneViewHit.collider == _myTempMeshCollider)
                {
                    availHit = sceneViewHit;
                    break;
                }
            }

            if (_sceneViewHitsCount == 0) return;
            
            Vector3[] sharedMeshVertices = _mySkinnedMeshRenderer.sharedMesh.vertices;
            
            float minDist = Vector3.Distance(sharedMeshVertices[0], availHit.point);
            int closestVertexIndex = 0;

            if (_mySkinnedMeshRenderer.bones.Length > 0)
            {
                for (int i = 0; i < sharedMeshVertices.Length; i++)
                {
                    Vector3 variable = sharedMeshVertices[i];
                    float distance = Vector3.Distance(variable, availHit.point);

                    if (distance < minDist)
                    {
                        minDist = distance;
                        closestVertexIndex = i;
                    }
                }

                Debug.LogWarning(closestVertexIndex);
                
                int closestVertBoneIndex = _mySkinnedMeshRenderer.sharedMesh
                .boneWeights[closestVertexIndex]
                .boneIndex0;
                            
                CreateSurfacePlot
                (availHit, _mySkinnedMeshRenderer.bones[closestVertBoneIndex]);
            }
            else
            {
                foreach (RaycastHit sceneViewHit in _sceneViewHits)
                {
                    if (sceneViewHit.collider == _myTempMeshCollider)
                    {
                        CreateSurfacePlot(sceneViewHit, null);
                    }
                }
            }
        }

        private void CreateSurfacePlot(RaycastHit sceneViewHit, Transform vertBone)
        {
            _createdSurfacePlot = ObjectFactory.CreateGameObject(_myGoScene, HideFlags.None, "SurfaceData");

            SurfacePlotSkinned newPlot = ObjectFactory.AddComponent<SurfacePlotSkinned>
            (_createdSurfacePlot);

            Transform plotTrans = _createdSurfacePlot.transform;

            if (vertBone)
            {
                plotTrans.SetParent(vertBone);
                newPlot.Construct(plotTrans, vertBone);
            }
            else
            {
                Debug.LogWarning
                (
                    "Surface data of SkinnedMeshRenderer has been plotted in the root object transform because renderer does not have any bones!"
                );
                newPlot.Construct(plotTrans, null);
            }

            plotTrans.position = sceneViewHit.point;
            plotTrans.forward = sceneViewHit.normal;
            
            Undo.RecordObject(_targetSurfaceDataCollector, "AddPlot");
            ((ISurfaceTool)_targetSurfaceDataCollector).AddPlot(newPlot);
            OnSurfaceDataCaptured?.Invoke(_createdSurfacePlot, _mySkinnedMeshRenderer);
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