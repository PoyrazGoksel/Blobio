using UnityEngine;
using UnityEngine.Events;

namespace WorldObjects
{
    public class Bait : MonoBehaviour
    {
        public event UnityAction<Bait> Eaten;
        private Vector3 _initPos;
        public Vector3 InitPos => _initPos;


        private void Start()
        {
            _initPos = transform.position;
        }

        public void OnEaten()
        {
            Eaten?.Invoke(this);
        }
    }
}