using System.Collections;
using Pathfinding;
using UnityEngine;

namespace Slimes.Pathfinding
{
    public class PlayerPathSetter : MonoBehaviour
    {
        [SerializeField] private AIPath _aiPath;
        
        private Camera _mainCam;
        private Vector3 _moveDelta;

        private void Awake()
        {
            _mainCam = Camera.main;
    }

        private void Start()
        {
            StartCoroutine(InputListenerRoutine());
        }

        private IEnumerator InputListenerRoutine()
        {
            while(true)
            {
                if(Input.GetMouseButton(0))
                { 
                    Ray inputRay = _mainCam.ScreenPointToRay(Input.mousePosition);
                
                    //Debug.DrawRay(_mainCamTrans.position, inputRay.direction * 100f, Color.red, Time.deltaTime);

                    if(Physics.Raycast(inputRay, out RaycastHit inputRayCastHit, 100f))
                    {
                        //Debug.LogWarning(inputRayCastHit.transform.gameObject);

                        if(inputRayCastHit.transform.CompareTag("Ground"))
                        {
                            Vector3 goToPos = inputRayCastHit.point;

                            _aiPath.destination = goToPos;
                            _aiPath.SearchPath();
                        }
                    }
                }
            
                yield return null;
            }
        }
    }
}