using Events;
using Events.Internal;
using Extensions.Unity;
using UnityEngine;
using Utils;
using WorldObjects;

namespace Slimes
{
    public abstract class Slime : EventListenerMono, IPausable
    {
        protected const int EatScoreDiff = 2;
        public int Score => _score;
        public TransformEncapsulated Trans{get;set;}
        [SerializeField] private int _score;
        [SerializeField] private Transform _rigTrans; // Declare a field for the model's Transform.
        [SerializeField] protected SlimeEvents SlimeEvents;

        protected virtual void Awake()
        {
            Trans = new TransformEncapsulated(transform);
        }

        void IPausable.UnPause() {UnPause();}

        void IPausable.Pause() {Pause();}

        protected virtual void OnBaitCollision(Bait colBait)
        {
            _score ++;
            IncreaseSize(_score); // You can change this value to your liking
        }

        protected virtual void IncreaseSize(int size)
        {
            float newSizeOffset = IncreaseMeshSize(size);
            SlimeEvents.SizeIncrease?.Invoke(newSizeOffset);
        }

        private float IncreaseMeshSize(int size)
        {
            _rigTrans.localScale = SlimeF.CalcRigTransLocalScale
            (size, out float newSizeOffset); // Increase the size of the modelTransform.

            return newSizeOffset;
        }

        protected abstract void Pause();

        protected abstract void UnPause();

        protected override void RegisterEvents() {GameStateEvents.Pause += OnPause;}

        private void OnPause(bool isPaused)
        {
            if(isPaused) Pause();
            else UnPause();
        }

        protected override void UnRegisterEvents() {GameStateEvents.Pause -= OnPause;}

        public void Eaten() {gameObject.Destroy();}
    }

    public interface IPausable
    {
        void Pause();

        void UnPause();
    }
}