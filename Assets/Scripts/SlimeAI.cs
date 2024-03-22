using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private Camera _mainCam;
    private Transform _mainCamTrans;
    private Vector3 _moveDelta;

    private void Awake()
    {
        _mainCam = Camera.main;
        _mainCamTrans = _mainCam.transform;
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

                        _navMeshAgent.destination = goToPos;
                    }
                }
            }
            
            yield return null;
        }
    }
}