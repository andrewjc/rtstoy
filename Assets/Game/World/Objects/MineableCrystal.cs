using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.World.Objects
{
	public class MineableCrystal : Mineable
	{
        public new MineableResource resourceType
        {
            get { return MineableResource.Crystal; }
        }

        public MiningResult Mine(int quantity)
        {
            lock (this)
            {
                this.remainingQuantity -= quantity;
            }

            if (remainingQuantity == 0)
            {
                isExhausted = true;
                UnityEngine.Object.Destroy(gameObject);
            }

            return new MiningResult() { maxQuantity = this.maxQuantity, remainingQuantity = this.remainingQuantity };
        }
    }
}