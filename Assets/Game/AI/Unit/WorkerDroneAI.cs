using System;
using Game.World.Objects;
using UnityEngine;

namespace Game.AI.Unit
{

    internal class WorkerDroneAI : MonoBehaviour
    {
        private WorkerDroneAIState unitState;

        // The target marker.
        public Vector3 movementTarget;

        // Speed in units per sec.
        public float speed;

        internal void beginNavigateCarryingCrystal(Vector3 vector3, float v, Action<Vector3> onReachedStorageSite)
        {
            throw new NotImplementedException();
        }

        internal void beginIdle()
        {
            unitState = WorkerDroneAIState.Idle;
            speed = 0;
        }

        internal void beginMoveTo(Vector3 moveTo, Action<Vector3> onReachedDestination)
        {
            unitState = WorkerDroneAIState.Moving;
            speed = 3;

            movementTarget = moveTo;
        }

        internal void beginMine(MineableResource crystal, Vector3 position)
        {
            throw new NotImplementedException();
        }

        void Update()
        {
            if (unitState == WorkerDroneAIState.Moving)
            {
                // The step size is equal to speed times frame time.
                float step = speed * Time.deltaTime;

                // Move our position a step closer to the target.
                transform.position = Vector3.MoveTowards(transform.position, movementTarget, step);
                transform.LookAt(movementTarget);
                Utils.VectorUtil.sitOnTerrain(gameObject);
                
            }
        }
    }

    enum WorkerDroneAIState
    {
        Idle,
        Moving
    }
}