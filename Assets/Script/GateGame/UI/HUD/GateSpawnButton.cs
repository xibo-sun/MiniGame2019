using System;
using GateGame.Gates;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GateGame.UI.HUD
{
    /// <summary>
    /// A button controller for spawning gates
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class GateSpawnButton : MonoBehaviour
    {
        public Gate m_Gate;

        public event Action<Gate> buttonTapped;
        public event Action<Gate> draggedOff;

        /// <summary>
        /// The attached rect transform
        /// </summary>
        RectTransform m_RectTransform;

        public bool buttonClicked  { get; private set; }


        /// <summary>
        /// Checks if the pointer is out of bounds
        /// and then fires the draggedOff event
        /// </summary>
        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(m_RectTransform, eventData.position))
            {
                if (draggedOff != null)
                {
                    draggedOff(m_Gate);
                }
            }
        }


        /// <summary>
        /// Cache the rect transform
        /// </summary>
        protected virtual void Awake()
        {
            buttonClicked = false;
            m_RectTransform = (RectTransform)transform;
        }

        /// <summary>
        /// Initialize the gate spawn button
        /// </summary>
        protected virtual void Start()
        {
            this.buttonTapped += OnButtonTapped;
            this.draggedOff += OnButtonDraggedOff;
        }


        /// <summary>
        /// The click for when the button is tapped
        /// </summary>
        public void OnClick()
        {
            buttonClicked = true;
            if (buttonTapped != null)
            {
                buttonTapped(m_Gate);
            }
        }

        /// <summary>
        /// Sets the GameUI to build Gate
        /// </summary>
        void OnButtonTapped(Gate gateData)
        {
            var gameUI = GameUI.instance;
            if (gameUI.isBuilding)
            {
                gameUI.CancelGhostPlacement();
            }
            gameUI.SetToBuildMode(gateData);
        }

        /// <summary>
        /// Sets the GameUI to build Gate
        /// </summary>
        void OnButtonDraggedOff(Gate gateData)
        {
            if (!GameUI.instance.isBuilding)
            {
                GameUI.instance.SetToDragMode(gateData);
            }
        }

        /// <summary>
        /// Unsubscribes from all the gate spawn buttons
        /// </summary>
        void OnDestroy()
        {
            this.buttonTapped -= OnButtonTapped;
            this.draggedOff -= OnButtonDraggedOff;
        }

    }
}


