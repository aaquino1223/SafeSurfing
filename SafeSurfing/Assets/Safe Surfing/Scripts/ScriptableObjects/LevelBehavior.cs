using SafeSurfing.Common.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing 
{
    [CreateAssetMenu]
    public class LevelBehavior : ScriptableObject
    {
        public Wave[] Waves;
        public PickUpType PickUpType;
        public AudioClip AudioClip;
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
        public JustInTimeInstruction JustInTime;
    }
}