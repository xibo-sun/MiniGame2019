using UnityEngine;
using System.Collections.Generic;
using GateGame.Gates;
using GateGame.Laser;

namespace GateGame.Level
{
    public class EndPoint : MonoBehaviour
    {
        public GateData EndCondition;
        public Material materialHigh;
        public Material materialLow;
        public float Clk;
        public bool isLightedRightly;


        List<LaserMachine> lasers = new List<LaserMachine>();
        Light lightThis;
        MeshRenderer meshRendererThis;

        private void Awake()
        {
            lightThis = GetComponentInChildren<Light>();
            meshRendererThis = GetComponentInChildren<MeshRenderer>();
            lightThis.gameObject.SetActive(false);
            isLightedRightly = false;
        }


        public void CompareResult(GateData inputData, LaserMachine laser)
        {
            if (lasers.Contains(laser))
                return;

            lasers.Add(laser);
            if (inputData == EndCondition)
            {
                isLightedRightly = true;
                turnOnTheLight();
                LevelManager.instance.IncreaseSatisfiedEndPoints();
            }
            else if (inputData != EndCondition)
            {
                isLightedRightly = false;
                turnOffTheLight();
                LevelManager.instance.AriseUnSatisfiedEndPoints();
            }
        }
        
        private void Update()
        {
            // clear the List
            for (int i = 0; i < lasers.Count; i++)
            {
                if (lasers[i] == null)
                {
                    lasers.Remove(lasers[i]);
                    LevelManager.instance.DecreaseSatisfiedEndPoints();
                }

            }

            if (lasers.Count == 0)
            {
                isLightedRightly = false;
                turnOffTheLight();
            }
        }


        public void turnOnTheLight()
        {
            lightThis.gameObject.SetActive(true);
        }

        public void turnOffTheLight()
        {
            lightThis.gameObject.SetActive(false);
        }

        public void changeColor()
        {
            // change the end condition
            if (EndCondition == GateData.HighLevel) EndCondition = GateData.LowLevel;
            else EndCondition = GateData.HighLevel;

            // change the light color
            if (EndCondition == GateData.HighLevel) lightThis.color = Color.blue;
            else lightThis.color = Color.red;

            // change the meshrender
            if (EndCondition == GateData.HighLevel) meshRendererThis.material = materialLow;
            else meshRendererThis.material = materialHigh;

        }
    }
}

