using UnityEngine;
using Utils;
using WorldObjects;

namespace Slimes
{
    public abstract class Slime : EventListenerMono
    {
        public int Score => _score;
        [SerializeField] protected int _score;

        [SerializeField] private Transform _rigTrans; // Declare a field for the model's Transform.

        protected virtual void IncreaseSize(int size)
        {
            _rigTrans.localScale = SlimeF.CalcRigTransLocalScale(size);  // Increase the size of the modelTransform.
        }

        protected virtual void OnBaitCollision(Bait colBait)
        {
            _score ++;
            IncreaseSize(_score); // You can change this value to your liking
        }
    }
}