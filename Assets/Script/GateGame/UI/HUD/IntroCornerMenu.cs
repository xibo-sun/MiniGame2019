using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GateGame.UI.HUD
{
    /// <summary>
    ///  Corner Introduction Menu.
    /// </summary>
    public class IntroCornerMenu : MonoBehaviour
    {
        /// <summary>
        /// The UI is opened in first time.
        /// </summary>
        public bool isFirstOpened;

        private void Awake()
        {
            isFirstOpened = false;
        }

        public void setFirstOpened()
        {
            isFirstOpened = true;
        }


    }
}
