using GateGame.UI.HUD;

namespace GateGame.UI.Tutorial
{
    /// <summary>
    /// Help menu tutorial.
    /// </summary>
    public class HelpMenuTutorial : Tutorial
    {
        public IntroCornerMenu introCornerMenu;

        public override void CheckIfHappened()
        {
            if (introCornerMenu.isFirstOpened)
            {
                TutorialManager.instance.CompletedTutorial();
            }

        }
    }

}


