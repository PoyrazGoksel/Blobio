using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _enemyCount = 100;
    [SerializeField] private GameObject _enemyPrefab;
    
    private Terrain _currTerrain;
    private List<Vector2Int> _enemySpawnAllocation = new();

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

        for(int i = 0; i < _enemyCount; i ++)
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
            _enemyPrefab,
            transform
        );

        Vector3 transformLocalPosition = new(position.x, 0, position.y);
        
        enemy.transform.localPosition = transformLocalPosition;
    }
}