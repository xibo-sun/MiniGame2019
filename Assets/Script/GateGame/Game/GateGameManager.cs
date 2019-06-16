using Core.Data;
using Core.Game;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GateGame.Game
{
    /// <summary>
    /// Game Manager - a persistent single that handles persistence, and level lists, etc.
    /// This should be initialized when the game starts.
    /// </summary>
    public class GateGameManager : GameManagerBase<GateGameManager, GameDataStore>
    {
        /// <summary>
        /// Scriptable object for list of levels
        /// </summary>
        public LevelList levelList;

        /// <summary>
        /// Set sleep timeout to never sleep
        /// </summary>
        protected override void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            base.Awake();
        }

        /// <summary>
        /// Method used for completing the level
        /// </summary>
        /// <param name="levelId">The levelId to mark as complete</param>
        public void CompleteLevel(string levelId)
        {
            Debug.Log("CompleteLevelId:" + levelId);
            if (!levelList.ContainsKey(levelId))
            {
                Debug.LogWarningFormat("[GAME] Cannot complete level with id = {0}. Not in level list", levelId);
                return;
            }

            m_DataStore.CompleteLevel(levelId);
            SaveData();
        }

        /// <summary>
        /// Gets the id for the current level
        /// </summary>
        public LevelItem GetLevelForCurrentScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            return levelList.GetLevelByScene(sceneName);
        }

        protected override void Start()
        {
            base.Start();
            Screen.SetResolution(1280, 960, false);
        }

        /// <summary>
        /// Determines if a specific level is completed
        /// </summary>
        /// <param name="levelId">The level ID to check</param>
        /// <returns>true if the level is completed</returns>
        public bool IsLevelCompleted(string levelId)
        {
            if (!levelList.ContainsKey(levelId))
            {
                Debug.LogWarningFormat("[GAME] Cannot check if level with id = {0} is completed. Not in level list", levelId);
                return false;
            }

            return m_DataStore.IsLevelCompleted(levelId);
        }

    }
}

