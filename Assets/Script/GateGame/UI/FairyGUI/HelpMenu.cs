using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class HelpMenu : MonoBehaviour
{

    private GComponent mainUI;
    public HelpWindow helpWindow;

    void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        helpWindow = new HelpWindow();
        mainUI.GetChild("n1").onClick.Add(()=> { helpWindow.Show();});
    }
}
