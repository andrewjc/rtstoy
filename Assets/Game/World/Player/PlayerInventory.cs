using System;

namespace Game.World.Player
{
    internal class PlayerInventory
    {
        private int crystalCount;

        internal void AddCrystals(int numCrystals)
        {
            this.crystalCount += numCrystals;
        }

        internal int GetTotalCrystals()
        {
            return this.crystalCount;
        }
    }
}