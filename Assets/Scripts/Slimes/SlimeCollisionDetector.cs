using UnityEngine;
using UnityEngine.Events;
using WorldObjects;

namespace Slimes
{
    public class SlimeCollisionDetector : MonoBehaviour
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
    }
}