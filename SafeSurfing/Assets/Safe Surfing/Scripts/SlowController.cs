using SafeSurfing.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class SlowController : TrapController<float>
    {
        public float SpeedDecrease = 2.5f;

        public override float GetEffect(GameObject player = null)
        {
            return SpeedDecrease;
        }

        private void Start()
        {
            StartCoroutine(Util.TimedAction(null, () => Destroy(gameObject), 5f));
        }
    }
}