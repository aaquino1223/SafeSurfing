using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing.Common.Interfaces
{
    public interface IHeading
    {
        Vector3 Heading { get; }
    }

    public interface ICanSpawnEnemy
    {
        event EventHandler<IEnumerable<EnemyController>> SpawnedEnemies;
    }
}
