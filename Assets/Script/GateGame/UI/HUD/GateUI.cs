using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GateGame.Gates;


namespace GateGame.UI.HUD
{

    /// <summary>
    /// Controls the UI objects that draw the gate data
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class GateUI : MonoBehaviour
    {
        /// <summary>
        /// The text object for the name
        /// </summary>
        public Text gateName;

        /// <summary>
        /// The text object for the description
        /// </summary>
        public Text description;

        /// <summary>
        /// The attached cancel placement button
        /// </summary>
        public Button cancelPlacementButton;

        /// <summary>
        /// The confirmation button.
        /// </summary>
        public Button confirmationButton;

        /// <summary>
        /// The main game camera
        /// </summary>
        protected Camera m_GameCamera;


        /// <summary>
        /// The current gate
        /// </summary>
        protected Gate m_Gate;

        /// <summary>
        /// The panel rect transform.
        /// </summary>
        public RectTransform panelRectTransform;

        /// <summary>
        /// The canvas attached to the gameObject
        /// </summary>
        protected Canvas m_Canvas;



        /// <summary>
        /// The Gate is selected in first time.
        /// </summary>
        public bool isFirstGateSelected;

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

            m_Canvas.enabled = true;

            if (cancelPlacementButton != null)
            {
                cancelPlacementButton.gameObject.SetActive(true);
                confirmationButton.gameObject.SetActive(false);
            }
            UpdateInfo(gateToShow);
        }

        /// <summary>
        /// Hides the gate info UI
        /// </summary>
        public virtual void Hide()
        {
            m_Gate = null;
            m_Canvas.enabled = false;
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
            m_Canvas = GetComponent<Canvas>();
            isFirstGateSelected = false;
        }

        private void UpdateInfo(Gate gateToShow)
        {
            if (gateToShow.gateType == Gate.GateType.And)
            {
                gateName.text = "与门";
                description.text = "2输入\n输出=输入相与";
            }else if (gateToShow.gateType == Gate.GateType.Or)
            {
                gateName.text = "或门";
                description.text = "2输入\n输出=输入相或";
            } else if (gateToShow.gateType == Gate.GateType.Not)
            {
                gateName.text = "非门";
                description.text = "1输入\n输出=输入取反";
            }
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
            point.z = 0;
            panelRectTransform.transform.position = point;
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
                Hide();
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
                Hide();
            }
        }

        /// <summary>
        /// Subscribe to mouse button action
        /// </summary>
        protected virtual void Start()
        {
            m_GameCamera = Camera.main;
            m_Canvas.enabled = false;
            if (GameUI.instanceExists)
            {
                GameUI.instance.selectionChanged += OnUISelectionChanged;
                GameUI.instance.stateChanged += OnGameUIStateChanged;
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
}


