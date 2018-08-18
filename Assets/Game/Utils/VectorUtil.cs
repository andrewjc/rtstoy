using System.Linq;
using UnityEngine;

namespace Game.Utils
{

    public class VectorUtil
    {
       
        public static Vector3 sitOnTerrain(GameObject gameObject)
        {
            Transform transformVector3 = null;
            if (gameObject == null || gameObject.transform == null) return Vector3.zero;
            if (gameObject.transform != null)
                transformVector3 = gameObject.transform;

            if (transformVector3 != null)
            {

                transformVector3.position = new Vector3(transformVector3.position.x, sitOnTerrain(transformVector3.position).y,
                    transformVector3.position.z);

                return transformVector3.position;
            }
            return gameObject.transform.position;
        }
        
        public static Vector3 sitOnTerrain(Vector3 originalPosition)
        {
            originalPosition.Set(originalPosition.x, Terrain.activeTerrain.SampleHeight(originalPosition),
                    originalPosition.z);
            return originalPosition;
        }

        /*
         * Projects a vector onto a 2d vector. Removing the Y value.
         **/
        public static Vector2 to2D(Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        /*
         * Return the nearest point to on the collider of a given game object to the given vector
         */ 
        public static Vector3 nearestPointOnGameObject(Vector3 vector, GameObject gameObject)
        {
            Collider[] collider = gameObject.GetComponentsInChildren<Collider>();

            var collSorted = collider.Select(e => e.ClosestPointOnBounds(vector)).OrderBy(e => Vector3.Distance(e, vector));

            return collSorted.FirstOrDefault();
        }

        public static Vector3 randomCircle(Vector3 center, float radius)
        {
            var ang = Random.value*360;
            Vector3 pos = new Vector3(center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad), center.y, center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad));
            return pos;
        }

        public static Vector3 gridSnap2D(Vector3 vector, int gridSize)
        {
            return new Vector3(Mathf.Round(vector.x / gridSize) * gridSize, vector.y, Mathf.Round(vector.z / gridSize) * gridSize);
        }


        public static float distanceBetween2D(GameObject gameObject, GameObject gameObject2)
        {
            return Vector2.Distance(to2D(gameObject.transform.position), to2D(gameObject2.transform.position));
        }

        public static GameObject getClosestToGameObject(GameObject gameObject, GameObject[] objectsToSort)
        {
            GameObject spawnPoint = gameObject;
            GameObject closest = null;
            float distance = float.PositiveInfinity;
            foreach (GameObject cluster in objectsToSort)
            {
                Vector3 diff = cluster.transform.position - spawnPoint.transform.position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = cluster;
                    distance = curDistance;
                }
            }
            if (closest == null) return null;

            return closest;
        }

        public static Vector2 lerp2D(Vector3 vector1, Vector3 vector2, float t)
        {
            return Vector2.Lerp(to2D(vector1), to2D(vector2), t);
        }

        public static Vector2 lerp(Vector2 vector1, Vector2 vector2, float t)
        {
            return Vector2.Lerp(vector1, vector2, t);
        }

        public static Vector3 lerp(Vector3 vector1, Vector3 vector2, float t)
        {
            return Vector3.Lerp(vector1, vector2, t);
        }

        public static Bounds getMaxBounds(GameObject g)
        {
            Quaternion currentRotation = g.transform.rotation;
            g.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            Bounds bounds = new Bounds(g.transform.position, Vector3.zero);

            foreach (Renderer renderer in g.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }

            Vector3 localCenter = bounds.center - g.transform.position;
            bounds.center = localCenter;
            Debug.Log("The local bounds of this model is " + bounds);

            g.transform.rotation = currentRotation;
            
            return bounds;
        }

        public static Vector3 findVectorOnCircle(Vector3 initialVector, float surroundingClearing)
        {
            int circleLevel = 1;
            int offsetFromSpawn = 10;
            
            // A Loop (max loop 100)
            for (int i = 100; i > 0; i--)
            {
                // Cast a circle around the starting point

                // B (Loop max 10)
                for (int x = 10; x > 0; x--)
                {
                    // Pick a random point on the circle
                    Vector3 randomCircle = VectorUtil.randomCircle(initialVector, circleLevel);

                    // Make sure that the new point doesn't intersect with a masked layer
                    LayerMask mask = (1 << LayerMask.NameToLayer("NonBuildArea"));
                    // in a function
                    RaycastHit hit;
                    if (Physics.Raycast(randomCircle, -Vector3.up, out hit, Mathf.Infinity, mask))
                    {
                        continue;
                    }

                    // If the point doesn't have any collisions within 10 meters
                    Collider[] colliders = Physics.OverlapSphere(randomCircle, surroundingClearing);
                    if (colliders != null && colliders.Length == 1)
                    {
                        randomCircle = VectorUtil.sitOnTerrain(randomCircle);
                        return randomCircle;
                    }
                }
                circleLevel++;
            }
            return Vector3.zero;
        }
    }

}