using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using GateGame.UI.HUD;

public class PauseMenuGUI : MonoBehaviour
{
    private GComponent mainUI;
    public PauseWindow pauseWindow;

    public GButton pauseButton;

    SceneLoader sceneLoader;
    PauseMenu pauseMenu;
    HelpWindow helpWindow;

    void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        sceneLoader = GetComponent<SceneLoader>();
        pauseMenu = GetComponent<PauseMenu>();
        helpWindow = GetComponent<HelpMenu>().helpWindow;
        pauseWindow = new PauseWindow(mainUI, sceneLoader, pauseMenu);
        pauseButton = mainUI.GetChild("n2").asButton;
        pauseButton.onClick.Add(() => {
            pauseMenu.Pause();
            pauseWindow.Show();
        }); 
    }


}
