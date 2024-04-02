using UnityEngine;
using WorldObjects;

namespace Slimes
{
    public abstract class Slime : MonoBehaviour
    {
        [SerializeField] protected int _score;
        public int Score => _score;

        protected virtual void OnBaitCollision(Bait colBait)
        {
            _score ++;
        }
    }
}