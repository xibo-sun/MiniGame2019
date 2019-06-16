using GateGame.Level;
using System.Collections.Generic;

namespace GateGame.UI.Tutorial
{
    /// <summary>
    /// Help menu tutorial.
    /// </summary>
    public class HowToWinTutorial : Tutorial
    {
        public List<EndPoint> endPoints = new List<EndPoint>();

        public override void CheckIfHappened()
        {
            for (int i = 0; i < endPoints.Count; i++)
            {
                if (endPoints[i].isLightedRightly == false)
                {
                    return;
                }
                TutorialManager.instance.CompletedTutorial();
            }

        }
    }

}
