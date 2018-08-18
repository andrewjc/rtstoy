using System;
using System.ComponentModel;
using Game.Utils;
using Game.World.Player;
using UnityEngine;

namespace Game.World.Units.Factories
{
    internal class BuildingFactory
    {
        public static GameObject CreateBuilding(PlayerBase playerBase, UnitType type, Vector3 startPosition,
            Quaternion rotation)
        {
            int layerMask = LayerMask.NameToLayer("Buildings");
            var gameObj = CreateBuilding(playerBase, type, startPosition, layerMask, rotation);
            return gameObj;
        }

        public static GameObject CreateBuilding(PlayerBase playerBase, UnitType type, Vector3 startPosition, int layer, Quaternion rotation)
        {
            Debug.Log("CreateBuilding: " + type);
            GameObject gameObject = null;
            switch (type)
            {
                case UnitType.CommandCenter:
                    gameObject = ResourceUtils.CreateFromResource("Prefabs/CommandCenter_Level" + playerBase.getPlayerStageNumber(), startPosition, rotation) as GameObject;
                    gameObject.AddComponent<PlayerUnit>().SetName(GetBuildingName(playerBase, type)).SetUnitType(type).SetPlayerBase(playerBase);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Unhandled building type: " + type);
            }

            gameObject.layer = layer;
            return gameObject;
        }

        private static String GetBuildingName(PlayerBase playerBase, UnitType type)
        {
            String buildingTypeStr = type.ToString() + " " + playerBase.GetUnitCount(type);
            return buildingTypeStr;
        }

        public static GameObject CreateBuilding(PlayerBase playerBase, UnitType type, Vector3 startPosition)
        {
            return CreateBuilding(playerBase, type, startPosition, Quaternion.identity);
        }
    }
}