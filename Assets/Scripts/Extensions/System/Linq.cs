using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.System
{
    public static class Linq
    {
        public static IEnumerable<T> DoToAll<T>(this IEnumerable<T> thisIEnumerable, Action<T> func)
        {
            IEnumerable<T> doToAll = thisIEnumerable as T[] ?? thisIEnumerable.ToArray();

            foreach (T variable in doToAll)
            {
                func(variable);
            }

            return doToAll;
        }
        
        public static int ToIndex(this int i)
        {
            if (i <= 0)
            {
                return 0;
            }

            return i - 1;
        }
        
        public static int ToCount(this int i)
        {
            if (i <= 0)
            {
                return 1;
            }

            return i + 1;
        }
        
        public static T Random<T>(this IEnumerable<T> thisCollection, int min = 0, int max = 0)
        {
            List<T> thisList = thisCollection.ToList();
            int count = thisList.Count;
            
            if (max == 0)
            {
                max = count;
            }
            
            int rand = UnityEngine.Random.Range(min, max);
            return thisList[rand];
        }

        public static List<T> Clone<T>(this List<T> thisCollection)
        {
            return new List<T>(thisCollection);
        }
        
        public static List<T> ToList<T>(this T[,,] arr)
        {
            List<T> newList = new List<T>();
            foreach (T item in arr)
            {
                newList.Add(item);
            }

            return newList;
        }

        public static List<T> ToList<T>(this T[,] arr)
        {
            List<T> newList = new List<T>();
            foreach (T VARIABLE in arr)
            {
                newList.Add(VARIABLE);
            }

            return newList;
        }
        
        public static T RandomOrDefault<T>(this IEnumerable<T> thisCollection)
        {
            List<T> thisList = thisCollection.ToList();
            if (thisList.Count == 0)
            {
                return default;
            }
            List<T> filtered = thisList.NotNull().ToList();
            
            int rand = UnityEngine.Random.Range(0, filtered.Count);
            return filtered[rand];
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> thisCollection)
        {
            return thisCollection.Where(e => e != null);
        }
        
        public static IEnumerable<T> OnlyNull<T>(this IEnumerable<T> thisCollection)
        {
            return thisCollection.Where(e => e == null);
        }

        public static bool SafeAdd<T,T1>(this Dictionary<T,T1> thisDict, T key, T1 val)
        {
            if (thisDict.ContainsKey(key) == false)
            {
                thisDict.Add(key, val);
                return true;
            }

            return false;
        }
        
        public static void SafeWhile(this object thisObj, Func<bool> condition,Action action, int repeat, Action safeExitAction = null)
        {
            int safe = repeat;
            
            while (condition() && safe > 0)
            {
                action.Invoke();
                safe --;
            }

            if (safeExitAction != null && safe == 0)
            {
                safeExitAction.Invoke();
            }
        }
    }
}