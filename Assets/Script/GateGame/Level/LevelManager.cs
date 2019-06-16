using System;
using UnityEngine;
using System.Collections.Generic;
using Core.Utilities;
using GateGame.UI.Tutorial;


namespace GateGame.Level
{
    /// <summary>
    /// The level manager - handles the level states
    /// </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        /// <summary>
        /// The configured level intro. If this is null the LevelManager will fall through to the gameplay state (i.e. SpawningEnemies)
        /// </summary>
        public Collider[] environmentColliders;

        /// <summary>
        /// The home bases that the player must defend
        /// </summary>
        public List<EndPoint> endPoints;

        /// <summary>
        /// Number of end points currently in the level
        /// </summary>
        public int numberOfEndPoints { get; protected set; }



        /// <summary>
        /// The current state of the level
        /// </summary>
        public LevelState levelState { get; protected set; }

        /// <summary>
        /// If the game is over
        /// </summary>
        public bool isGameOver
        {
            get { return (levelState == LevelState.Win) || (levelState == LevelState.Lose); }
        }

        /// <summary>
        /// Fired when all the end points are rightly lighted 
        /// </summary>
        public event Action levelCompleted;

        /// <summary>
        /// Fired when one of end points wrongly lighted
        /// </summary>
        public event Action levelFailed;

        /// <summary>
        /// Fired when the level state is changed - first parameter is the old state, second parameter is the new state
        /// </summary>
        public event Action<LevelState, LevelState> levelStateChanged;

        public void IncreaseSatisfiedEndPoints()
        {
            numberOfEndPoints++;
            Debug.Log("num");
            Debug.Log(numberOfEndPoints);
            if (numberOfEndPoints == endPoints.Count)
            {
                ChangeLevelState(LevelState.Win);
            }
        }

        public void DecreaseSatisfiedEndPoints()
        {
            numberOfEndPoints--;
            if (numberOfEndPoints == endPoints.Count)
            {
                ChangeLevelState(LevelState.Win);
            }
        }

        public void AriseUnSatisfiedEndPoints()
        {
            ChangeLevelState(LevelState.Lose);
        }

        /// <summary>
        /// Changes the state and broadcasts the event
        /// </summary>
        /// <param name="newState">The new state to transitioned to</param>
        protected virtual void ChangeLevelState(LevelState newState)
        {

            // If the state hasn't changed then return
            if (levelState == newState)
            {
                return;
            }

            LevelState oldState = levelState;
            levelState = newState;
            if (levelStateChanged != null)
            {
                levelStateChanged(oldState, newState);
            }

            switch (newState)
            {
                case LevelState.Gaming:
                    break;
                case LevelState.Lose:
                    SafelyCallLevelFailed();
                    break;
                case LevelState.Win:
                    SafelyCallLevelCompleted();
                    break;
            }
        }

        /// <summary>
        /// Calls the <see cref="levelCompleted"/> event
        /// </summary>
        protected virtual void SafelyCallLevelCompleted()
        {

            if (levelCompleted != null)
            {
                levelCompleted();
            }
        }


        /// <summary>
        /// Calls the <see cref="levelFailed"/> event
        /// </summary>
        protected virtual void SafelyCallLevelFailed()
        {
            if (levelFailed != null)
            {
                levelFailed();
            }
        }

        /// <summary>
        /// Fired when Intro is completed or immediately, if no intro is specified
        /// </summary>
        protected virtual void IntroCompleted()
        {
            ChangeLevelState(LevelState.Gaming);
        }


        /// <summary>
        /// Sets the level state to intro and ensures that the number of end points equals 0
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // Does not use the change state function as we don't need to broadcast the event for this default value
            levelState = LevelState.Intro;
            numberOfEndPoints = 0;


            // If there's an intro use it, otherwise fall through to gameplay
            if (TutorialManager.instance != null)
            {
                TutorialManager.instance.introCompleted += IntroCompleted;
            }
            else
            {
                IntroCompleted();
            }

        }

    }
}


