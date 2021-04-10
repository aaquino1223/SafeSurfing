using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing 
{
    public class PullController : TrapController<Vector3>
    {
        public override Vector3 GetEffect(GameObject player)
        {
            var playerPosition = player.transform.localPosition;

            var parentPosition = transform.parent.localPosition;

            return (parentPosition - playerPosition).normalized * 4;

        }
    }
}
