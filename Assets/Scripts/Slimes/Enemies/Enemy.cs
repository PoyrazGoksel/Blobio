using System.Collections;
using Pathfinding;
using UnityEngine;
using WorldObjects;

namespace Slimes.Enemies
{
    public class Enemy : Slime
    {
        [SerializeField] private AIPath _aiPath;
        private Coroutine _baitChaseRoutine;
        private Bait _currBait;
        private WaitForFixedUpdate _waitForFixedUpdate = new();

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Bait"))
            {
                _currBait = other.GetComponent<Bait>();
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

        private void StopBaitChaseRoutine()
        {
            if(_baitChaseRoutine != null)
            {
                StopCoroutine(_baitChaseRoutine);
                _baitChaseRoutine = null;
            }
        }

        private IEnumerator BaitChaseRoutine()
        {
            while(true)
            {
                if(_currBait == null)
                {
                    StopBaitChaseRoutine();
                    yield break;
                }
                
                _aiPath.destination = _currBait.InitPos;
                _aiPath.SearchPath();
                
                yield return _waitForFixedUpdate;
            }
        }
    }
}