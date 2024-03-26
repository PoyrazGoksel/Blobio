using UnityEngine;

namespace Slimes
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private SlimeCollisionDetector _slimeCollisionDetector;

        private void Awake()
        {
            _slimeCollisionDetector.BaitCollision += OnBaitCollision;
        }

        private void OnBaitCollision(Bait colBait)
        {
            Debug.LogWarning($"ColBait {colBait}");
        }
    }
}