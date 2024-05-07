using System.Linq;
using UnityEngine.Events;

namespace Extensions.Unity
{
    public static class UnityActionExt
    {
        public static bool Contains(this UnityAction action, object o)
        {
            return action.GetInvocationList().Any(il => il.Target == o);
        }

        public static bool Contains<T>(this UnityAction<T> action, object o)
        {
            return action.GetInvocationList().Any(il => il.Target == o);
        }

        public static bool Contains<T, T1>(this UnityAction<T, T1> action, object o)
        {
            return action.GetInvocationList().Any(il => il.Target == o);
        }

        public static bool Contains<T, T1, T2>(this UnityAction<T, T1 , T2> action, object o)
        {
            return action.GetInvocationList().Any(il => il.Target == o);
        }
    }
}