using System.Collections.Generic;
using UnityEngine;

namespace GateGame.UI.Tutorial
{
    /// <summary>
    /// Key tutorial.
    /// </summary>
    public class KeyTutorial : Tutorial
    {

        public List<KeyCode> keys = new List<KeyCode>();

        public override void CheckIfHappened()
        {
            bool isClicked = false;
            for (int i = 0; i < keys.Count; i++)
            {
                if (UnityEngine.Input.GetKeyDown(keys[i]))
                {
                    isClicked = true;
                    break;
                }
            }

            if (isClicked)
            {
                TutorialManager.instance.CompletedTutorial();
                this.gameObject.SetActive(false);
            }

        }

    }
}


