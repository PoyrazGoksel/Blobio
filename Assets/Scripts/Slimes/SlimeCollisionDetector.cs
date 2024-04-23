using Events;
using Events.Internal;
using UnityEngine;
using Utils;
using WorldObjects;

namespace Slimes
{
    public class SlimeCollisionDetector : EventListenerMono
    {
        [SerializeField] private SlimeEvents _slimeEvents;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Bait colBait))
            {
                _slimeEvents.BaitCollision?.Invoke(colBait);
                colBait.OnEaten();
            }
        }

        private void OnSizeIncreased(int playerSize)
        {
            transform.localScale = SlimeF.CalcRigTransLocalScale(playerSize);
        }

        protected override void RegisterEvents() {PlayerEvents.SizeIncreased += OnSizeIncreased;}

        protected override void UnRegisterEvents() {PlayerEvents.SizeIncreased -= OnSizeIncreased;}
    }
}