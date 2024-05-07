using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Extensions.System;
#if UNITY_EDITOR
using UnityEditor; 
#endif
using UnityEngine;

namespace Extensions.Unity
{
    public class AutoSerializeMyRef
    {
#if UNITY_EDITOR

        [MenuItem("CONTEXT/MonoBehaviour/Serialize/MyTrans")]
        private static void SerializeMyTrans(MenuCommand command)
        {
            GenerateRefCode codeGenerate = new GenerateRefCode
            (
                command.context.GetType()
                .Name, "Transform"
            );

            MonoBehaviour a = ((MonoBehaviour)command.context);

            List<MemberInfo> memlist = a.GetType()
            .GetMembers(BindingFlags.Instance | BindingFlags.NonPublic).ToList();

            Debug.LogWarning(memlist.Count);
            
            foreach (MemberInfo memberInfo in memlist)
            {
                Debug.LogWarning(memberInfo.Name);
            }
        }

        [MenuItem("CONTEXT/MonoBehaviour/Serialize/MySphereCollider")]
        private static void SerializeMySphereCollider(MenuCommand command)
        {
            if (((MonoBehaviour)command.context).TryGetComponent(out SphereCollider _))
            {
                GenerateRefCode codeGenerate = new GenerateRefCode
                (
                    command.context.GetType()
                    .Name, "SphereCollider"
                );   
            }
            else
            {
                Debug.LogWarning("Requested component is missing on this GameObject!");
            }
        }

        [MenuItem("CONTEXT/MonoBehaviour/Serialize/MyBoxCollider")]
        private static void SerializeMyBoxCollider(MenuCommand command)
        {
            if (((MonoBehaviour)command.context).TryGetComponent(out BoxCollider _))
            {
                GenerateRefCode codeGenerate = new GenerateRefCode
                (
                    command.context.GetType()
                    .Name, "BoxCollider"
                );
            } 
            else
            {
                Debug.LogWarning("Requested component is missing on this GameObject!");
            }
        }
        
        [MenuItem("CONTEXT/MonoBehaviour/Serialize/MyMeshRenderer")]
        private static void SerializeMyMeshRenderer(MenuCommand command)
        {
            if (((MonoBehaviour)command.context).TryGetComponent(out MeshRenderer _))
            {
                GenerateRefCode codeGenerate = new GenerateRefCode
                (
                    command.context.GetType()
                    .Name, "MeshRenderer"
                );
            } 
            else
            {
                Debug.LogWarning("Requested component is missing on this GameObject!");
            }
        }
        
        [MenuItem("CONTEXT/MonoBehaviour/Serialize/MySkinnedMeshRenderer")]
        private static void SerializeMySkinnedMeshRenderer(MenuCommand command)
        {
            if (((MonoBehaviour)command.context).TryGetComponent(out SkinnedMeshRenderer _))
            {
                GenerateRefCode codeGenerate = new GenerateRefCode
                (
                    command.context.GetType()
                    .Name, "SkinnedMeshRenderer"
                );
            } 
            else
            {
                Debug.LogWarning("Requested component is missing on this GameObject!");
            }
        }
        
        [MenuItem("CONTEXT/MonoBehaviour/Serialize/MyMeshFilter")]
        private static void SerializeMyMeshFilter(MenuCommand command)
        {
            if (((MonoBehaviour)command.context).TryGetComponent(out MeshFilter _))
            {
                GenerateRefCode codeGenerate = new GenerateRefCode
                (
                    command.context.GetType()
                    .Name, "MeshFilter"
                );
            } 
            else
            {
                Debug.LogWarning("Requested component is missing on this GameObject!");
            }
        }
        
        [MenuItem("CONTEXT/MonoBehaviour/Serialize/MyRigid")]
        private static void SerializeMyRigid(MenuCommand command)
        {
            if (((MonoBehaviour)command.context).TryGetComponent(out Rigidbody _))
            {
                GenerateRefCode codeGenerate = new GenerateRefCode
                (
                    command.context.GetType()
                    .Name, "Rigidbody"
                );
            } 
            else
            {
                Debug.LogWarning("Requested component is missing on this GameObject!");
            }
        }
        
        [MenuItem("CONTEXT/MonoBehaviour/Serialize/MyCamera")]
        private static void SerializeMyCamera(MenuCommand command)
        {
            if (((MonoBehaviour)command.context).TryGetComponent(out Camera _))
            {
                GenerateRefCode codeGenerate = new GenerateRefCode
                (
                    command.context.GetType()
                    .Name, "Camera"
                );
            } 
            else
            {
                Debug.LogWarning("Requested component is missing on this GameObject!");
            }
        }
        
#endif
    }
}
