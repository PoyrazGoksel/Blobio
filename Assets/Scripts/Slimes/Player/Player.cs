using Events;
using UnityEngine;
using WorldObjects;

namespace Slimes.Player
{
    public class Player : Slime
    {
        [SerializeField] private SlimeCollisionDetector _slimeCollisionDetector;

        private void OnEnable()
        {
            _slimeCollisionDetector.BaitCollision += OnBaitCollision;
        }

        protected override void OnBaitCollision(Bait colBait)
        {
            base.OnBaitCollision(colBait);
            PlayerEvents.PlayerBaitConsume?.Invoke(Score);
        }

        private void OnDisable()
        {
            _slimeCollisionDetector.BaitCollision -= OnBaitCollision;
        }
    }
}