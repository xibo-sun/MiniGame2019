using System.Collections.Generic;
using UnityEngine;
using Core.Game;
using GateGame.Game;
using GateGame.Level;

namespace GateGame.UI
{
    /// <summary>
    /// A manager for the level select user interface
    /// </summary>
    public class LevelSelectScreen : MonoBehaviour
    {
        /// <summary>
        /// The button list which represents the levels
        /// </summary>
        public List<LevelButton> levelButtons;

        /// <summary>
        /// The reference to the list of levels to display
        /// </summary>
        protected LevelList m_LevelList;

        /// <summary>
        /// Instantiate the buttons
        /// </summary>
        protected virtual void Start()
        {
            if (GateGameManager.instance == null)
            {
                return;
            }

            m_LevelList = GateGameManager.instance.levelList;

            int amount = m_LevelList.Count;
            for (int i = 0; i < amount; i++)
            {
                LevelButton button = levelButtons[i];
                button.gameObject.SetActive(false);
                Debug.Log(GateGameManager.instance.IsLevelCompleted(button.level_id));
                if (GateGameManager.instance.IsLevelCompleted(button.level_id))
                {
                    button.gameObject.SetActive(true);
                } else
                {
                    if (i > 0)
                    {
                        LevelButton prev_button = levelButtons[i - 1];
                        if (GateGameManager.instance.IsLevelCompleted(prev_button.level_id))
                            button.gameObject.SetActive(true);
                        else
                            button.gameObject.SetActive(false);
                    }
                    else
                        button.gameObject.SetActive(true);
                }

            }
        }

    }

}
