using GateGame.UI.HUD;

namespace GateGame.UI.Tutorial
{
    /// <summary>
    /// Cancel tutorial.
    /// </summary>
    public class CancelTutorial : Tutorial
    {
        public GateUI cancelGateUI;

        public override void CheckIfHappened()
        {
            if (cancelGateUI.isFirstGateSelected)
            {
                TutorialManager.instance.CompletedTutorial();
            }

        }
    }

}

