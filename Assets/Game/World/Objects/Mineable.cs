using UnityEngine;

namespace Game.World.Objects
{
    public enum MineableResource { Crystal }

    public class Mineable : MonoBehaviour
    {
        public float remainingQuantity;

        public int maxQuantity;

        public bool isOccupied;

        public bool isExhausted;

    }
}