using System;
using System.Collections;
using System.Reflection;
using UnityEngine.Events;

namespace Extensions.Unity
{
    public static class UnityEventsBaseExt
    {
        public static int GetListenerCount(this UnityEventBase unityEvent)
        {
            FieldInfo mCalls = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);


            object invokeCallList = mCalls?.GetValue(unityEvent);
            PropertyInfo property = invokeCallList?.GetType().GetProperty("Count");
            if (property != null)
            {
                return (int)property.GetValue(invokeCallList);
            }

            return default;
        }

        public static bool Contains(this UnityEventBase unityEvent, object targetInstance, UnityAction method)
        {
            FieldInfo mCalls = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            object mCallsVal = mCalls?.GetValue(unityEvent);
            PropertyInfo mCallsCountInfo = mCallsVal?.GetType().GetProperty("Count");

            if (mCallsVal == null) return false;

            if (mCallsCountInfo == null) return false;
            
            if ((int)(mCallsCountInfo.GetValue(mCallsVal)) == 0)
            {
                return false;
            }

            FieldInfo mRuntimeCalls = mCallsVal.GetType().GetField("m_RuntimeCalls",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            IList mRuntimeCallsVal = (IList) mRuntimeCalls?.GetValue(mCallsVal);

            if (mRuntimeCallsVal == null ) return false;

            foreach (object mRuntimeCall in mRuntimeCallsVal)
            {
                bool isFound = (bool)(mRuntimeCall.GetType().InvokeMember("Find",
                    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly,
                    Type.DefaultBinder, mRuntimeCall, new[] { targetInstance, method.Method }));

                 if (isFound)
                 {
                     return true;
                 }
            }
            return false;
        }
    }
}