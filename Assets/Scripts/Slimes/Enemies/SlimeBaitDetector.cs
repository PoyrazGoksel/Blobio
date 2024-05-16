using Events.Internal;
using UnityEngine;
using Utils;
using WorldObjects;

namespace Slimes
{
    public class SlimeBaitDetector : EventListenerMono
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
            if(other.TryGetComponent(out Bait colBait))
            {
                _slimeEvents.BaitDetection?.Invoke(colBait);
            }
        }

        protected override void RegisterEvents()
        {
            _slimeEvents.SizeIncrease += OnSizeIncrease;
        }

        private void OnSizeIncrease(float newSizeOff)
        {
            _sphereCollider.radius = _initDetectorSize + _initDetectorSize * newSizeOff;
        }

        protected override void UnRegisterEvents()
        {
            _slimeEvents.SizeIncrease -= OnSizeIncrease;
        }
    }
}