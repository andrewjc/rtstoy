using Assets.Game.World.Objects;
using Game.World.Objects;
using UnityEngine;

namespace Game
{
    public class Miner : MonoBehaviour
    {
        private MineableResource mineableResourceType;

        public Miner()
        {
        }

        public Miner SetMinableType(MineableResource resourceType)
        {
            this.mineableResourceType = resourceType;
            return this;
        }
    }
}