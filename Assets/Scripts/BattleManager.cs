using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public record WaveConfig
{
    [Serializable]
    public record EnemyConfig
    {
        public EnemyController EnemyPrefab;
        public int Amount;
    }

    [NonReorderable]
    public List<EnemyConfig> Enemies;
    public float Duration;
}

public class BattleManager : MonoBehaviour
{
    [Header("Maps")]
    [SerializeField] private List<Sprite> _mapsList;

    [Header("Components")]
    [SerializeField] private ObjectPlacer _placer;
    [SerializeField] private MapController _mapController;

    [Header("Enemy")]
    [NonReorderable]
    [SerializeField] private List<WaveConfig> _waves;

    [Header("Gun")]
    [SerializeField] private GameObject _gunPrefab;
    [SerializeField] private int _gunCost;

    [Header("Debug")]
    [SerializeField] private int _health;
    [SerializeField] private int _coins;
    [SerializeField] private int _enemiesSpawned;
    [SerializeField] private int _wave;

    private List<EnemyController> _enemies = new List<EnemyController>();

    private void Start()
    {
        _mapController.Generate(_mapsList.GetRandom());
        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _coins >= _gunCost)
        {
            _placer.PlaceObject(_gunPrefab);
            _coins -= _gunCost;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _coins += 10;
        }
    }

    private IEnumerator SpawnEnemy()
    {
        _wave = 0;
        foreach (WaveConfig config in _waves)
        {
            _wave++;

            int enemiesAmount = config.Enemies.Sum(enemyConfig => enemyConfig.Amount);
            float spawnRate = enemiesAmount / config.Duration;
            int enemyToSpawnCount = enemiesAmount;

            while (enemyToSpawnCount > 0)
            {
                yield return new WaitForSeconds(1f / spawnRate);

                var enemyConfig = config.Enemies.Find(enemyConfig => enemyConfig.Amount > 0);
                EnemyController enemy = Instantiate(enemyConfig.EnemyPrefab, GameObject.FindWithTag("Start").transform.position, Quaternion.identity);
                enemyConfig.Amount--;

                enemy.OnDeath += (EnemyController enemyController, bool isKilled) =>
                {
                    if (isKilled)
                    {
                        _coins += 5;
                    }
                    else
                    {
                        _health -= 5;
                    }

                    _enemies.Remove(enemyController);
                };

                _enemies.Add(enemy);
                enemyToSpawnCount--;

                _enemiesSpawned++;
            }
        }
    }
}