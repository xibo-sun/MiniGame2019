using System.Collections.Generic;
using GateGame.UI.HUD;

namespace GateGame.UI.Tutorial
{
    /// <summary>
    /// Button click tutorial.
    /// </summary>
    public class ButtonClickTutorial : Tutorial
    {
        public List<GateSpawnButton> buttons = new List<GateSpawnButton>();

        public override void CheckIfHappened()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].buttonClicked)
                { 
                    TutorialManager.instance.CompletedTutorial();
                    this.gameObject.SetActive(false); 
                }

            }
        }



    }
}


