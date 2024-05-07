using System;

namespace Extensions.Unity
{
    [Serializable]
    public class JsonVersionWrapper
    {
        public int V;

        protected virtual void Update(int lastVer, int newVer)
        {
            // for (int i = lastVer; i <= newVer; i++)
            // {
            //     switch (lastVer)
            //     {
            //         case 1:
            //             break;
            //         case 2:
            //             break;
            //     }
            // }
        }
    }
}