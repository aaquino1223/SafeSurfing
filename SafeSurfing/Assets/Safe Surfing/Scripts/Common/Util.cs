using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SafeSurfing.Common
{
    public static class Util
    {
        public static IEnumerator TimedAction(UnityAction before, UnityAction after, float time)
        {
            before?.Invoke();
            yield return new WaitForSeconds(time);
            after?.Invoke();
        }


    }
}