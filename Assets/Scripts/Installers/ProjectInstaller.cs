using Datas;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Installers
{
    public class ProjectInstaller
    {
        public static ProjectInstaller Instance{get;private set;}
        public static UnityAction ProjectInstalled;
        
        public GameSettings GameSettings{get;private set;}
        public Camera MainCam{get;private set;}

        //This executes before awake of first scene
        private ProjectInstaller()
        {
            if(Instance != null) return;
            SceneManager.sceneLoaded += OnSceneLoaded;

            Instance = this;

            InstallSettings();
            ProjectInstalled?.Invoke();
        }

        private void InstallSettings()
        {
            GameSettings = Resources.Load<GameSettings>("GameSettings");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoad()
        {
            ProjectInstaller projectInstaller = new();
        }

        private void OnSceneLoaded(Scene loadedScene, LoadSceneMode arg1)
        {
            if(loadedScene.name == "Main")
            {
                MainCam = Camera.main;
            }
        }
    }
}