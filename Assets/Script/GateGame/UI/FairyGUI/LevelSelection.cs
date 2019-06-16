using System.Collections.Generic;
using UnityEngine;
using Core.Game;
using GateGame.Game;
using FairyGUI;
using GateGame.UI.HUD;
using GateGame.Level;

public class LevelSelection : MonoBehaviour
{
    /// <summary>
    /// The button list which represents the levels
    /// </summary>
    protected List<GGroup> levelButtonGroups = new List<GGroup>();
    protected List<LevelButton> levelButtons = new List<LevelButton>();

    /// <summary>
    /// The reference to the list of levels to display
    /// </summary>
    protected LevelList m_LevelList;

    private GComponent mainUI;
    private SceneLoader sceneLoader;

    /// <summary>
    /// Instantiate the buttons
    /// </summary>
    protected virtual void Start()
    {

        mainUI = GetComponent<UIPanel>().ui;
        sceneLoader = GetComponent<SceneLoader>();


        if (GateGameManager.instance == null)
        {
            return;
        }

        m_LevelList = GateGameManager.instance.levelList;

        


        int amount = m_LevelList.Count;
        for (int i = 0; i < amount; i++)
        {
            GGroup i_group = mainUI.GetChild("level" + i).asGroup;
            i_group.visible = false;
            levelButtonGroups.Add(i_group);
            LevelButton levelButton = new LevelButton(mainUI.GetChild("l" + i).asButton);
            levelButton.level_id = i.ToString();
            levelButtons.Add(levelButton);

            if (GateGameManager.instance.IsLevelCompleted(i.ToString()))
            {
                Debug.Log(i);
                i_group.visible = true;
            }
            else
            {
                if (i > 0)
                {
                    if (GateGameManager.instance.IsLevelCompleted((i - 1).ToString()))
                        i_group.visible = true;
                }
                else
                    i_group.visible = true;
            }
        }


        for (int i = 0; i < amount; i++)
        {
            levelButtons[i].register();
        }

    }
}
