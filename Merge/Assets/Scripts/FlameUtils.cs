using System.Collections.Generic;
using UnityEngine;

namespace FlameUtils
{
    /// <summary>
    /// A utility class providing common functionalities.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Destroys the given GameObject after a specified time.
        /// </summary>
        /// <param name="obj">The GameObject to destroy.</param>
        /// <param name="time">The time delay before destruction.</param>
        public static void DestroyAfterTime(GameObject obj, float time)
        {
            Object.Destroy(obj, time);
        }
    }

    /// <summary>
    /// A simple object pooling system for GameObjects.
    /// </summary>
    public class ObjectPool
    {
        private List<GameObject> pooledObjects = new List<GameObject>();
        private GameObject prefab;

        /// <summary>
        /// Initializes an ObjectPool with the specified prefab and initial count of objects.
        /// </summary>
        /// <param name="prefab">The prefab to be pooled.</param>
        /// <param name="initialCount">The initial number of objects to be preloaded.</param>
        public ObjectPool(GameObject prefab, int initialCount)
        {
            this.prefab = prefab;
            Preload(initialCount);
        }

        /// <summary>
        /// Preloads a specified number of objects into the pool.
        /// </summary>
        /// <param name="count">The number of objects to preload.</param>
        private void Preload(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject spawnedObject = Object.Instantiate(prefab);
                spawnedObject.SetActive(false);
                pooledObjects.Add(spawnedObject);
            }
        }

        /// <summary>
        /// Spawns an object from the pool at the specified location.
        /// </summary>
        /// <param name="spawnLocation">The position where the object will be spawned.</param>
        /// <returns>The spawned GameObject.</returns>
        public GameObject SpawnObject(Vector3 spawnLocation)
        {
            GameObject spawnedObject = pooledObjects.Find(obj => !obj.activeInHierarchy);

            if (spawnedObject == null)
            {
                spawnedObject = Object.Instantiate(prefab);
                pooledObjects.Add(spawnedObject);
            }

            spawnedObject.SetActive(true);
            spawnedObject.transform.position = spawnLocation;

            return spawnedObject;
        }

        /// <summary>
        /// Despawns the specified object by deactivating it.
        /// </summary>
        /// <param name="objToDespawn">The object to despawn.</param>
        public void DespawnObject(GameObject objToDespawn)
        {
            objToDespawn.SetActive(false);
        }
    }
}
