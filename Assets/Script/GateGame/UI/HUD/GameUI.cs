using System;
using Core.Input;
using Core.Health;
using Core.Utilities;
using GateGame.Gates;
using GateGame.Gates.Placement;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;


namespace GateGame.UI.HUD
{
    /// <summary>
    /// A game UI wrapper for a pointer that also contains raycast information
    /// </summary>
    public struct UIPointer
    {
        /// <summary>
        /// The pointer info
        /// </summary>
        public PointerInfo pointer;

        /// <summary>
        /// The ray for this pointer
        /// </summary>
        public Ray ray;

        /// <summary>
        /// The raycast hit object into the 3D scene
        /// </summary>
        public RaycastHit? raycast;

        /// <summary>
        /// True if this pointer started over a UI element or anything the event system catches
        /// </summary>
        public bool overUI;
    }



    /// <summary>
    /// An object that manages user interaction with the game.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class GameUI : Singleton<GameUI>
    {

        /// <summary>
        /// The states the UI can be in
        /// </summary>
        public enum State
        {
            /// <summary>
            /// The game is in its normal state. Here the player can pan the camera, select units and gates
            /// </summary>
            Normal,

            /// <summary>
            /// The game is in 'build mode'. Here the player can pan the camera, confirm or deny placement
            /// </summary>
            Building,

            /// <summary>
            /// The game is Paused. Here, the player can restart the level, or quit to the main menu
            /// </summary>
            Paused,

            /// <summary>
            /// The game is over and the level was failed/completed
            /// </summary>
            GameOver,
            
            /// <summary>
            /// The game is in 'build mode' and the player is dragging the ghost gate
            /// </summary>
            BuildingWithDrag
        }

        #region variables
        /// <summary>
        /// Gets the current UI state
        /// </summary>
        public State state { get; private set; }

        /// <summary>
        /// The currently selected gate
        /// </summary>
        public LayerMask placementAreaMask;

        /// <summary>
        /// The layer for gate selection
        /// </summary>
        public LayerMask gateSelectionLayer;

        /// <summary>
        /// The physics layer for moving the ghost around the world
        /// when the placement is not valid
        /// </summary>
        public LayerMask ghostWorldPlacementMask;

        /// <summary>
        /// The radius of the sphere cast 
        /// for checking ghost placement
        /// </summary>
        public float sphereCastRadius = 1;


        /// <summary>
        /// Fires when the <see cref="State"/> changes
        /// should only allow firing when TouchUI is used
        /// </summary>
        public event Action<State, State> stateChanged;

        /// <summary>
        /// Fires off when the ghost was previously not valid but now is due to currency amount change
        /// </summary>
        public event Action ghostBecameValid;


        /// <summary>
        /// Fires when a gate is selected/deselected
        /// </summary>
        public event Action<Gate> selectionChanged;


        /// <summary>
        /// Gets the current selected tower
        /// </summary>
        public Gate currentSelectedGate { get; private set; }

        /// <summary>
        /// Grid position ghost gate in on
        /// </summary>
        IntVector2 m_GridPosition;


        /// <summary>
        /// Our cached camera reference
        /// </summary>
        Camera m_Camera;


        /// <summary>
        /// Tracks if the ghost is in a valid location and the player can afford it
        /// </summary>
        bool m_GhostPlacementPossible;



        /// <summary>
        /// Current Gate placeholder. Will be null if not in the <see cref="State.Building" /> state.
        /// </summary>
        GatePlacementGhost m_CurrentGate;


        /// <summary>
        /// Placement area ghost gate is currently on
        /// </summary>
        IPlacementArea m_CurrentArea;

        #endregion

        #region Basic fuctions
        /// <summary>
        /// Gets whether a gate has been selected
        /// </summary>
        public bool isGateSelected
        {
            get { return currentSelectedGate != null; }
        }


        /// <summary>
        /// Creates a new UIPointer holding data object for the given pointer position
        /// </summary>
        protected UIPointer WrapPointer(PointerInfo pointerInfo)
        {
            return new UIPointer
            {
                overUI = IsOverUI(pointerInfo),
                pointer = pointerInfo,
                ray = m_Camera.ScreenPointToRay(pointerInfo.currentPosition)
            };
        }

        /// <summary>
        /// Checks whether a given pointer is over any UI
        /// </summary>
        /// <param name="pointerInfo">The pointer to test</param>
        /// <returns>True if the event system reports this pointer being over UI</returns>
        protected bool IsOverUI(PointerInfo pointerInfo)
        {
            int pointerId;
            EventSystem currentEventSystem = EventSystem.current;

            // Pointer id is negative for mouse, positive for touch
            var cursorInfo = pointerInfo as MouseCursorInfo;
            var mbInfo = pointerInfo as MouseButtonInfo;
            var touchInfo = pointerInfo as TouchInfo;

            if (cursorInfo != null)
            {
                pointerId = PointerInputModule.kMouseLeftId;
            }
            else if (mbInfo != null)
            {
                // LMB is 0, but kMouseLeftID = -1;
                pointerId = -mbInfo.mouseButtonId - 1;
            }
            else if (touchInfo != null)
            {
                pointerId = touchInfo.touchId;
            }
            else
            {
                throw new ArgumentException("Passed pointerInfo is not a TouchInfo or MouseCursorInfo", "pointerInfo");
            }

            return currentEventSystem.IsPointerOverGameObject(pointerId);
        }


        #endregion


        #region State Change Functions

        /// <summary>
        /// Gets whether certain build operations are valid
        /// </summary>
        public bool isBuilding
        {
            get
            {
                return state == State.Building || state == State.BuildingWithDrag;
            }
        }



        /// <summary>
        /// Returns the GameUI to dragging mode with the curent gate
        /// </summary>
        public void ChangeToDragMode()
        {
            if (!isBuilding)
            {
                throw new InvalidOperationException("Trying to return to Build With Dragging Mode when not in Build Mode");
            }
            SetState(State.BuildingWithDrag);
        }


        /// <summary>
        /// Changes the state and fires
        /// </summary>
        void SetState(State newState)
        {
            if (state == newState)
            {
                return;
            }
            State oldState = state;
            if (oldState == State.Paused || oldState == State.GameOver)
            {
                Time.timeScale = 1f;
            }

            switch (newState)
            {
                case State.Normal:
                    break;
                case State.Building:
                    break;
                case State.BuildingWithDrag:
                    break;
                case State.Paused:
                case State.GameOver:
                    if (oldState == State.Building)
                    {
                        CancelGhostPlacement();
                    }
                    Time.timeScale = 0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("newState", newState, null);
            }
            state = newState;
            if (stateChanged != null)
            {
                stateChanged(oldState, state);
            }
        }


        /// <summary>
        /// Called when the game is over
        /// </summary>
        public void GameOver()
        {
            SetState(State.GameOver);
        }

        /// <summary>
        /// Pause the game and display the pause menu
        /// </summary>
        public void Pause()
        {
            SetState(State.Paused);
        }

        /// <summary>
        /// Resume the game and close the pause menu
        /// </summary>
        public void Unpause()
        {
            SetState(State.Normal);
        }


        /// <summary>
        /// Changes the mode to drag
        /// </summary>
        public void SetToDragMode([NotNull] Gate gateToBuild)
        {
            if (state != State.Normal)
            {
                throw new InvalidOperationException("Trying to enter drag mode when not in Normal mode");
            }

            if (m_CurrentGate != null)
            {
                // Destroy current ghost
                CancelGhostPlacement();
            }
            SetUpGhostGate(gateToBuild);
            SetState(State.BuildingWithDrag);
        }


        /// <summary>
        /// Sets the UI into a build state for a given gate
        /// </summary>
        /// <param name="gateToBuild">
        /// The gate to build
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Throws exception trying to enter Build Mode when not in Normal Mode
        /// </exception>
        public void SetToBuildMode([NotNull] Gate gateToBuild)
        {
            if (state != State.Normal)
            {
                throw new InvalidOperationException("Trying to enter Build mode when not in Normal mode");
            }

            if (m_CurrentGate != null)
            {
                // Destroy current ghost
                CancelGhostPlacement();
            }
            SetUpGhostGate (gateToBuild);
            SetState(State.Building);
        }

        #endregion



        #region Ghost Building Functions
        /// <summary>
        /// Position the ghost gate at the given pointer
        /// </summary>
        public void TryMoveGhost(PointerInfo pointerInfo, bool hideWhenInvalid = true)
        {
            if (m_CurrentGate == null)
            {
                throw new InvalidOperationException("Trying to move the gate ghost when we don't have one");
            }

            UIPointer pointer = WrapPointer(pointerInfo);
            // Do nothing if we're over UI
            if (pointer.overUI && hideWhenInvalid)
            {
                m_CurrentGate.Hide();
                return;
            }
            MoveGhost(pointer, hideWhenInvalid);
        }

        /// <summary>
        /// Move the ghost to the pointer's position
        /// </summary>
        /// <param name="pointer">The pointer to place the ghost at</param>
        /// <param name="hideWhenInvalid">Optional parameter for whether the ghost should be hidden or not</param>
        /// <exception cref="InvalidOperationException">If we're not in the correct state</exception>
        protected void MoveGhost(UIPointer pointer, bool hideWhenInvalid = true)
        {
            if (m_CurrentGate == null || !isBuilding)
            {
                throw new InvalidOperationException(
                    "Trying to position a tower ghost while the UI is not currently in the building state.");
            }

            // Raycast onto placement layer
            PlacementAreaRaycast(ref pointer);

            if (pointer.raycast != null)
            {
                MoveGhostWithRaycastHit(pointer.raycast.Value);
            }
            else
            {
                MoveGhostOntoWorld(pointer.ray, hideWhenInvalid);
            }
        }


        /// <summary>
        /// Raycast onto tower placement areas
        /// </summary>
        /// <param name="pointer">The pointer we're testing</param>
        protected void PlacementAreaRaycast(ref UIPointer pointer)
        {
            pointer.raycast = null;

            if (pointer.overUI)
            {
                // Pointer is over UI, so no valid position
                return;
            }

            // Raycast onto placement area layer
            RaycastHit hit;
            if (Physics.Raycast(pointer.ray, out hit, float.MaxValue, placementAreaMask))
            {
                pointer.raycast = hit;
            }
        }

        /// <summary>
        /// Move ghost with successful raycastHit onto m_PlacementAreaMask
        /// </summary>
        protected virtual void MoveGhostWithRaycastHit(RaycastHit raycast)
        {
            // We successfully hit one of our placement areas
            // Try and get a placement area on the object we hit
            m_CurrentArea = raycast.collider.GetComponent<IPlacementArea>();

            if (m_CurrentArea == null)
            {
                Debug.LogError("There is not an IPlacementArea attached to the collider found on the m_PlacementAreaMask");
                return;
            }
            m_GridPosition = m_CurrentArea.WorldToGrid(raycast.point, m_CurrentGate.controller.dimensions);
            GateFitStatus fits = m_CurrentArea.Fits(m_GridPosition, m_CurrentGate.controller.dimensions);

            m_CurrentGate.Show();
            m_GhostPlacementPossible = fits == GateFitStatus.Fits;
            m_CurrentGate.Move(m_CurrentArea.GridToWorld(m_GridPosition, m_CurrentGate.controller.dimensions),
                                m_CurrentArea.transform.rotation,
                                m_GhostPlacementPossible);
        }


        /// <summary>
        /// Move ghost with the given ray
        /// </summary>
        protected virtual void MoveGhostOntoWorld(Ray ray, bool hideWhenInvalid)
        {
            m_CurrentArea = null;

            if (!hideWhenInvalid)
            {
                RaycastHit hit;
                // check against all layers that the ghost can be on
                Physics.SphereCast(ray, sphereCastRadius, out hit, float.MaxValue, ghostWorldPlacementMask);
                if (hit.collider == null)
                {

                    return;
                }
                m_CurrentGate.Show();
                m_CurrentGate.Move(hit.point, hit.collider.transform.rotation, false);
            }
            else
            {
                m_CurrentGate.Hide();
            }
        }


        /// <summary>
        /// Cancel placing the ghost
        /// </summary>
        public void CancelGhostPlacement()
        {
            if (!isBuilding)
            {
                throw new InvalidOperationException("Can't cancel out of ghost placement when not in the building state.");
            }

            //if (buildInfoUI != null)
            //{
            //    buildInfoUI.Hide();
            //}
            Destroy(m_CurrentGate.gameObject);
            m_CurrentGate = null;
            SetState(State.Normal);
            DeselectGate();
        }

        ///// <summary>
        ///// Closes the Tower UI on death of tower
        ///// </summary>
        //protected void OnGateDied(DamageableBehaviour targetable)
        //{
        //    //towerUI.enabled = false;
        //    //radiusVisualizerController.HideRadiusVisualizers();
        //    DeselectGate();
        //}

        /// <summary>
        /// Creates and hides the tower and shows the buildInfoUI
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Throws exception if the <paramref name="gateToBuild"/> is null
        /// </exception>
        void SetUpGhostGate([NotNull] Gate gateToBuild)
        {
            if (gateToBuild == null)
            {
                throw new ArgumentNullException("gateToBuild");
            }

            m_CurrentGate = Instantiate(gateToBuild.gateGhostPrefab);
            m_CurrentGate.Initialize(gateToBuild);
            m_CurrentGate.Hide();

            //activate build info
            //if (buildInfoUI != null)
            //{
            //    buildInfoUI.Show(gateToBuild);
            //}
        }


        #endregion


        #region Gate Building Functions
        /// <summary>
        /// Attempt to position a gate at the given location
        /// </summary>
        /// <param name="pointerInfo">The pointer we're using to position the gate</param>
        public void TryPlaceGate(PointerInfo pointerInfo)
        {
            UIPointer pointer = WrapPointer(pointerInfo);

            // Do nothing if we're over UI
            if (pointer.overUI)
            {
                return;
            }
            BuildGate(pointer);
        }

        /// <summary>
        /// Used to build the gate during the build phase
        /// Checks currency and calls <see cref="PlaceGhost" />
        /// <exception cref="InvalidOperationException">
        /// Throws exception when not in a build mode or when gate is not a valid position
        /// </exception>
        /// </summary>
        public void BuildGate(UIPointer pointer)
        {
            if (!isBuilding)
            {
                throw new InvalidOperationException("Trying to buy towers when not in a Build Mode");
            }
            if (m_CurrentGate == null || !IsGhostAtValidPosition())
            {
                return;
            }
            PlacementAreaRaycast(ref pointer);
            if (!pointer.raycast.HasValue || pointer.raycast.Value.collider == null)
            {
                CancelGhostPlacement();
                return;
            }
            PlaceGhost(pointer);
        }


        /// <summary>
        /// Checks the position of the <see cref="m_CurrentGate"/> 
        /// on the <see cref="m_CurrentArea"/>
        /// </summary>
        /// <returns>
        /// True if the placement is valid
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throws exception if the check is done in <see cref="State.Normal"/> state
        /// </exception>
        public bool IsGhostAtValidPosition()
        {
            if (!isBuilding)
            {
                throw new InvalidOperationException("Trying to check ghost position when not in a build mode");
            }
            if (m_CurrentGate == null)
            {
                return false;
            }
            if (m_CurrentArea == null)
            {
                return false;
            }
            GateFitStatus fits = m_CurrentArea.Fits(m_GridPosition, m_CurrentGate.controller.dimensions);
            return fits == GateFitStatus.Fits;
        }

        /// <summary>
        /// Place the ghost at the pointer's position
        /// </summary>
        /// <param name="pointer">The pointer to place the ghost at</param>
        /// <exception cref="InvalidOperationException">If we're not in the correct state</exception>
        protected void PlaceGhost(UIPointer pointer)
        {
            if (m_CurrentGate == null || !isBuilding)
            {
                throw new InvalidOperationException(
                    "Trying to position a tower ghost while the UI is not currently in a building state.");
            }

            MoveGhost(pointer);

            if (m_CurrentArea != null)
            {
                GateFitStatus fits = m_CurrentArea.Fits(m_GridPosition, m_CurrentGate.controller.dimensions);

                if (fits == GateFitStatus.Fits)
                {
                    // Place the ghost
                    Gate controller = m_CurrentGate.controller;

                    Gate createdTower = Instantiate(controller);
                    createdTower.Initialize(m_CurrentArea, m_GridPosition);

                    CancelGhostPlacement();
                }
            }
        }


        #endregion


        #region Gate Selecting Function
        /// <summary>
        /// Selects a gate beneath the given pointer if there is one
        /// </summary>
        /// <param name="info">
        /// The pointer information concerning the selector of the pointer
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Throws an exception when not in <see cref="State.Normal"/>
        /// </exception>
        public void TrySelectGate(PointerInfo info)
        {
            if (state == State.Paused)
                return;

            if (state != State.Normal)
            {
                throw new InvalidOperationException("Trying to select towers outside of Normal state");
            }
            UIPointer uiPointer = WrapPointer(info);
            RaycastHit output;
            bool hasHit = Physics.Raycast(uiPointer.ray, out output, float.MaxValue, gateSelectionLayer);
            Debug.Log(uiPointer.overUI);
            if (!hasHit || uiPointer.overUI)
            {
                return;
            }
            var controller = output.collider.GetComponent<Gate>();
            if (controller != null)
            {
                SelectGate(controller);
            }
        }

        /// <summary>
        /// Select the current gate and shows the UI
        /// </summary>
        public void SelectGate(Gate gate)
        {
            if (state != State.Normal)
            {
                throw new InvalidOperationException("Trying to select whilst not in a normal state");
            }
            DeselectGate();
            currentSelectedGate = gate;

            if (selectionChanged != null)
            {
                selectionChanged(gate);
            }
        }



        /// <summary>
        /// Deselect the current gate and hides the UI
        /// </summary>
        public void DeselectGate()
        {
            if (state != State.Normal)
            {
                throw new InvalidOperationException("Trying to deselect tower whilst not in Normal state");
            }

            currentSelectedGate = null;

            if (selectionChanged != null)
            {
                selectionChanged(null);
            }
        }



        public void CancelSelectedGate()
        {
            if (state != State.Normal)
            {
                throw new InvalidOperationException("Trying to sell tower whilst not in Normal state");
            }
            if (currentSelectedGate == null)
            {
                throw new InvalidOperationException("Selected Tower is null");
            }

            currentSelectedGate.Remove();
            DeselectGate();
        }


        #endregion



        #region Unity Functions
        /// <summary>
        /// Set initial values, cache attached components
        /// and configure the controls
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            state = State.Normal;
            m_Camera = GetComponent<Camera>();
        }

        /// <summary>
        /// Reset TimeScale if game is paused
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Time.timeScale = 1f;
        }


        #endregion

    }

}
