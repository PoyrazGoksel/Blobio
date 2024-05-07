using Events.Internal;
using UnityEngine;

namespace Slimes.Enemies
{
    public class SlimeEnemyDetector : MonoBehaviour
    {
        [SerializeField] private SlimeEvents _slimeEvents;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Slime slime))
            {
                _slimeEvents.EnemyDetected?.Invoke(slime);
            }
        }
    }
}