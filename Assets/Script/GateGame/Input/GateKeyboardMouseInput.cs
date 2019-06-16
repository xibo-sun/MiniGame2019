using Core.Input;
using GateGame.UI.HUD;
using UnityEngine;
using UnityInput = UnityEngine.Input;
using State = GateGame.UI.HUD.GameUI.State;

namespace GateGame.Input
{
    public class GateKeyboardMouseInput : KeyboardMouseInput
    {

        /// <summary>
        /// Cached eference to gameUI
        /// </summary>
        GameUI m_GameUI;

        // Register input events
        protected override void OnEnable()
        {
            //base.OnEnable();

            m_GameUI = GetComponent<GameUI>();

            if (InputController.instanceExists)
            {
                InputController controller = InputController.instance;

                controller.tapped += OnTap;
                controller.mouseMoved += OnMouseMoved;
            }
        }

        /// Deregister input events
        protected override void OnDisable()
        {
            if (!InputController.instanceExists)
            {
                return;
            }

            InputController controller = InputController.instance;

            controller.tapped -= OnTap;
            controller.mouseMoved -= OnMouseMoved;
        }

        void OnMouseMoved(PointerInfo pointer)
        {
            // We only respond to mouse info
            var mouseInfo = pointer as MouseCursorInfo;

            if ((mouseInfo != null) && (m_GameUI.isBuilding))
            {
                m_GameUI.TryMoveGhost(pointer, false);
            }
        }


        void OnTap(PointerActionInfo pointer)
        {
            // We only respond to mouse info
            var mouseInfo = pointer as MouseButtonInfo;

            if (mouseInfo != null && !mouseInfo.startedOverUI)
            {
                if (m_GameUI.isBuilding)
                {
                    if (mouseInfo.mouseButtonId == 0) // LMB confirms
                    {
                        m_GameUI.TryPlaceGate(pointer);
                    }
                    else // RMB cancels
                    {
                        m_GameUI.CancelGhostPlacement();
                    }
                }
                else
                {
                    if (mouseInfo.mouseButtonId == 0)
                    {
                        // select towers
                        m_GameUI.TrySelectGate(pointer);
                    }
                }
            }
        }


        /// <summary>
        /// Handle camera panning behaviour
        /// </summary>
        protected override void Update()
        {
            base.Update();

            // Escape handling
            if (UnityInput.GetKeyDown(KeyCode.Escape))
            {
                switch (m_GameUI.state)
                {
                    case State.Normal:
                        if (m_GameUI.isGateSelected)
                        {
                            m_GameUI.DeselectGate();
                        }
                        else
                        {
                            m_GameUI.Pause();

                        }
                        break;
                    case State.BuildingWithDrag:
                    case State.Building:
                        m_GameUI.CancelGhostPlacement();
                        break;
                }
            }
        }

    }

}

