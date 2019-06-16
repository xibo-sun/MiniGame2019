using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GateGame.Gates.Placement;

namespace GateGame.UI.Tutorial
{
    /// <summary>
    /// Placed tutorial.
    /// </summary>
    public class PlacedTutorial : Tutorial
    {
        public List<SingleGatePlacementArea> placementAreas = new List<SingleGatePlacementArea>();

        public override void CheckIfHappened()
        {
            for (int i = 0; i < placementAreas.Count; i++)
            {
                if (placementAreas[i].IsOccuppied())
                {
                    TutorialManager.instance.CompletedTutorial();
                    this.gameObject.SetActive(false);
                }


            }
        }
    }
}

