using System;
using Game.World;
using Game.World.Player;
using UnityEngine;

namespace Game.AI
{
    public abstract class AIUnitTask : MonoBehaviour
    {
        public bool isAborted;
        public bool isPaused;
        public bool isPausedUntilOtherTask;
        public bool isCompleted;
        public AIUnitTask pauseUntilTaskComplete;
        public bool isExecuting;

        public String GetID()
        {
            return Convert.ToString(GetHashCode());
        }

        public void Start()
        {
            
        }

        public void Awake()
        {
            this.enabled = false;
        }

        public virtual void Abort()
        {
            isAborted = true;
            enabled = false;
        }

        public virtual void Complete()
        {
            isCompleted = true;
            enabled = false;
        }
        
        public virtual void Pause ()
        {
            isPaused = true;
            isExecuting = false;
            enabled = false;
        }

        public virtual void Update()
        {
            if (isExecuting || isCompleted || isPaused || isAborted) return;
        }

        public virtual void PauseUntilTask(AIUnitTask task)
        {
            isPaused = true;
            isExecuting = false;
            enabled = false;
            isPausedUntilOtherTask = true;
            pauseUntilTaskComplete = task;
        }

        public virtual void Resume() {
            isPaused = false;
            enabled = true;
            isExecuting = true;
        }

        public PlayerBase GetPlayerBase()
        {
            return GetPlayerUnit().playerBase;
        }
        public PlayerUnit GetPlayerUnit()
        {
            return gameObject.GetComponent<PlayerUnit>();
        }
    }
}