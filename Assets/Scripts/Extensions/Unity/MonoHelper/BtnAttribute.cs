using System;

namespace Extensions.Unity.MonoHelper
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class BtnAttribute : Attribute
    {
        public string Name { get; private set; }

        public BtnAttribute(string name)
        {
            Name = name;
        }
    }
}