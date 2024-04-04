using Events;
using UnityEngine;
using WorldObjects;

namespace Slimes.Player
{
    public class Player : Slime
    {
        [SerializeField] private SlimeCollisionDetector _slimeCollisionDetector;

        protected override void IncreaseSize(int size)
        {
            base.IncreaseSize(size);
            PlayerEvents.SizeIncreased?.Invoke(size);
        }

        protected override void RegisterEvents()
        {
            _slimeCollisionDetector.BaitCollision += OnBaitCollision;
        }

        protected override void OnBaitCollision(Bait colBait)
        {
            base.OnBaitCollision(colBait);
            PlayerEvents.PlayerBaitConsume?.Invoke(Score);
        }

        protected override void UnRegisterEvents()
        {
            _slimeCollisionDetector.BaitCollision -= OnBaitCollision;
        }
    }
}