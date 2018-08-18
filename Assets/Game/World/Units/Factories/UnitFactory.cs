using System;
using Game.Utils;
using Game.World.Player;
using UnityEngine;

namespace Game.World.Units.Factories
{
    internal class UnitFactory
    {
        public static GameObject CreateUnit(PlayerBase playerBase, UnitType type, Vector3 startPosition, Quaternion rotation)
        {
            switch (type)
            {
                case UnitType.Rover:
                    GameObject unit = ResourceUtils.CreateFromResource("Prefabs/Rover", startPosition, rotation) as GameObject;
                    unit.AddComponent<PlayerUnit>().
                        SetName(GetUnitName(playerBase, type)).
                        SetPlayerBase(playerBase)
                        ;
                    unit.layer = LayerMask.NameToLayer("Units");
                    return unit;
                default:
                    return null;
            }
        }

        private static String GetUnitName(PlayerBase playerBase, UnitType type)
        {
            String buildingTypeStr = type.ToString() + " " + playerBase.GetUnitCount(type);
            return buildingTypeStr;
        }

        public static GameObject CreateUnit(PlayerBase playerBase, UnitType type, Vector3 startPosition)
        {
            return CreateUnit(playerBase, type, startPosition, Quaternion.identity);
        }

        public static bool IsBuilding(UnitType unitType)
        {
            return unitType != UnitType.Rover;
        }

        public static bool IsHuman(UnitType unitType)
        {
            return !IsBuilding(unitType);
        }
    }
}