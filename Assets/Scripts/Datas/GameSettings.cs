using Slimes.Enemies;
using Slimes.Pathfinding;
using Slimes.Player;
using UnityEngine;
using WorldObjects;

namespace Datas
{
    [CreateAssetMenu(fileName = nameof(GameSettings), menuName = "Blobio/" + nameof(GameSettings), order = 0)]
    public class GameSettings : ScriptableObject
    {
        
        public PlayerCam.Settings PlayerCamSettings => _playerCamSettings;
        public EnemySpawner.Settings EnemySpawnerSettings => _enemySpawnerSettings;
        [SerializeField] private PlayerCam.Settings _playerCamSettings;
        [SerializeField] private EnemySpawner.Settings _enemySpawnerSettings;
        [SerializeField] private BaitSpawner.Settings _baitSpawnerSettings;
        [SerializeField] private PlayerPathSetter.Settings _playerPathSetterSettings;

        public PlayerPathSetter.Settings PlayerPathSetterSettings => _playerPathSetterSettings;

        
        public BaitSpawner.Settings BaitSpawnerSettings => _baitSpawnerSettings;

        
    }
}