using System.Collections.Generic;
using Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Enemy Settings")] 
        [SerializeField] private List<Material> _materialsByHP;
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private float _minimumDelayBetweenSpanws;
        [SerializeField] private float _maxDelayBetweenSpanws;
        // TODO: difficulty multiplier
        
        private float _timeToNextSpawn;
        private bool _isPlaying;
        
        public static GameManager Instance { get; private set; }
        public string EnemyTag => "Enemy"; 

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // TODO: wait for 3 2 1 
            StartGame();
        }
        
        private void StartGame()
        {
            _isPlaying = true;
            SetNextSpawnTime();
        }

        public void Update()
        {
            if (_isPlaying)
            {
                _timeToNextSpawn -= Time.deltaTime;
                if (_timeToNextSpawn < 0)
                {
                    SpawnNewEnemy();
                    SetNextSpawnTime();
                }
            }
        }
        
        private void SpawnNewEnemy()
        {
            var newEnemy = EnemySpawner.BaseInstance.Spawn();
            var spawnPoint = _spawnPoints.GetRandom();
            newEnemy.Initialize(spawnPoint.position, GetRandomEnemyHealth());
            newEnemy.UpdateMoving(true);
        }

        private int GetRandomEnemyHealth()
        {
            return Random.Range(1, _materialsByHP.Count + 1);
        }

        public Material GetEnemyMaterial(int remainingHealth)
        {
            var index = remainingHealth - 1;
            var materialShouldExist = index > -1 && index < _materialsByHP.Count;
            if (materialShouldExist)
            {
                return _materialsByHP[index];   
            }
            else
            {
                return null;
            }
        }
        
        public void BulletHitEnemy(Bullet bullet, Enemy enemy)
        {
            BulletSpawner.BaseInstance.Release(bullet);
            var shouldDestroy = enemy.ReduceHealth();
            if (shouldDestroy)
            {
                enemy.Explode();
                EnemySpawner.BaseInstance.Release(enemy);
                
                // increase score + hud
            }
        }

        private void SetNextSpawnTime()
        {
            _timeToNextSpawn = Random.Range(_minimumDelayBetweenSpanws, _maxDelayBetweenSpanws);
        }
    }
}