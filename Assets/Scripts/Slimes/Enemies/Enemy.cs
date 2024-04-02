using System.Collections;
using Pathfinding;
using UnityEngine;
using WorldObjects;

namespace Slimes.Enemies
{
    public class Enemy : Slime
    {
        [SerializeField] private AIPath _aiPath;
        [SerializeField] private SphereCollider _sphereDetector;
        
        private Coroutine _baitChaseRoutine;
        private Bait _currBait;
        private WaitForFixedUpdate _waitForFixedUpdate = new();
        private Coroutine _seekRoutine;
        private Vector3 _currSeekPos;
        private bool _isSeeking;
        
        private void Start()
        {
            StartSeekRoutine();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Bait"))
            {
                _currBait = other.GetComponent<Bait>();
                StopSeekRoutine();
                StartBaitChaseRoutine();
            }
        }

        private void StartBaitChaseRoutine()
        {
            if(_baitChaseRoutine != null)
            {
                StopCoroutine(_baitChaseRoutine);
            }
            _baitChaseRoutine = StartCoroutine(BaitChaseRoutine());
        }

        private IEnumerator BaitChaseRoutine()
        {
            while(true)
            {
                if(_currBait == null)
                {
                    StopBaitChaseRoutine();
                    StartSeekRoutine();
                    yield break;
                }
                
                _aiPath.destination = _currBait.InitPos;
                _aiPath.SearchPath();
                
                yield return _waitForFixedUpdate;
            }
        }

        private void StopBaitChaseRoutine()
        {
            if(_baitChaseRoutine != null)
            {
                StopCoroutine(_baitChaseRoutine);
                _baitChaseRoutine = null;
            }
        }

        private void StartSeekRoutine()
        {
            _isSeeking = false;
            if(_seekRoutine != null)
            {
                StopCoroutine(_seekRoutine);
            }
            _seekRoutine = StartCoroutine(SeekRoutine());
        }

        private IEnumerator SeekRoutine()
        {
            while(true)
            {
                if(_isSeeking == false)
                {
                    SetRandSeekPos();
                }


                if(Vector3.Distance(transform.position, _currSeekPos) < 1.2f)
                {
                    SetRandSeekPos();
                }

                yield return null;
            }
        }

        private void SetRandSeekPos()
        {
            _isSeeking = true;
            _currSeekPos =  GetRandSeekPos();

            _aiPath.destination = _currSeekPos;
            _aiPath.SearchPath();
        }

        private Vector3 GetRandSeekPos()
        {
             Vector3 returnPos = transform.position;
             float sphereDetectorRadius = _sphereDetector.radius;
             float sphereDetectorRadiusNegative = -1f*sphereDetectorRadius;
             
             //TODO: limit return position inside of terrain limits
             
             returnPos.x -= Random.Range(sphereDetectorRadiusNegative, sphereDetectorRadius);
             returnPos.z -= Random.Range(sphereDetectorRadiusNegative, sphereDetectorRadius);

             return returnPos;
        }

        private void StopSeekRoutine()
        {
            if(_seekRoutine != null)
            {
                StopCoroutine(_seekRoutine);
                _seekRoutine = null;
            }

            _isSeeking = false;
        }
    }
}