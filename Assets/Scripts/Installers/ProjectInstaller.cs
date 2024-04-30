using Datas;
using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Installers
{
    public class ProjectInstaller
    {
        public static ProjectInstaller Instance{get;private set;}
        public static UnityAction ProjectInstalled;
        private PlayerData _playerData;
        public GameSettings GameSettings{get;private set;}
        public Camera MainCam{get;private set;}
        public PlayerData PlayerData => _playerData;

        //This executes before awake of first scene
        private ProjectInstaller()
        {
            if(Instance != null) return;
            SceneManager.sceneLoaded += OnSceneLoaded;

            Instance = this;

            RegisterEvents();
            InstallSettings();
            InstallData();
            ProjectInstalled?.Invoke();
        }

        private void InstallData()
        {
            _playerData = new PlayerData();
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

        private static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void RegisterEvents()
        {
            MainMenuEvents.NewGameBTN += OnNewGameBTN;
            MainUIEvents.ExitBTN += OnExitBTN;
        }

        private void OnExitBTN()
        {
            LoadScene("MainMenu");
        }

        private void OnNewGameBTN()
        {
            LoadScene("Main");
        }
    }
}