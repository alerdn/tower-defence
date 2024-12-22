using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "WaveData")]
public class WaveData : ScriptableObject
{
    [Serializable]
    public record EnemyConfig
    {
        public EnemyController EnemyPrefab;
        public int Amount;
        public float SpawnInterval;
    }

    public List<EnemyConfig> Enemies;
    public float DelayToStart;
}