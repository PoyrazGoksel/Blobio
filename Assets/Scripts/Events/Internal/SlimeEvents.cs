using Slimes;
using UnityEngine;
using UnityEngine.Events;
using WorldObjects;

namespace Events.Internal
{
    public class SlimeEvents : MonoBehaviour
    {
        public UnityAction<Bait> BaitCollision;
        public UnityAction<Slime> EnemyDetected;
    }
}