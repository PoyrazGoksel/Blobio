using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extensions.Unity
{
    /// TODO:Prevent null possible exceptions
    public static class EDebug
    {
        public static void Method()
        {
            if (! Debug.isDebugBuild) return;

            string methodInfoLine = StackTraceUtility.ExtractStackTrace()
            .Split(new[] {"\n"}, StringSplitOptions.None)[2];

            Debug.LogWarning(methodInfoLine.Split(new[] {" "}, StringSplitOptions.None)[0]);
        }

        public static void Method(object debugMessage, bool writeToEnd = false)
        {
            if (! Debug.isDebugBuild) return;

            string methodInfoLine = StackTraceUtility.ExtractStackTrace()
            .Split(new[] {"\n"}, StringSplitOptions.None)[2];

            if (writeToEnd)
            {
                Debug.LogWarning
                (
                    methodInfoLine.Split
                    (new[] {" "}, StringSplitOptions.None)[0] +
                    "\n <-" +
                    debugMessage
                );
            }
            else
            {
                Debug.LogWarning
                (
                    debugMessage +
                    "<-" +
                    methodInfoLine.Split(new[] {" "}, StringSplitOptions.None)[0]
                );
            }
        }

        public static void FilterLogMethod(int numberOfPreviousMethods, string filterAssembly)
        {
            if (! Debug.isDebugBuild) return;

            string[] methodInfoLines = StackTraceUtility.ExtractStackTrace()
            .Split(new[] {"\n"}, StringSplitOptions.None);

            List<string> methodInfoLinesClean = new();

            foreach (string methodInfoLine in methodInfoLines)
            {
                if (! methodInfoLine.Contains(filterAssembly + "."))
                    methodInfoLinesClean.Add(methodInfoLine);
            }

            string methodNamesToWrite = string.Empty;

            for (int i = 0; i < numberOfPreviousMethods; i ++)
            {
                if (2 + i == methodInfoLinesClean.Count) break;

                if (i > 0)
                {
                    methodNamesToWrite += "\n <= " +
                    methodInfoLinesClean[2 + i]
                    .Split(new[] {" "}, StringSplitOptions.None)[0]
                    .Split(new[] {"."}, StringSplitOptions.None)
                    .Last();
                }
                else
                {
                    methodNamesToWrite += methodInfoLinesClean[2 + i]
                    .Split(new[] {" "}, StringSplitOptions.None)[0]
                    .Split(new[] {"."}, StringSplitOptions.None)
                    .Last();
                }
            }

            Debug.LogWarning(methodNamesToWrite);
        }

        public static void FilterLogMethod
        (
            object debugMessage = default,
            int numberOfPreviousMethods = 2,
            string filterAssembly = "strange",
            bool writeToEnd = false
        )
        {
            if (! Debug.isDebugBuild) return;

            string[] methodInfoLines = StackTraceUtility.ExtractStackTrace()
            .Split(new[] {"\n"}, StringSplitOptions.None);

            List<string> methodInfoLinesClean = new();

            foreach (string methodInfoLine in methodInfoLines)
            {
                if (! methodInfoLine.Contains(filterAssembly + "."))
                    methodInfoLinesClean.Add(methodInfoLine);
            }

            string methodNamesToWrite = string.Empty;

            for (int i = 0; i < numberOfPreviousMethods; i ++)
            {
                if (2 + i == methodInfoLinesClean.Count) break;

                if (i > 0)
                {
                    methodNamesToWrite += "\n <= " +
                    methodInfoLinesClean[2 + i]
                    .Split(new[] {" "}, StringSplitOptions.None)[0]
                    .Split(new[] {"."}, StringSplitOptions.None)
                    .Last();
                }
                else
                {
                    methodNamesToWrite += methodInfoLinesClean[2 + i]
                    .Split(new[] {" "}, StringSplitOptions.None)[0]
                    .Split(new[] {"."}, StringSplitOptions.None)
                    .Last();
                }
            }

            if (writeToEnd)
            {
                Debug.LogWarning(methodNamesToWrite + "\n <-" + debugMessage);
            }
            else
            {
                Debug.LogWarning(debugMessage + "<-" + methodNamesToWrite);
            }
        }

        public static void Log(object message, object context = null)
        {
            if (Debug.isDebugBuild == false) return;

            Debug.Log(message, (Object)context);
        }

        public static void LogW(object message, object context = null)
        {
            if (Debug.isDebugBuild == false) return;

            Debug.LogWarning(message, (Object)context);
        }
    }
}