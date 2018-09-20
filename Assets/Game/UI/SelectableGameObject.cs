using UnityEngine;

namespace Game.UI
{
    public class SelectableGameObject : MonoBehaviour
    {
        public int selectionRadius;
        public SelectionShape selectionShape;
    }

    public enum SelectionShape
    {
        Circle,
        Square
    }
}