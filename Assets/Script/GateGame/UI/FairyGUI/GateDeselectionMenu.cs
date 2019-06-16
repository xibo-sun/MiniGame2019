using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using GateGame.Gates;
using GateGame.UI.HUD;

public class GateDeselectionMenu : MonoBehaviour
{
    private GComponent mainUI;
    public GateDeselectWindow gateDeselectWindow;


    /// <summary>
    /// The current gate
    /// </summary>
    public Gate m_Gate;

    /// <summary>
    /// The Gate is selected in first time.
    /// </summary>
    public bool isFirstGateSelected;

    /// <summary>
    /// The main game camera
    /// </summary>
    protected Camera m_GameCamera;


    void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        gateDeselectWindow = new GateDeselectWindow(this);

        m_GameCamera = Camera.main;
        if (GameUI.instanceExists)
        {
            GameUI.instance.selectionChanged += OnUISelectionChanged;
            GameUI.instance.stateChanged += OnGameUIStateChanged;
        }
    }

    /// <summary>
    /// Draws the gate data on to the canvas.
    /// </summary>
    /// <param name="gateToShow">Gate to show.</param>
    public virtual void Show(Gate gateToShow)
    {
        if (gateToShow == null)
        {
            return;
        }
        m_Gate = gateToShow;
        AdjustPosition();

        gateDeselectWindow.Show();
    }

    /// <summary>
    /// Cancel the placement of gates through <see cref="GameUI"/>
    /// </summary>
    public void CancelPlacementButtonClick()
    {
        GameUI.instance.CancelSelectedGate();
    }

    /// <summary>
    /// Get the text attached to the buttons
    /// </summary>
    protected virtual void Awake()
    {
        isFirstGateSelected = false;
    }


    /// <summary>
    /// Adjust the position of the UI
    /// </summary>
    protected void AdjustPosition()
    {
        if (m_Gate == null)
        {
            return;
        }
        Vector3 point = m_GameCamera.WorldToScreenPoint(m_Gate.transform.position);
        //gateDeselectWindow.AdjustPosition(point);
        //gateDeselectWindow.SetXY(point.x,point.y);

    }

    /// <summary>
    /// Fires when gate is selected/deselected
    /// </summary>
    /// <param name="newGate"></param>
    protected virtual void OnUISelectionChanged(Gate newGate)
    {
        if (newGate != null)
        {
            isFirstGateSelected = true;
            Show(newGate);
        }
        else
        {
            gateDeselectWindow.Hide();
        }
    }

    /// <summary>
    /// Fired when the <see cref="GameUI"/> state changes
    /// If the new state is <see cref="GameUI.State.GameOver"/> we need to hide the <see cref="GateUI"/>
    /// </summary>
    /// <param name="oldState">The previous state</param>
    /// <param name="newState">The state to transition to</param>
    protected void OnGameUIStateChanged(GameUI.State oldState, GameUI.State newState)
    {
        if (newState == GameUI.State.GameOver)
        {
            gateDeselectWindow.Hide();
        }
    }


    /// <summary>
    /// Adjust position when the camera moves
    /// </summary>
    protected virtual void Update()
    {
        AdjustPosition();
    }

    /// <summary>
    /// Unsubscribe from GameUI selectionChanged and stateChanged
    /// </summary>
    void OnDestroy()
    {
        if (GameUI.instanceExists)
        {
            GameUI.instance.selectionChanged -= OnUISelectionChanged;
            GameUI.instance.stateChanged -= OnGameUIStateChanged;
        }
    }

}
