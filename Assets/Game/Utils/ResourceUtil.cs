using UnityEngine;

namespace Game.Utils
{
    public static class ResourceUtils
    {
        private static readonly int MAX_TREE_RESOURCE_QUANTITY = 100;

        private static readonly int MAX_STONE_RESOURCE_QUANTITY = 200;

        public static GameObject CreateFromResource(string resourceName, Vector3 position, Quaternion rotation)
        {
            Debug.Log("LoadResource: " + resourceName);
            return GameObject.Instantiate(Resources.Load(resourceName), position, rotation) as GameObject;
        }

        internal static GameObject CreateFromResource(string resourceName)
        {
            Debug.Log("LoadResource: " + resourceName);
            return GameObject.Instantiate(Resources.Load(resourceName)) as GameObject;
        }

        public static Material GetMaterial(string materialName)
        {
            Material mat = Resources.Load("Materials/" + materialName) as Material;
            return mat;
        }
    }
}