using SafeSurfing.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public abstract class TrapController<T> : MonoBehaviour
    {
        public abstract T GetEffect(GameObject player = null);
    }
}