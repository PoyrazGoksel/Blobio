using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Extensions.Unity
{
    public static class MonoBehaviourExt
    {
        public static void WaitFor
        (this MonoBehaviour mono, YieldInstruction wait, UnityAction action)
        {
            mono.StartCoroutine(Wait(wait, action));
        }

        private static IEnumerator Wait(YieldInstruction waitFor, UnityAction action)
        {
            yield return waitFor;
            action.Invoke();
        }
    }
}