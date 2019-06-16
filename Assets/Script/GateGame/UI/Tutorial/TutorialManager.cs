using UnityEngine.UI;
using System.Collections.Generic;
using Core.Utilities;
using System;
using UnityEngine;

namespace GateGame.UI.Tutorial
{
    /// <summary>
    /// The Tutorial Manager
    /// </summary>
    public class TutorialManager : Singleton<TutorialManager>
    {
        public List<Tutorial> tutorials = new List<Tutorial>();

        public Text expText;
        Tutorial currentTutorial;

        bool readyToChange;

        public bool tutorialAllComplited;

        /// <summary>
        /// Occurs when intro completed.
        /// </summary>
        public event Action introCompleted;


        /// <summary>
        /// Gets the tutorial by order.
        /// </summary>
        /// <returns>The tutorial by order.</returns>
        /// <param name="order">Order.</param>
        public Tutorial GetTutorialByOrder(int order)
        {
            for (int i = 0; i < tutorials.Count; i++)
            {
                if (tutorials[i].Order == order)
                    return tutorials[i];
            }

            return null;
        }

        /// <summary>
        /// Sets the next tutorial.
        /// </summary>
        /// <param name="currentOrder">Current order.</param>
        public void SetNextTutorial(int currentOrder)
        {
            currentTutorial = GetTutorialByOrder(currentOrder);

            if (!currentTutorial)
            {
                CompletedAllTutorials();
                return;
            }

            expText.text = currentTutorial.Explanation;
        }

        /// <summary>
        /// Completes all tutorials.
        /// </summary>
        public void CompletedAllTutorials()
        {
            expText.text = "你已经完成所有新手教程，继续探索吧!!!";
            tutorialAllComplited = true;
            Invoke("SafelyCallIntroCompleted", 1);
        }

        /// <summary>
        /// Completes the tutorial.
        /// </summary>
        public void CompletedTutorial()
        {
            if (readyToChange == false)
                return;

            SetNextTutorial(currentTutorial.Order + 1);
        }


        private void Start()
        {
            SetNextTutorial(0);
            readyToChange = false;
            tutorialAllComplited = false;
        }


        private void Update()
        {
            if (currentTutorial)
                currentTutorial.CheckIfHappened();
        }

        /// <summary>
        /// Safelies call the intro completed.
        /// </summary>
        protected void SafelyCallIntroCompleted()
        {

            expText.gameObject.SetActive(false);
            if (introCompleted != null)
            {
                introCompleted();
            }
        }


        public int currentTutorialOrder()
        {
            return currentTutorial.Order;
        }


        public void setReadyToChange()
        {
            readyToChange = true;
        }

    }




}


