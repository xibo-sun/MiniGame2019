using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class GateDeselectWindow : Window
{
    GateDeselectionMenu _gateDeselectionMenu;

    public GateDeselectWindow(GateDeselectionMenu gateDeselectionMenu)
    {
        _gateDeselectionMenu = gateDeselectionMenu;
    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("MainGameUI", "GateSelectMenuWindow").asCom;
        contentPane.GetChild("n0").onClick.Add(() =>
        {
            _gateDeselectionMenu.m_Gate = null;
            this.Hide();
        });

        contentPane.GetChild("n3").onClick.Add(()=>
        {
            _gateDeselectionMenu.CancelPlacementButtonClick();
        }
        );
    }

    //public void AdjustPosition(Vector3 point)
    //{
    //    if (contentPane == null)
    //        return;
    //    Debug.Log(contentPane);
    //    contentPane.GetChild("frame").SetXY(point.x, point.y);
    //}
}
