using Events.Internal;
using Extensions.Unity.MonoHelper;
using UnityEngine;
using WorldObjects;

namespace Slimes
{
    public class SlimeEatDetector : EventListenerMono
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
            if(other.TryGetComponent(out Bait currBait))
            {
                _slimeEvents.BaitCollision?.Invoke(currBait);
                currBait.OnEaten();
            }
            
            if(other.TryGetComponent(out Slime otherSlime))
            {
                _slimeEvents.SlimeCollision?.Invoke(otherSlime);
            }
        }

        protected override void RegisterEvents()
        {
            _slimeEvents.SizeIncrease += OnSizeIncrease;
        }

        private void OnSizeIncrease(float newSizeOff)
        {
            _sphereCollider.radius += _initDetectorSize * newSizeOff;
        }

        protected override void UnRegisterEvents()
        {
            _slimeEvents.SizeIncrease -= OnSizeIncrease;
        }
    }
}