using Game.Utils;
using Game.World.Objects;
using Game.World.Player;
using System.Linq;
using UnityEngine;

namespace Game.AI.Unit
{
    /*
     * When this task is executed, it causes the worker drone to
     * carry the crystals being carried back to a storage site.
     * 
     * A storage site can be one of the following:
     * The closest crystal storage site (A mining post for example)
     * If no crystal storage site is found, the worker drone will drop
     * the crystals off at the command pod.
     */
    internal class ReturnCrystalToStorageTask : AIUnitTask
    {
        private GameObject storageSite;
        private int amountBeingCarried;

        public new void Start()
        {
            base.Start();
        }
        public new void Awake()
        {
            base.Awake();
        }
        public new void Update()
        {
            if (isCompleted || isPaused || isAborted) return;

            if (storageSite == null)
            {
                // Find a storage site.
                var test = GameObject.FindObjectsOfType<CrystalStorage>();
                var gameObjects = test.Select(e => e.gameObject).ToArray();
                var woodStorageObject = VectorUtil.getClosestToGameObject(gameObject, gameObjects);
                storageSite = woodStorageObject;

                // If none are found, then use the townhall
                if (storageSite == null)
                {
                    storageSite = gameObject.GetComponent<PlayerUnit>().playerBase.GetAllUnitsByType(World.Units.UnitType.CommandCenter)[0] as GameObject;
                }

                gameObject.GetComponent<WorkerDroneAI>().beginNavigateCarryingCrystal(VectorUtil.nearestPointOnGameObject(gameObject.transform.position, storageSite), 0.2f, OnReachedStorageSite);
            }
        }

        private void OnReachedStorageSite(Vector3 storageVector)
        {
            gameObject.GetComponent<WorkerDroneAI>().beginIdle();
            storageSite.GetComponent<CrystalStorage>().Add(amountBeingCarried);
            storageSite = null;
            Complete();
        }

        public AIUnitTask SetStorage(int stoneStorage)
        {
            this.amountBeingCarried = stoneStorage;
            return this;
        }
    }
}