using System;
using System.Collections;
using Installers;
using Pathfinding;
using UnityEngine;

namespace Slimes.Pathfinding
{
    public class PlayerPathSetter : MonoBehaviour
    {
        [SerializeField] private AIPath _aiPath;
        private Vector3 _moveDelta;
        private Settings _mySettings;

        private void Awake()
        {
            _mySettings = ProjectInstaller.Instance.GameSettings.PlayerPathSetterSettings;
        }

        private void Start() {StartCoroutine(InputListenerRoutine());}

        private IEnumerator InputListenerRoutine()
        {
            while(true)
            {
                _aiPath.maxSpeed = _mySettings.PlayerSpeed;
                
                if(Input.GetMouseButton(0))
                {
                    Ray inputRay = ProjectInstaller.Instance.MainCam.ScreenPointToRay(Input.mousePosition);

                    RaycastHit[] hits = Physics.RaycastAll(inputRay, 100f);

                    foreach(RaycastHit hit in hits)
                    {
                        if(hit.transform.CompareTag("Ground"))
                        {
                            Vector3 goToPos = hit.point;

                            _aiPath.destination = goToPos;
                            _aiPath.SearchPath();
                        }
                    }
                }

                yield return null;
            }
        }

        [Serializable]
        public class Settings
        {
            public float PlayerSpeed => _playerSpeed;
            [SerializeField] private float _playerSpeed;
        }

        public void Pause()
        {
            _aiPath.isStopped = true;
        }  
        public void UnPause()
        {
            _aiPath.isStopped = false;
        }
    }
}