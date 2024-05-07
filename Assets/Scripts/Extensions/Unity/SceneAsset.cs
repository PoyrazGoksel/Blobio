using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extensions.Unity
{
    [Serializable][Obsolete]
    public class SceneAsset : Object, ISerializationCallbackReceiver
    {
        public string SceneName => _sceneName;
        [SerializeField] private string _sceneName;
        public void OnBeforeSerialize()
        {
            _sceneName = name;
        }

        public void OnAfterDeserialize()
        {
        }
    }
}