using Events;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using WorldObjects;

namespace Slimes
{
    public class SlimeCollisionDetector : EventListenerMono
    {
        public event UnityAction<Bait> BaitCollision;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Bait colBait))
            {
                BaitCollision?.Invoke(colBait);
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