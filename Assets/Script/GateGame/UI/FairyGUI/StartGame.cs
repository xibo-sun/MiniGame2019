using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using GateGame.UI.HUD;

namespace GateGame.UI.FairyGUI
{


    public class StartGame : MonoBehaviour
    {

        private GComponent mainUI;
        private SceneLoader sceneLoader;
        // Start is called before the first frame update
        void Start()
        {
            mainUI = GetComponent<UIPanel>().ui;
            sceneLoader = GetComponent<SceneLoader>();
            mainUI.GetChild("n9").asCom.onClick.Add(()=> { sceneLoader.LoadScene(); });

        }
    }
}


