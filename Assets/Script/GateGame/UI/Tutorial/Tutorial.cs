using UnityEngine;
using System.Collections.Generic;


namespace GateGame.UI.Tutorial
{
    /// <summary>
    /// The base class of Tutorial
    /// </summary>
    public class Tutorial : MonoBehaviour
    {

        public int Order;

        public string Explanation;

        public List<GameObject> gamesObjectsNeedsActive = new List<GameObject>();

        private void Awake()
        {
            // It's unstable, So I commented it, and add the tutorial manually
            //TutorialManager.instance.tutorials.Add(this);
            for (int i = 0; i < gamesObjectsNeedsActive.Count; i++)
            {
                gamesObjectsNeedsActive[i].SetActive(false);
            }
        }

        private void Update()
        {
            if (TutorialManager.instance.tutorialAllComplited == false)
            {
                if (TutorialManager.instance.currentTutorialOrder() == Order)
                {
                    for (int i = 0; i < gamesObjectsNeedsActive.Count; i++)
                    {
                        gamesObjectsNeedsActive[i].SetActive(true);
                    }
                }
            }
        }


        public virtual void CheckIfHappened() { }
    }
}

