using System;
using System.Collections.Generic;
using Installers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Slimes.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        private Terrain _currTerrain;
        private List<Vector2Int> _enemySpawnAllocation = new();
        private Settings _mySettings;

        private void Awake()
        {
            _mySettings = ProjectInstaller.Instance.GameSettings.EnemySpawnerSettings;
        }

        private void Start()
        {
            _currTerrain = FindObjectOfType<Terrain>();
            SpawnAllEnemies(_currTerrain.terrainData.size);
        }

        private void SpawnAllEnemies(Vector3 terrainDataSize)
        {
            Vector2Int terrainSize2d = Vector2Int.FloorToInt(terrainDataSize);

            for(int x = 0; x < terrainSize2d.x; x ++)
            {
                for(int z = 0; z < terrainDataSize.z; z ++)
                {
                    _enemySpawnAllocation.Add(new Vector2Int(x,z));
                }
            }

            for(int i = 0; i < _mySettings.EnemyCount; i ++)
            {
                Vector2Int randSpawnPos = _enemySpawnAllocation[Random.Range
                (0, _enemySpawnAllocation.Count - 1)];
            
                SpawnEnemy(randSpawnPos);

                _enemySpawnAllocation.Remove(randSpawnPos);
            }
        }

        private void SpawnEnemy(Vector2Int position)
        {
            GameObject enemy = Instantiate
            (
                _mySettings.EnemyPrefab,
                transform
            );

            Vector3 transformLocalPosition = new(position.x, 0, position.y);
        
            enemy.transform.localPosition = transformLocalPosition;
        }
    
        [Serializable]
        public class Settings
        {
            [SerializeField] private GameObject _enemyPrefab;
            public GameObject EnemyPrefab => _enemyPrefab;
            [SerializeField] private int _enemyCount = 100;
            public int EnemyCount => _enemyCount;
        }
    }
}