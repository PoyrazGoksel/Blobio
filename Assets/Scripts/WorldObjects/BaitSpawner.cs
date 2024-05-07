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
        [SerializeField] private GameObject _eatenParticleSystemPrefab;
        
        private Coroutine _coroutine;
        private Settings _mySettings;
        private void Awake()
        {
            _mySettings = ProjectInstaller.Instance.GameSettings.BaitSpawnerSettings;
            SpawnAllBaits();
        
            _coroutine = StartCoroutine(SpawnRoutine());
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

            GameObject newParticleSystemGo = Instantiate
            (_eatenParticleSystemPrefab, eatenBait.TransformEncapsulated.position, Quaternion.identity);
            _baits.Remove(eatenBait);

            StartCoroutine(ParticleDelayedDestroy(newParticleSystemGo, 2f));
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

        private IEnumerator ParticleDelayedDestroy(GameObject particleSystem, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            Destroy(particleSystem);
        }

        [Serializable]
        public class Settings
        {
            [SerializeField] private Vector2Int _terrainSize;
            [SerializeField] private int _initBaitCount = 50;
            [SerializeField] private float _baitSpawnFreq = 1f;
            [SerializeField] private GameObject _baitPrefab;
            [SerializeField] private float _spawnOffSetY = 0.25f;

            public Vector2Int TerrainSize => _terrainSize;
            public int InitBaitCount => _initBaitCount;
            public float BaitSpawnFreq => _baitSpawnFreq;
            public GameObject BaitPrefab => _baitPrefab;
            public float SpawnOffSetY => _spawnOffSetY;
        }
    }
}