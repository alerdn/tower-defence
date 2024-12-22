using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Maps")]
    [SerializeField] private List<Sprite> _mapsList;

    [Header("Components")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private ObjectPlacer _placer;
    [SerializeField] private MapController _mapController;

    [Header("Enemy")]
    [NonReorderable]
    [SerializeField] private List<WaveData> _waves;
    [SerializeField] private int _coinsOnKill;

    [Header("Gun")]
    [SerializeField] private GameObject _gunPrefab;
    [SerializeField] private int _gunCost;

    [Header("Debug")]
    [SerializeField] private int _coins;
    [SerializeField] private int _wave;

    private void Start()
    {
        _inputReader.SetControllerMode(ControllerMode.Battle);
        _inputReader.PlaceObjectEvent += TryPlaceObject;

        StartCoroutine(Init());
    }

    private void OnDestroy()
    {
        _inputReader.PlaceObjectEvent -= TryPlaceObject;
    }

    private IEnumerator Init()
    {
        yield return _mapController.Generate(_mapsList.GetRandom());
        yield return SpawnEnemy();
    }

    private void TryPlaceObject()
    {
        if (_coins >= _gunCost && _placer.TryPlaceObject(_gunPrefab, _inputReader.MousePositionValue))
        {
            _coins -= _gunCost;
        }
    }

    private IEnumerator SpawnEnemy()
    {
        _wave = 0;
        foreach (WaveData config in _waves)
        {
            _wave++;
            yield return new WaitForSeconds(config.DelayToStart);

            foreach (var enemyConfig in config.Enemies)
            {
                for (int i = 0; i < enemyConfig.Amount; i++)
                {
                    EnemyController enemy = Instantiate(enemyConfig.EnemyPrefab, GameObject.FindWithTag("Start").transform.position + Vector3.up / 2, Quaternion.identity, transform);
                    enemy.OnDeath += (EnemyController enemyController, bool isKilled) =>
                    {
                        if (isKilled)
                        {
                            _coins += _coinsOnKill;
                        }
                        else
                        {
                            PlayerController.Instance.TakeDamage(1);
                        }
                    };

                    yield return new WaitForSeconds(enemyConfig.SpawnInterval);
                }
            }
        }
    }
}