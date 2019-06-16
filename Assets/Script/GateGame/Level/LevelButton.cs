using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GateGame.Game;
using FairyGUI;

namespace GateGame.Level
{
    /// <summary>
    /// The button for selecting a level
    /// </summary>
    //[RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        /// <summary>
        /// Reference to the required button component
        /// </summary>
        protected GButton m_Button;

        public string level_id;

        public LevelButton(GButton levelButton)
        {
            m_Button = levelButton;
        }

        /// <summary>
        /// When the user clicks the button, change the scene
        /// </summary>
        public void ButtonClicked()
        {
            ChangeScenes();
        }

        /// <summary>
        /// Changes the scene to the scene name provided by m_Item
        /// </summary>
        protected void ChangeScenes()
        {
            SceneManager.LoadScene(GateGameManager.instance.levelList[level_id].sceneName);
        }

        public void register()
        {
            m_Button.onClick.Add(()=> { ButtonClicked(); });
        }

    }
}


