using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using GateGame.UI.HUD;

public class PauseWindow : Window
{
    private GComponent _mainUI;
    private SceneLoader _sceneLoader;


    PauseMenu _pauseMenu;
    GComponent resumeButton;
    GComponent restartButton;
    GComponent mainMenuButton;

    public PauseWindow(GComponent mainUI, SceneLoader sceneLoader, PauseMenu pauseMenu)
    {
        _mainUI = mainUI;
        _sceneLoader = sceneLoader;
        _pauseMenu = pauseMenu;
    }

    protected override void OnInit()
    {

        this.contentPane = UIPackage.CreateObject("MainGameUI", "PauseMenuWindow").asCom;
        // resume button
        contentPane.GetChild("n3").onClick.Add(()=> { 
            _pauseMenu.ClosePauseMenu();
            _pauseMenu.Unpause();
        });

        // restart button
        contentPane.GetChild("n4").onClick.Add(() => {
            GRoot.inst.CloseAllWindows();
            _pauseMenu.RestartPressed();
            _sceneLoader.RestartCurrentScene();
        });

        // mainMenu button
        contentPane.GetChild("n2").onClick.Add(() => {
            GRoot.inst.CloseAllWindows();
            //for (int i = 0; i < GRoot.inst.numChildren; i++)
            //{
            //    GObject tmp = GRoot.inst.GetChildAt(i);
            //    GRoot.inst.RemoveChild(tmp);
            //    tmp.Dispose();
            //}
            _pauseMenu.LevelSelectPressed();
            _sceneLoader.LoadScene();
        });

        //contentPane.SetXY((int)((_mainUI.width - this.width) / 2), (int)((_mainUI.height - this.height) / 2), true);
    }

}