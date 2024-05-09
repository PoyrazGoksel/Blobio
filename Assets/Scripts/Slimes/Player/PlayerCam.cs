using System;
using Events;
using Installers;
using UnityEngine;

namespace Slimes.Player
{
    public class PlayerCam : MonoBehaviour
    {
        [SerializeField] private Transform _slimeTrans;
        [SerializeField] private Transform _myTrans;
        private Settings _mySettings;
        private float _currSizeOffSet;
        private void Awake()
        {
            _mySettings = ProjectInstaller.Instance.GameSettings.PlayerCamSettings;
        }

        private void OnEnable()
        {
            PlayerEvents.SizeIncreased += OnSizeIncreased;
        }

        private void OnDisable()
        {
            PlayerEvents.SizeIncreased -= OnSizeIncreased;
        }

        private void OnSizeIncreased(int playerSize)
        {
            _currSizeOffSet += 0.1f * playerSize;
        }

        private void Update()
        {
            if(_slimeTrans == false) return;
            
            Vector3 offSetVect = Vector3.back * (_currSizeOffSet + _mySettings.OffSet);

            Quaternion angleAxis = Quaternion.AngleAxis(_mySettings.PanAngle, Vector3.right);

            Vector3 rotatedVect = angleAxis * offSetVect;

            _myTrans.position = _slimeTrans.position + rotatedVect;
            _myTrans.LookAt(_slimeTrans);
        }
        
        [Serializable]
        public class Settings
        {
            [SerializeField] private float _offSet = 10f;
            [SerializeField] private float _panAngle = 60f;
            public float OffSet => _offSet;
            public float PanAngle => _panAngle;
        }
    }
}