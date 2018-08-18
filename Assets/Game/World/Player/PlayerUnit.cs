using System;
using System.Collections.Generic;
using Game.AI;
using Game.World.Units;
using Game.World.Units.Factories;
using UnityEngine;

namespace Game.World.Player
{
    public class PlayerUnit : MonoBehaviour
    {
        public LinkedList<AIUnitTask> taskStack;
        public AIUnitTask currentTask;
        public PlayerBase playerBase;
        public UnitType unitType { get; set; }

        public bool IsIdle => taskStack.Count == 0 && currentTask == null;

        public bool IsReassignable { get; set; }
        public String Name { get; set; }

        public bool IsHuman => UnitFactory.IsHuman(unitType);

        public bool IsBuilding => UnitFactory.IsBuilding(unitType);

        public PlayerUnit SetPlayerBase(PlayerBase playerBase)
        {
            this.playerBase = playerBase;
            return this;
        }

        public PlayerUnit SetUnitType(UnitType unitType)
        {
            this.unitType = unitType;
            return this;
        }

        public PlayerUnit SetName(String name)
        {
            this.Name = name;
            return this;
        }
       
        public void Reset()
        {
            taskStack.Clear();
            currentTask = null;
        }

        public void Start()
        {
            int hashCode = this.GetHashCode();
            int instanceId = this.GetInstanceID();
            this.playerBase = (gameObject.GetComponent<PlayerUnit>() as PlayerUnit).playerBase;

            InvokeRepeating("QueueLoop", 1, 1);
        }

        public void Awake()
        {
            taskStack = new LinkedList<AIUnitTask>();
        }

        public void QueueLoop()
        {
            if (taskStack == null) return; //fatal

            bool shouldPickNewItem = false;
            if (currentTask == null)
            {
                shouldPickNewItem = true;
            }
            else
            {
                if (currentTask.isCompleted || currentTask.isAborted)
                {
                    Debug.Log("Removing ai task " + currentTask.ToString());

                    taskStack.Remove(currentTask);

                    DestroyImmediate(currentTask);
                    shouldPickNewItem = true;
                }
                else if (currentTask.isPaused)
                {
                    shouldPickNewItem = true;
                }
            }

            if (shouldPickNewItem)
            {
                if (taskStack != null && taskStack.Count > 0)
                {
                    currentTask = null;

                    LinkedListNode<AIUnitTask> taskPntr = taskStack.First;
                    while (taskPntr != null)
                    {
                        if (taskPntr.Value.isAborted || taskPntr.Value.isCompleted)
                        {
                            // Remove node
                            taskStack.Remove(taskPntr);
                            taskPntr = taskPntr.Next;
                        }
                        else if (taskPntr.Value.isPaused)
                        {
                            if (taskPntr.Value.isPausedUntilOtherTask &&
                                (taskPntr.Value.pauseUntilTaskComplete == null ||
                                 (taskPntr.Value.pauseUntilTaskComplete.isPaused == false ||
                                  taskPntr.Value.pauseUntilTaskComplete.isCompleted == true ||
                                  taskPntr.Value.pauseUntilTaskComplete.isAborted == true)))
                            {
                                // The task on the stack is paused until this task is complete
                                currentTask = taskPntr.Value;
                                break;
                            }
                            else
                            {
                                taskPntr = taskPntr.Next;
                            }
                        }
                        else if (taskPntr.Value.isExecuting)
                        {
                            taskPntr = taskPntr.Next;
                        }
                        else
                        {
                            currentTask = taskPntr.Value;
                            break;
                        }
                    }
                }
                if (currentTask != null)
                {
                    currentTask.Resume();
                }
                else
                {
                    // Ask the base for a task.
                    beginIdle();
                }
            }
        }

        public int GetTaskCount()
        {
            int val = 0;
            if (taskStack == null) val = 0;
            else val = taskStack.Count;
            return val;
        }

        public void ClearTaskList()
        {
            foreach (AIUnitTask task in taskStack)
            {
                task.Abort();
            }
            taskStack.Clear();
        }

        public void AddTask(AIUnitTask task)
        {
            Debug.Log("Adding ai unit task " + task.ToString());
            taskStack.AddFirst(task);
            if (IsReassignable == true && currentTask != null && currentTask.Equals(null)==false)
            {
                // Abort what we are currently doing...
                Debug.Log("Cancelling ai unit's temporary task: " + currentTask.ToString());
                currentTask.Abort();
                taskStack.Remove(currentTask);
            }

            IsReassignable = false;
        }

        public void AddTemporaryTask(AIUnitTask task)
        {
            Debug.Log("Adding ai unit task " + task.ToString());
            taskStack.AddFirst(task);
            IsReassignable = true;
        }
        

        public void beginIdle()
        {
            currentTask = null;
        }

        public bool HasTask(Type type)
        {
            foreach (AIUnitTask task in taskStack)
            {
                if (task.GetType() == type) return true;
            }
            return false;
        }
    }
}