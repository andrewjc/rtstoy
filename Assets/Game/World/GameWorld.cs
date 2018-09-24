using System;
using Game.Render;
using Game.Utils;
using Game.World.Objects;
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
            playerBase.SetSpawnPoint(spawnPoint);

        }

        internal Vector3 getSpawnPointPosition()
        {
            return playerBase.GetSpawnPoint().transform.position;      
         }

        internal PlayerBase getPlayerBase()
        {
            return playerBase;
        }

        internal MineableCrystal GetUnoccupiedNearbyCrystal(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}