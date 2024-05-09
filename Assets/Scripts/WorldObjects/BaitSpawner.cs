using System;
using System.Collections;
using System.Collections.Generic;
using Installers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WorldObjects
{
    public class BaitSpawner : MonoBehaviour
    {
        [SerializeField] private List<Bait> _baits;
        private Settings _mySettings;

        private void Awake()
        {
            _mySettings = ProjectInstaller.Instance.GameSettings.BaitSpawnerSettings;
            SpawnAllBaits();
        
            StartCoroutine(SpawnRoutine());
        }

        private void SpawnAllBaits()
        {
            _baits = new List<Bait>();
        
            for(int i = 0; i < _mySettings.InitBaitCount; i ++) SpawnRandBait();
        }

        private void SpawnRandBait()
        {
            float randX = Random.Range(0f, _mySettings.TerrainSize.x);
            float randZ = Random.Range(0f, _mySettings.TerrainSize.y);
            
            Vector3 newBaitPos = new(randX, _mySettings.SpawnOffSetY, randZ);

            GameObject newBaitGo = Instantiate(_mySettings.BaitPrefab, transform);

            newBaitGo.transform.localPosition = newBaitPos;
            Bait newBait = newBaitGo.GetComponent<Bait>();
        
            _baits.Add(newBait);
        
            newBait.Eaten += OnBaitEaten;
        }

        private void OnBaitEaten(Bait eatenBait)
        {
            eatenBait.Eaten -= OnBaitEaten;

            _baits.Remove(eatenBait);

            Destroy(eatenBait.gameObject);
        }

        private IEnumerator SpawnRoutine()
        {
            while(true)
            {
                if(_baits.Count < _mySettings.InitBaitCount) SpawnRandBait();
            
                yield return new WaitForSeconds(_mySettings.BaitSpawnFreq);
            }
        }

        [Serializable]
        public class Settings
        {
            public Vector2Int TerrainSize => _terrainSize;
            public int InitBaitCount => _initBaitCount;
            public float BaitSpawnFreq => _baitSpawnFreq;
            public GameObject BaitPrefab => _baitPrefab;
            public float SpawnOffSetY => _spawnOffSetY;
            [SerializeField] private Vector2Int _terrainSize;
            [SerializeField] private int _initBaitCount = 50;
            [SerializeField] private float _baitSpawnFreq = 1f;
            [SerializeField] private GameObject _baitPrefab;
            [SerializeField] private float _spawnOffSetY = 0.25f;
        }
    }
}