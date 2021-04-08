using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelBehavior : ScriptableObject
{
    public Wave[] Waves;
}

[Serializable]
public class Wave
{
    public SpawnPoint[] SpawnPoints;
}

[Serializable]
public class SpawnPoint
{
    public float Time;
    public GameObject EnemyPrefab;
}