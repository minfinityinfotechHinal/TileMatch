using System;
using UnityEngine;
using Watermelon.Map;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Watermelon
{
    public class GameController : MonoBehaviour
    {
        private static GameController gameController;

        [DrawReference]
        [SerializeField] GameData data;

        [LineSpacer]
        [SerializeField] UIController uiController;
        [SerializeField] MapBehavior mapBehavior;
        [SerializeField] MusicSource musicSource;

        private static LevelController levelController;
        private static ParticlesController particlesController;
        private static FloatingTextController floatingTextController;
        private static PUController powerUpController;
        private static TutorialController tutorialController;

        public static GameData Data => gameController.data;

        private static bool isGameActive;
        public static bool IsGameActive => isGameActive;

        private void Awake()
        {
            gameController = this;

            // Cache components
            CacheComponent(out particlesController);
            CacheComponent(out floatingTextController);
            CacheComponent(out levelController);
            CacheComponent(out powerUpController); 
            CacheComponent(out tutorialController);

            musicSource.Init();
            musicSource.Activate();

            uiController.Init();

            particlesController.Init();
            floatingTextController.Init();

            powerUpController.Init();
            levelController.Init();
            tutorialController.Init();

            uiController.InitPages();
        }

        private void Start()
        {
            ITutorial tutorial = TutorialController.GetTutorial(TutorialID.FirstLevel);
            if (data.ShowTutorial && !tutorial.IsFinished)
            {
                // Start first level tutorial
                tutorial.StartTutorial();
            }
            else
            {
                mapBehavior.Show();

                // Display default page
                UIController.ShowPage<UIMainMenu>();

                AdsManager.EnableBanner();

#if UNITY_EDITOR
                CheckIfNeedToAutoRunLevel();
#endif
            }

            GameLoading.MarkAsReadyToHide();
        }

        public static void LoadLevel(int index, SimpleCallback onLevelLoaded = null)
        {
            AdsManager.ShowInterstitial(null);

            gameController.mapBehavior.Hide();

            UIController.HidePage<UIMainMenu>(() =>
            {
                AdsManager.EnableBanner();

                levelController.LoadLevel(index, onLevelLoaded);

                UIController.ShowPage<UIGame>();

                isGameActive = true;
            });
        }

        public static void LoadCustomLevel(LevelData levelData, PreloadedLevelData preloadedLevelData, BackgroundData backgroundData, bool animateDock, SimpleCallback onLevelLoaded = null)
        {
            levelController.LoadCustomLevel(levelData, preloadedLevelData, backgroundData, animateDock, onLevelLoaded);

            UIController.ShowPage<UIGame>();

            isGameActive = true;
        }

        public static void OnLevelCompleted()
        {
            if (!isGameActive)
                return;

            SaveController.Save();

            UIController.HidePage<UIGame>(() =>
            {
                UIController.ShowPage<UIComplete>();
            });

            isGameActive = false;
        }

        public static void OnLevelFailed()
        {
            if (!isGameActive)
                return;

            RaycastController.Disable();

            SaveController.Save();

            UIController.HidePage<UIGame>(() =>
            {
                UIController.ShowPage<UIGameOver>();
            });

            isGameActive = false;
        }

        public static void LoadNextLevel(SimpleCallback onLevelLoaded = null)
        {
            LoadLevel(LevelController.DisplayedLevelIndex, onLevelLoaded);
        }

        public static void ReplayLevel()
        {
            isGameActive = false;

            SaveController.Save();

            UIController.ShowPage<UIMainMenu>();

            LoadLevel(LevelController.DisplayedLevelIndex);
        }

        public static void ReturnToMenu()
        {
            isGameActive = false;

            LevelController.UnloadLevel();

            gameController.mapBehavior.Show();

            AdsManager.ShowInterstitial(null);

            UIController.ShowPage<UIMainMenu>();

            AdsManager.EnableBanner();

            SaveController.Save();
        }

        public static void Revive()
        {
            LevelController.Revive();

            Tween.NextFrame(() =>
            {
                isGameActive = true;
            });
        }

        #region Extensions
        public bool CacheComponent<T>(out T component) where T : Component
        {
            Component unboxedComponent = gameObject.GetComponent(typeof(T));

            if (unboxedComponent != null)
            {
                component = (T)unboxedComponent;

                return true;
            }

            Debug.LogError(string.Format("Scripts Holder doesn't have {0} script added to it", typeof(T)));

            component = null;

            return false;
        }
        #endregion

        #region Dev

#if UNITY_EDITOR

        private static readonly string AUTO_RUN_LEVEL_SAVE_NAME = "auto run level editor";

        public static bool AutoRunLevelInEditor
        {
            get { return EditorPrefs.GetBool(AUTO_RUN_LEVEL_SAVE_NAME, false); }
            set { EditorPrefs.SetBool(AUTO_RUN_LEVEL_SAVE_NAME, value); }
        }

        private void CheckIfNeedToAutoRunLevel()
        {
            if (AutoRunLevelInEditor)
                LoadLevel(LevelController.DisplayedLevelIndex);

            AutoRunLevelInEditor = false;
        }
#endif


        #endregion
    }
}