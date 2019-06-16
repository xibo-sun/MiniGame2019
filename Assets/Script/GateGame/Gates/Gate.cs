using System;
using Core.Utilities;
using UnityEngine;
using GateGame.UI.HUD;
using GateGame.Gates.Placement;
using GateGame.Laser;
using System.Collections.Generic;

namespace GateGame.Gates
{
    /// <summary>
    /// Common functionality for all types of gates
    /// </summary>
    public class Gate : MonoBehaviour
    {

        public enum GateType
        {
            And,
            Or,
            Not
        }

        /// <summary>
        /// Gets the first level gate ghost prefab
        /// </summary>
        public GatePlacementGhost gateGhostPrefab;

        /// <summary>
        /// The size of the gate's footprint
        /// </summary>
        public IntVector2 dimensions;

        /// <summary>
        /// Gets the grid position for this gate on the <see cref="placementArea"/>
        /// </summary>
        public IntVector2 gridPosition { get; private set; }

        /// <summary>
        /// The placement area we've been built on
        /// </summary>
        public IPlacementArea placementArea { get; private set; }

        /// <summary>
        /// The event that fires off when a player deletes a gate
        /// </summary>
        public Action gateDeleted;

        /// <summary>
        /// The event that fires off when a gate has been destroyed
        /// </summary>
        public Action gateDestroyed;


        /// <summary>
        /// Select the type of Gate
        /// </summary>
        public GateType gateType;
        List<GateData> inputs = new List<GateData>();
        List<LaserMachine> lasers = new List<LaserMachine>();

        LaserMachine laserPoint;


        /// <summary>
        /// Provide the gate with data to initialize with
        /// </summary>
        /// <param name="targetArea">The placement area configuration</param>
        /// <param name="destination">The destination position</param>
        public virtual void Initialize(IPlacementArea targetArea, IntVector2 destination)
        {
            placementArea = targetArea;
            gridPosition = destination;

            if (targetArea != null)
            {
                transform.position = placementArea.GridToWorld(destination, dimensions);
                transform.rotation = placementArea.transform.rotation;
                targetArea.Occupy(destination, dimensions);
            }
        }

        /// <summary>
        /// Removes gate from placement area and destroys it
        /// </summary>
        public void Remove()
        {

            placementArea.Clear(gridPosition, dimensions);
            Destroy(gameObject);
        }


        public void AddInput(GateData inputData, LaserMachine laser)
        {
            if (lasers.Contains(laser))
                return;

            inputs.Add(inputData);
            lasers.Add(laser);
            if ((gateType == GateType.And && inputs.Count == 2) ||
                (gateType == GateType.Or && inputs.Count == 2) ||
                (gateType == GateType.Not && inputs.Count == 1))
            {
                laserPoint.outputType = CalOutputType();

                //laserPoint.UpdateProperties();

                laserPoint.gameObject.SetActive(true);
            }
            else
                laserPoint.gameObject.SetActive(false);

        }

        private void Awake()
        {
            laserPoint = GetComponentInChildren<LaserMachine>();
            laserPoint.gameObject.SetActive(false);
        }

        GateData CalOutputType()
        {
            if (gateType == GateType.And && inputs.Count == 2){
                if (inputs[0] == GateData.HighLevel && inputs[1] == GateData.HighLevel)
                {
                    return GateData.HighLevel;
                }
                else
                    return GateData.LowLevel;

            } else if (gateType == GateType.Or && inputs.Count == 2){
                if (inputs[0] == GateData.LowLevel && inputs[1] == GateData.LowLevel)
                {
                    return GateData.LowLevel;
                }
                else
                    return GateData.HighLevel;
            } else if (gateType == GateType.Not && inputs.Count == 1){
                if (inputs[0] == GateData.LowLevel)
                    return GateData.HighLevel;
                else if (inputs[0] == GateData.HighLevel)
                    return GateData.LowLevel;
            }

            throw new InvalidCastException("Wrong Input datas and Gate Type");
        }




    }


}

