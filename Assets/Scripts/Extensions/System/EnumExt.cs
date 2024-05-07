using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.System
{
    public static class EnumExt
    {
        public static List<T> GetValuesList<T>(this Enum thisEnum)
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static T LastElem<T>(this Enum thisEnum)
        {
            List<T> valuesList = thisEnum.GetValuesList<T>();
            return valuesList.Last();
        }
        
        public static T FirstElem<T>(this Enum thisEnum)
        {
            List<T> valuesList = thisEnum.GetValuesList<T>();
            return valuesList.First();
        }
        
        public static int Count(this Enum thisEnum)
        {
            List<Enum> valuesList = thisEnum.GetValuesList<Enum>();
            return valuesList.Count;
        }
    }
}