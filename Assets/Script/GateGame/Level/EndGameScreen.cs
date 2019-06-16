using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GateGame.Game;
using Core.Game;
using GateGame.UI.HUD;

namespace GateGame.Level
{ 
    /// <summary>
    /// UI to display the game over screen
    /// </summary>
    public class EndGameScreen : MonoBehaviour
    {
        /// <summary>
        /// AudioClip to play when victorious
        /// </summary>
        public AudioClip victorySound;

        /// <summary>
        /// AudioClip to play when failed
        /// </summary>
        public AudioClip defeatSound;

        /// <summary>
        /// AudioSource that plays the sound
        /// </summary>
        public AudioSource audioSource;

        /// <summary>
        /// The containing panel of the End Game UI
        /// </summary>
        public Canvas endGameCanvas;

        /// <summary>
        /// Reference to the Text object that displays the result message
        /// </summary>
        public Text endGameMessageText;

        /// <summary>
        /// Name of level select screen
        /// </summary>
        public string menuSceneName = "LevelSelect";

        /// <summary>
        /// Text to be displayed on popup
        /// </summary>
        public string levelCompleteText = "{0} COMPLETE!";

        public string levelFailedText = "{0} FAILED!";


        /// <summary>
        /// Background image
        /// </summary>
        public Image background;

        /// <summary>
        /// Color to set background
        /// </summary>
        public Color winBackgroundColor;

        public Color loseBackgroundColor;


        /// <summary>
        /// The Canvas that holds the button to go to the next level
        /// if the player has beaten the level
        /// </summary>
        public Button nextLevelButton;


        /// <summary>
        /// Reference to the <see cref="LevelManager" />
        /// </summary>
        protected LevelManager m_LevelManager;

        /// <summary>
        /// Safely unsubscribes from <see cref="LevelManager" /> events.
        /// Go back to the main menu scene
        /// </summary>
        public void GoToMainMenu()
        {
            SafelyUnsubscribe();
            SceneManager.LoadScene(menuSceneName);
        }

        /// <summary>
        /// Safely unsubscribes from <see cref="LevelManager" /> events.
        /// Reloads the active scene
        /// </summary>
        public void RestartLevel()
        {
            SafelyUnsubscribe();
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }


        /// <summary>
        /// Safely unsubscribes from <see cref="LevelManager" /> events.
        /// Goes to the next scene if valid
        /// </summary>
        public void GoToNextLevel()
        {
            SafelyUnsubscribe();
            if (!GateGameManager.instanceExists)
            {
                return;
            }
            GateGameManager gm = GateGameManager.instance;
            LevelItem item = gm.GetLevelForCurrentScene();
            LevelList list = gm.levelList;
            int levelCount = list.Count;
            int index = -1;
            for (int i = 0; i < levelCount; i++)
            {
                if (item == list[i])
                {
                    index = i + 1;
                    break;
                }
            }
            if (index < 0 || index >= levelCount)
            {
                return;
            }
            LevelItem nextLevel = gm.levelList[index];
            SceneManager.LoadScene(nextLevel.sceneName);
        }

        /// <summary>
        /// Hide the panel if it is active at the start.
        /// Subscribe to the <see cref="LevelManager" /> completed/failed events.
        /// </summary>
        protected void Start()
        {
            LazyLoad();
            endGameCanvas.enabled = false;
            nextLevelButton.enabled = false;
            nextLevelButton.gameObject.SetActive(false);

            m_LevelManager.levelCompleted += Victory;
            m_LevelManager.levelFailed += Defeat;
        }


        /// <summary>
        /// Shows the end game screen
        /// </summary>
        protected void OpenEndGameScreen(string endResultText)
        {
            LevelItem level = GateGameManager.instance.GetLevelForCurrentScene();
            endGameCanvas.enabled = true;

            if (level != null)
            {
                endGameMessageText.text = string.Format(endResultText, level.name.ToUpper());
                GateGameManager.instance.CompleteLevel(level.id);
            }
            else
            {
                // If the level is not in LevelList, we should just use the name of the scene. This will not store the level's score.
                string levelName = SceneManager.GetActiveScene().name;
                endGameMessageText.text = string.Format(endResultText, levelName.ToUpper());
            }


            if (!GameUI.instanceExists)
            {
                return;
            }
            if (GameUI.instance.state == GameUI.State.Building)
            {
                GameUI.instance.CancelGhostPlacement();
            }
            GameUI.instance.GameOver();
        }


        /// <summary>
        /// Ensure that <see cref="LevelManager" /> events are unsubscribed from when necessary
        /// </summary>
        protected void SafelyUnsubscribe()
        {
            LazyLoad();
            m_LevelManager.levelCompleted -= Victory;
            m_LevelManager.levelFailed -= Defeat;
        }

        /// <summary>
        /// Ensure <see cref="m_LevelManager" /> is not null
        /// </summary>
        protected void LazyLoad()
        {
            if ((m_LevelManager == null) && LevelManager.instanceExists)
            {
                m_LevelManager = LevelManager.instance;
            }
        }

        /// <summary>
        /// Occurs when the level is sucessfully completed
        /// </summary>
        protected void Victory()
        {
            OpenEndGameScreen(levelCompleteText);
            if ((victorySound != null) && (audioSource != null))
            {
                audioSource.PlayOneShot(victorySound);
            }
            background.color = winBackgroundColor;

            //first check if there are any more levels after this one
            if (nextLevelButton == null || !GateGameManager.instanceExists)
            {
                return;
            }
            GateGameManager gm = GateGameManager.instance;
            LevelItem item = gm.GetLevelForCurrentScene();
            LevelList list = gm.levelList;
            int levelCount = list.Count;
            int index = -1;
            for (int i = 0; i < levelCount; i++)
            {
                if (item == list[i])
                {
                    index = i;
                    break;
                }
            }
            //if the level does not exist or this is the last level
            //hide the next level button
            if (index < 0 || index == levelCount - 1)
            {
                nextLevelButton.enabled = false;
                nextLevelButton.gameObject.SetActive(false);
                return;
            }
            nextLevelButton.enabled = true;
            nextLevelButton.gameObject.SetActive(true);
        }


        /// <summary>
        /// Occurs when level is failed
        /// </summary>
        protected void Defeat()
        {
            OpenEndGameScreen(levelFailedText);
            if (nextLevelButton != null)
            {
                nextLevelButton.enabled = false;
                nextLevelButton.gameObject.SetActive(false);
            }
            if ((defeatSound != null) && (audioSource != null))
            {
                audioSource.PlayOneShot(defeatSound);
            }
            background.color = loseBackgroundColor;
        }


        /// <summary>
        /// Safely unsubscribes from <see cref="LevelManager" /> events.
        /// </summary>
        protected void OnDestroy()
        {
            SafelyUnsubscribe();
            if (GameUI.instanceExists)
            {
                GameUI.instance.Unpause();
            }
        }

    }
}


