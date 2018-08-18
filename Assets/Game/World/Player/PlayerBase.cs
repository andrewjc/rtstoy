using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using Game.World.Units;
using Game.World.Units.Factories;
using UnityEngine;

namespace Game.World.Player
{
     public class PlayerBase : MonoBehaviour
    {
        public GameObject spawnPoint;

        public Dictionary<UnitType, ArrayList> units;

        public void Start()
        {
            playerStage = PlayerStage.Level1;

            // Realign the spawnpoint so it's on the terrain
            spawnPoint.transform.position = VectorUtil.sitOnTerrain(spawnPoint);

            AddBuilding(UnitType.CommandCenter, BuildingFactory.CreateBuilding(this, UnitType.CommandCenter, spawnPoint.transform.position, spawnPoint.transform.rotation));
            
            AddUnit(UnitType.Rover, UnitFactory.CreateUnit(this, UnitType.Rover, VectorUtil.sitOnTerrain(spawnPoint.transform.position + (Vector3.forward * 10) - (Vector3.left * 2))));
            AddUnit(UnitType.Rover, UnitFactory.CreateUnit(this, UnitType.Rover, VectorUtil.sitOnTerrain(spawnPoint.transform.position + (Vector3.forward * 10) + (Vector3.left * 2))));

        }

        // Use this for initialization
        public void Awake()
        {
            this.units = new Dictionary<UnitType, ArrayList>();
        }

        public void AddUnit(UnitType unitType, GameObject unit)
        {
            unit.transform.position = VectorUtil.sitOnTerrain(unit);
            PlayerUnit pu = (unit.GetComponent<PlayerUnit>() as PlayerUnit);
            ArrayList ar = null;
            if (this.units.ContainsKey(unitType) == true)
            {
                ar = (ArrayList)units[unitType];
            }
            else
            {
                ar = new ArrayList();
                units.Add(unitType, ar);
            }

            ar.Add(unit);
        }

        public void AddBuilding(UnitType unitType, GameObject unit)
        {
            unit.transform.position = VectorUtil.sitOnTerrain(unit);
            PlayerUnit pu = (unit.GetComponent<PlayerUnit>() as PlayerUnit);
            ArrayList ar = null;
            if (this.units.ContainsKey(unitType) == true)
            {
                ar = (ArrayList)units[unitType];
            }
            else
            {
                ar = new ArrayList();
                units.Add(unitType, ar);
            }

            ar.Add(unit);

        }

        public ArrayList GetAllUnitsByType(UnitType unitType)
        {
            if (units != null && units.ContainsKey(unitType))
            {
                return units[unitType];
            }
            else return null;
        }

        public int GetUnitCount(UnitType unitType)
        {
            ArrayList units = GetAllUnitsByType(unitType);
            if (units != null)
                return units.Count;

            return 0;
        }
        
        public PlayerStage playerStage;
        public int getPlayerStageNumber()
        {
            switch (playerStage)
            {
                case PlayerStage.Level1:
                    return 1;
                case PlayerStage.Level2:
                    return 2;
                case PlayerStage.Level3:
                    return 3;
            }
            return 0;
        }
    }
}