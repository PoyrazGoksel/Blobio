using UnityEngine;
using UnityEngine.Events;

namespace Slimes
{
    public class Bait : MonoBehaviour
    {
        public event UnityAction<Bait> Eaten;
    
        public void OnEaten()
        {
            Eaten?.Invoke(this);
        }
    }
}