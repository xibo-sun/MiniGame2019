using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Utilities;
using GateGame.Level;
using GateGame.Laser;

namespace GateGame.Game
{
    /// <summary>
    /// Clock manager.
    /// </summary>
    public class ClockManager : Singleton<ClockManager>
    {
        public List<EndPoint> endPoints = new List<EndPoint>();

        public List<LaserMachine> lasers = new List<LaserMachine>();


        private void Start()
        {
            for (int i = 0; i < endPoints.Count; i++)
            {
                Timer.Register(endPoints[i].Clk, endPoints[i].changeColor, isLooped: true);
            }
        }
    }
}



