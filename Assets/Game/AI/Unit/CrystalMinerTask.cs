using Game.AI;
using Game.AI.Unit;
using Game.Utils;
using Game.World.Objects;
using Game.World.Player;
using UnityEngine;

namespace Game
{
    public class CrystalMinerTask : AIUnitTask
    {
        // The current tree being worked on
        private MineableCrystal selectedCrystal;
        private float mineStartTime = 0.0f;

        private int maxCarryCapacity = 10;
        private int harvestRate = 1;
        private int crystalCarryNum;

        private int tempLastSec = 0;
        private bool isAtCrystal;
        private int resourceCount;
        private int totalResourceMined;
        private bool startedWalking;
        private float MIN_MINE_DISTANCE = 0.2f;

        public new void Start()
        {
            base.Start();
        }
        public new void Awake()
        {
            base.Awake();

            maxCarryCapacity = Mathf.CeilToInt(maxCarryCapacity * (1 + GetPlayerBase().workerDroneCarryMultipler));
            harvestRate = Mathf.CeilToInt(harvestRate * (1 + GetPlayerBase().crystalMiningMultiplier));
        }

        public new void Update()
        {
            if (isCompleted || isPaused || isAborted) return;

            if (GetPlayerBase().GetInventory().GetTotalCrystals() >= resourceCount)
            {
                Complete();
                return;
            }

            if (this.selectedCrystal == null)
            {
                selectedCrystal = GameMain.GetInstance().GetGameWorld().GetUnoccupiedNearbyCrystal(gameObject);
                if (selectedCrystal == null)
                {
                    Abort();
                    return;
                }
            }

            Vector3 dest = VectorUtil.nearestPointOnGameObject(gameObject.transform.position, selectedCrystal.gameObject);
            float distanceToResource = (Vector2.Distance(VectorUtil.to2D(gameObject.transform.position), VectorUtil.to2D(dest)));
            if (distanceToResource > MIN_MINE_DISTANCE)
            {
                gameObject.GetComponent<WorkerDroneAI>().beginMoveTo(VectorUtil.nearestPointOnGameObject(gameObject.transform.position, selectedCrystal.gameObject), OnReachedTree);
            }
        }

        private void OnReachedTree(Vector3 treePosition)
        {
            if (selectedCrystal != null && selectedCrystal.enabled)
            {
                gameObject.GetComponent<WorkerDroneAI>().beginMine(MineableResource.Crystal, selectedCrystal.gameObject.transform.position);
                InvokeRepeating("mineTree", 1, 1);
            }
        }

        
        void mineTree()
        {
            if (gameObject.GetComponent<PlayerUnit>() == null) return;
            {
                if (GetPlayerBase().GetInventory().GetTotalCrystals() >= resourceCount)
                {
                    // Remove this task when the required limit is reached
                    AIUnitTask returnTask = gameObject.AddComponent<ReturnCrystalToStorageTask>().SetStorage(crystalCarryNum);
                    GetPlayerUnit().AddTask(returnTask);
                    this.crystalCarryNum = 0;
                    this.mineStartTime = 0.0f;
                    Complete();
                    return;
                }
                if (selectedCrystal == null)
                {
                    Abort();
                    return;
                }

                MiningResult result = selectedCrystal.Mine(harvestRate);
                this.crystalCarryNum += harvestRate;
                this.totalResourceMined += harvestRate;
                Debug.Log("Mined stone for " + harvestRate + " remaining: " + result.remainingQuantity);
                if (this.crystalCarryNum >= maxCarryCapacity)
                {
                    // When this unit's wood storage capability is exceeded, 
                    // we carry it back to the townhall, or the nearest wood camp storage site.
                    if (selectedCrystal != null)
                    {
                        selectedCrystal.isOccupied = false;
                    }
                    if (selectedCrystal != null)
                    {
                        selectedCrystal.isOccupied = false;
                    }
                    AIUnitTask returnTask = gameObject.AddComponent<ReturnCrystalToStorageTask>().SetStorage(crystalCarryNum);
                    PauseUntilTask(returnTask);
                    GetPlayerUnit().AddTask(returnTask);
                    this.crystalCarryNum = 0;
                    this.mineStartTime = 0.0f;
                    return;
                }
            }

        }

        void StopMining()
        {
            CancelInvoke("mineTree");
            gameObject.GetComponent<WorkerDroneAI>().beginIdle();
            mineStartTime = 0;
            if (selectedCrystal != null)
            {
                selectedCrystal.isOccupied = false;
            }
        }

        public override void Complete()
        {
            base.Complete();
            StopMining();
            selectedCrystal = null;
        }

        public override void Abort()
        {
            base.Abort();
            StopMining();
            selectedCrystal = null;
        }


        public override void Pause()
        {
            base.Pause();
            StopMining();
        }


        public override void PauseUntilTask(AIUnitTask otherTask)
        {
            base.PauseUntilTask(otherTask);
            StopMining();
        }

        public override void Resume()
        {
            if (isPaused)
            {
                if (this.selectedCrystal == null)
                {
                    selectedCrystal = GameMain.GetInstance().GetGameWorld().GetUnoccupiedNearbyCrystal(gameObject);
                    if (selectedCrystal == null)
                    {
                        Abort();
                        return;
                    }
                }

                if (selectedCrystal != null)
                {
                    Vector3 dest = VectorUtil.nearestPointOnGameObject(gameObject.transform.position, selectedCrystal.gameObject);
                    float distanceToResource = (Vector2.Distance(VectorUtil.to2D(gameObject.transform.position), VectorUtil.to2D(dest)));
                    if (distanceToResource > MIN_MINE_DISTANCE)
                    {
                        gameObject.GetComponent<WorkerDroneAI>()
                            .beginMoveTo(
                                VectorUtil.nearestPointOnGameObject(gameObject.transform.position, selectedCrystal.gameObject),  OnReachedTree);
                    }
                    else
                    {
                        gameObject.transform.LookAt(selectedCrystal.transform);
                        mineTree();
                    }
                }

            }

            base.Resume();
        }

        public CrystalMinerTask SetMaxResource(int resourceCount)
        {
            this.resourceCount = resourceCount;
            return this;
        }

        public AIUnitTask SetSelected(MineableCrystal mineable)
        {
            this.selectedCrystal = mineable;
            return this;
        }
    }
}