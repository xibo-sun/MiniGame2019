using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using GateGame.UI.HUD;
using GateGame.Gates;

public class GateSelectionButton : MonoBehaviour
{
    private GComponent mainUI;

    public Gate m_Gate_And;
    public Gate m_Gate_or;
    public Gate m_Gate_not;

    // Start is called before the first frame update
    void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;

        mainUI.GetChild("n5").asCom.onClick.Add(() => { OnButtonTapped(m_Gate_And); });
        mainUI.GetChild("n6").asCom.onClick.Add(() => { OnButtonTapped(m_Gate_or); });
        mainUI.GetChild("n7").asCom.onClick.Add(() => { OnButtonTapped(m_Gate_not); });
    }

    void OnButtonTapped(Gate gateData)
    {
        var gameUI = GameUI.instance;
        if (gameUI.isBuilding)
        {
            gameUI.CancelGhostPlacement();
        }
        gameUI.SetToBuildMode(gateData);
    }
}
