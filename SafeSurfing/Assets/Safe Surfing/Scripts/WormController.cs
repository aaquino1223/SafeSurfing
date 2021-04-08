using SafeSurfing.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class WormController : FollowPlayerController
    {
        protected override IEnumerable<Vector3> SpawnBehavior()
        {
            return new List<Vector3>() { new Vector3(transform.localPosition.x, _YMax - 2f, 0) };
        }
    }
}