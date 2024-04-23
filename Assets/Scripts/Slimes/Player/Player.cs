using Events;
using Events.Internal;
using Installers;
using UnityEngine;
using WorldObjects;

namespace Slimes.Player
{
    public class Player : Slime
    {
        [SerializeField] private SlimeCollisionDetector _slimeCollisionDetector;
        [SerializeField] private SlimeEvents _slimeEvents;

        protected override void IncreaseSize(int size)
        {
            base.IncreaseSize(size);
            PlayerEvents.SizeIncreased?.Invoke(size);
        }

        protected override void RegisterEvents()
        {
            _slimeEvents.BaitCollision += OnBaitCollision;
        }

        protected override void OnBaitCollision(Bait colBait)
        {
            base.OnBaitCollision(colBait);
            PlayerEvents.PlayerBaitConsume?.Invoke(Score);
        }

        protected override void UnRegisterEvents()
        {
            _slimeEvents.BaitCollision -= OnBaitCollision;
        }
    }
}