using System.Collections;
using UnityEngine;
using FlameUtils;

public class MenuManager : MonoBehaviour
{
    // Prefabs and spawn area
    public GameObject[] itemPrefabs;
    public BoxCollider2D spawnArea;
    public float spawnInterval = 5f;

    // Cameras and UI elements
    public Transform mainCameraTransform;
    public Transform themeCameraTransform;
    public GameObject playButton;

    // Singleton instance
    private static MenuManager _instance;

    public static MenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MenuManager>();
                if (_instance == null)
                {
                    GameObject managerObject = new GameObject("MenuManager");
                    _instance = managerObject.AddComponent<MenuManager>();
                }
            }
            return _instance;
        }
    }

    void Start()
    {
        // Set the main camera
        mainCameraTransform = Camera.main.transform;

        // Invoke the SpawnItem method repeatedly with the specified interval
        InvokeRepeating("SpawnItem", 0f, spawnInterval);
    }


    #region Item Spawning

    // Spawn items at regular intervals
    void SpawnItem()
    {
        // Generate a random position within the box collider
        Vector3 randomPosition = GetRandomPositionInSpawnArea();

        // Spawn the item at the random position
        GameObject spawnedItem = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)], randomPosition, Quaternion.identity);
        spawnedItem.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        // Destroy the item after a specified time
        Utilities.DestroyAfterTime(spawnedItem, 15f);
    }

    // Get a random position within the spawn area
    Vector3 GetRandomPositionInSpawnArea()
    {
        Bounds bounds = spawnArea.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(randomX, randomY, -5);
    }

    #endregion
}
