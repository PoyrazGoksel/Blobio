using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#if UNITY_EDITOR
using UnityEditor; 
#endif
using UnityEngine;

namespace Extensions.System
{
    public class GenerateRefCode
    {
#if UNITY_EDITOR

        public GenerateRefCode(string callingTypeName, string typeToAddName)
        {
            string refVarName = "_my" + typeToAddName;

            string scriptFile = Directory.EnumerateFiles
            (
                Application.dataPath + "/Scripts",
                callingTypeName + ".cs",
                SearchOption.AllDirectories
            )
            .FirstOrDefault();

            FileStream read = File.OpenRead(scriptFile);

            StreamReader streamReader = new StreamReader(read, Encoding.UTF8);

            string fileContents = streamReader.ReadToEnd();

            streamReader.Close();

            if (fileContents.Contains(refVarName))
            {
                Debug.LogWarning("This component already has this reference serialized!");
                return;
            }

            List<string> fileLines = new List<string>();
            string tempLineAccum = "";

            for (int i = 0; i < fileContents.Length; i ++)
            {
                char fileContent = fileContents[i];
                tempLineAccum += fileContent;

                if (fileContent == '\n')
                {
                    fileLines.Add(tempLineAccum);
                    tempLineAccum = "";
                }
            }
            
            fileLines.Add(tempLineAccum);

            var lineNumPairList = fileLines.Select((line, index) => new {line, index})
            .ToList();

            int typeDecLineIndex = lineNumPairList.First(pair => pair.line.Contains("class"))
            .index;

            int typeScopeStartLine = 0;

            for (int i = typeDecLineIndex; i < fileLines.Count; i ++)
            {
                if (fileLines[i]
                    .Contains('{'))
                {
                    typeScopeStartLine = i;
                    break;
                }
            }

            if (fileLines[typeDecLineIndex]
                .Contains('{'))
            {
                //TODO: TEMP
                return;
            }

            List<string> fileLinesList = fileLines.ToList();
            
            fileLinesList.Insert
            (
                typeScopeStartLine + 1,
                $"        [SerializeField] private {typeToAddName} {refVarName};\n"
            );

            FileStream fileWrite = File.Open(scriptFile, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileWrite, Encoding.UTF8);

            fileContents = "";

            foreach (string s in fileLinesList)
            {
                fileContents += s;
            }

            streamWriter.Write(fileContents);
            
            streamWriter.Flush();
            streamWriter.Close();
            
            AssetDatabase.Refresh();
        }
        
#endif
    }
}