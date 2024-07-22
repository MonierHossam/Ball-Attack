using System;
using UnityEngine;

[RequireComponent(typeof(CollectablesManager))]
public class GameManager : Singleton<GameManager>
{
    [SerializeField] CollectablesManager collectablesManager;

    [SerializeField] int initialColletablesToSpawn;
    [SerializeField] int spawnedCollectables;
    [SerializeField] int maxCollectablesInLevel;
    [SerializeField] int totalCollected;

    [SerializeField] float spawnInterval;
    private float sinceLastSpawn;

    private void Start()
    {
        collectablesManager = GetComponent<CollectablesManager>();

        collectablesManager.InitializPool(maxCollectablesInLevel);
        SpawnInitialCollectables();
    }

    private void Update()
    {
        if (spawnedCollectables < maxCollectablesInLevel && Time.time - sinceLastSpawn >= spawnInterval)
        {
            SpawnNewCollectables();
        }
    }

    private void SpawnNewCollectables()
    {
        float x = UnityEngine.Random.Range(-10, 10);
        float z = UnityEngine.Random.Range(-10, 10);

        Vector3 pos = new Vector3(x, 0, z);

        collectablesManager.SpawnFromPool(pos, Quaternion.identity);

        sinceLastSpawn = Time.time;
        spawnedCollectables++;
    }

    private void SpawnInitialCollectables()
    {
        for (int i = 0; i < initialColletablesToSpawn; ++i)
        {
            float x = UnityEngine.Random.Range(-10, 10);
            float z = UnityEngine.Random.Range(-10, 10);

            Vector3 pos = new Vector3(x, 0, z);

            collectablesManager.SpawnFromPool(pos, Quaternion.identity);
        }

        spawnedCollectables += initialColletablesToSpawn;
        sinceLastSpawn = Time.time;
    }

    public void ColliededWithCollectable(GameObject other)
    {
        Debug.Log("collieded");

        totalCollected++;

        if (totalCollected == maxCollectablesInLevel)
        {
            Debug.Log("Won");
        }

        collectablesManager.ReturnToPool(other);

    }

}
