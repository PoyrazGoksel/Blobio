using Extensions.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace WorldObjects
{
    public class Bait : MonoBehaviour
    {
        public event UnityAction<Bait> Eaten;

        public TransformEncapsulated TransformEncapsulated{get;set;}

        private void Awake()
        {
            TransformEncapsulated = new TransformEncapsulated(transform);
        }

        public void OnEaten()
        {
            Eaten?.Invoke(this);
        }
    }
}