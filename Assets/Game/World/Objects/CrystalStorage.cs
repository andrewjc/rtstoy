using Game.World.Player;
using UnityEngine;

namespace Game.World.Objects
{
    internal class CrystalStorage : MonoBehaviour
    {
        private int resourceCount;
        private PlayerBase playerBase;

        public void Awake()
        {

        }

        public void Add(int number)
        {
            lock (this)
            {
                this.resourceCount += number;
            }

            if (playerBase == null)
            {
                this.playerBase = gameObject.GetComponent<PlayerUnit>().playerBase;
            }

            playerBase.GetInventory().AddCrystals(number);

        }

        public CrystalStorage SetPlayerBase(PlayerBase playerBase)
        {
            this.playerBase = playerBase;
            return this;
        }
    }
}