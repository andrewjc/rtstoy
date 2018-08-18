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


            Camera.main.transform.position = spawnPoint.transform.position + (Vector3.forward * 5);
            Camera.main.transform.position = VectorUtil.sitOnTerrain(Camera.main.transform.position) + (Vector3.up * 5);
            Camera.main.transform.rotation = spawnPoint.transform.rotation;

            RtsCamera camComponent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RtsCamera>();
            if (camComponent != null)
            {
                camComponent.LookAt = Camera.main.transform.position;
                camComponent.Rotation = 180;
            }
        }
    }
}