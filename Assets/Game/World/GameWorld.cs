using System;
using Game.Render;
using Game.Utils;
using Game.World.Player;
using Game.World.Units;
using UnityEngine;

namespace Game.World
{
    public class GameWorld
    {
        private PlayerBase playerBase;

        public void Run()
        {
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");
            
            bool entirelyAutomated = false;
            
            int player = 0;

            playerBase = spawnPoint.AddComponent<PlayerBase>();
            playerBase.spawnPoint = spawnPoint;

        }

        internal Vector3 getSpawnPointPosition()
        {
            return playerBase.spawnPoint.transform.position;      
         }

        internal PlayerBase getPlayerBase()
        {
            return playerBase;
        }
    }
}