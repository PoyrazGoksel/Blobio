using System.Collections;
using System.Collections.Generic;
using Slimes;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaitSpawner : MonoBehaviour
{
    [SerializeField] private Vector2Int _terrainSize;
    [SerializeField] private List<Bait> _baits;
    [SerializeField] private int _initBaitCount = 50;
    [SerializeField] private float _baitSpawnFreq = 1f;
    [SerializeField] private Coroutine _coroutine;
    [SerializeField] private GameObject _baitPrefab;
    [SerializeField] private float _spawnOffSetY = 0.25f;

    private void Awake()
    {
        SpawnAllBaits();
        
        _coroutine = StartCoroutine(SpawnRoutine());
    }

    private void SpawnAllBaits()
    {
        _baits = new List<Bait>();
        
        for(int i = 0; i < _initBaitCount; i ++) SpawnRandBait();
    }

    private void SpawnRandBait()
    {
        float randX = Random.Range(0f, _terrainSize.x);
        float randZ = Random.Range(0f, _terrainSize.y);
            
        Vector3 newBaitPos = new(randX, _spawnOffSetY, randZ);

        GameObject newBaitGo = Instantiate(_baitPrefab, transform);

        newBaitGo.transform.localPosition = newBaitPos;
        Bait newBait = newBaitGo.GetComponent<Bait>();
        
        _baits.Add(newBait);
        
        newBait.Eaten += OnBaitEaten;
    }

    private void OnBaitEaten(Bait eatenBait)
    {
        Debug.LogWarning($"SpawnerPreRemove {eatenBait}");
        eatenBait.Eaten -= OnBaitEaten;
        _baits.Remove(eatenBait);
        Destroy(eatenBait.gameObject);
    }

    private IEnumerator SpawnRoutine()
    {
        while(true)
        {
            if(_baits.Count < _initBaitCount) SpawnRandBait();
            
            yield return new WaitForSeconds(_baitSpawnFreq);
        }
    }
}