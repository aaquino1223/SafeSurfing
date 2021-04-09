using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing.Common.Enums
{
    public enum EnemyState
    {
        Spawned,
        Normal
    }

    [Flags]
    public enum PickUpType
    {
        BulletSpeed = 1 << 0,
        FiringRate = 1 << 1,
        MoveSpeed = 1 << 2,
        Special = 1 << 3,
        Trojan = 1 << 4
    }

    public enum JITType
    {
        Tutorial,
        Virus,
        Trojan,
        Worm,
        DDOS,
        Boss,
        BulletSpeed,
        MoveSpeed,
        FiringRate,
        Special,
        
    }
}