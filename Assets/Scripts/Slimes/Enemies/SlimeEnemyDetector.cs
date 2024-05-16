using Events.Internal;
using Extensions.Unity.MonoHelper;
using UnityEngine;

namespace Slimes.Enemies
{
    public class SlimeEnemyDetector : EventListenerMono
    {
        [SerializeField] private SlimeEvents _slimeEvents;
        [SerializeField] private SphereCollider _sphereCollider;
        private float _initDetectorSize;

        private void Awake()
        {
            _initDetectorSize = _sphereCollider.radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Slime slime))
            {
                _slimeEvents.EnemyDetected?.Invoke(slime);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out Slime slime))
            {
                _slimeEvents.EnemyLost?.Invoke(slime);
            }
        }

        protected override void RegisterEvents()
        {
            _slimeEvents.SizeIncrease += OnSizeIncrease;
        }

        private void OnSizeIncrease(float newSizeOff)
        {
            _sphereCollider.radius += _initDetectorSize + _initDetectorSize * newSizeOff;
        }

        protected override void UnRegisterEvents()
        {
            _slimeEvents.SizeIncrease -= OnSizeIncrease;
        }
    }
}