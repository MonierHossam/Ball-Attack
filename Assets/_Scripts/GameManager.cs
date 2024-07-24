using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CollectablesManager))]
public class GameManager : Singleton<GameManager>
{
    [SerializeField] ObjectPoolManager collectablesManager;
    [SerializeField] ObjectPoolManager enemiesManager;

    public PlayerMovement playerMovement;
    LevelManager levelManager;

    [Header("Variables")]
    #region Variables 
    public GameState gameState;

    public LevelSO currentLevel;

    [SerializeField] Vector3 minLevelBoundries;
    [SerializeField] Vector3 maxLevelBoundries;

    [SerializeField] int spawnedCollectables;
    [SerializeField] int totalCollected;
    [SerializeField] float spawnInterval;
    [SerializeField] float enemyInterval;
    [SerializeField] float minHealth = 0.4f;

    private int initialColletablesToSpawn;
    private int maxCollectablesInLevel;

    [SerializeField] int spawnedEnemies;
    private int initialEnemiesToSpawn;
    private int maxEnemiesInLevel;

    private int totalBallsInScene;
    private int totalEnemiesInScene;
    private float sinceLastSpawn;
    private float sinceLastEnemySpawn;
    #endregion

    #region Events
    public Action<float> OnSizeChanged;
    public Action<int> OnBallsCountChanged;
    public Action<GameState> OnGameEnded;
    public Action<int> OnLevelNumberChange;
    public Action<int> OnEnemiesCountChanged;
    #endregion


    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        levelManager = LevelManager.GetInstance();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartGame();


        if (!playerMovement)
        {
            playerMovement = FindAnyObjectByType<PlayerMovement>();
        }

        if (!collectablesManager)
        {
            collectablesManager = GetComponent<CollectablesManager>();
        }

        if (!enemiesManager)
        {
            enemiesManager = GetComponent<EnemiesManager>();
        }
    }

    private void Update()
    {
        if (spawnedCollectables < maxCollectablesInLevel && Time.time - sinceLastSpawn >= spawnInterval)
        {
            SpawnNewItem(SpawnType.Collectable);
        }

        if (spawnedEnemies < maxEnemiesInLevel && Time.time - sinceLastEnemySpawn >= spawnInterval)
        {
            SpawnNewItem(SpawnType.Enemies);
        }
    }

    private void SpawnNewItem(SpawnType spawn)
    {
        float x = UnityEngine.Random.Range(minLevelBoundries.x, maxLevelBoundries.x);
        float z = UnityEngine.Random.Range(minLevelBoundries.z, maxLevelBoundries.z);

        Vector3 pos = new Vector3(x, 0, z);

        if (SpawnType.Collectable == spawn)
        {
            collectablesManager.SpawnFromPool(pos, Quaternion.identity);

            sinceLastSpawn = Time.time;
            spawnedCollectables++;
            totalBallsInScene++;
            OnBallsCountChanged?.Invoke(totalBallsInScene);
        }
        else
        {
            enemiesManager.SpawnFromPool(pos, Quaternion.identity);

            sinceLastEnemySpawn = Time.time;
            spawnedEnemies++;
            totalEnemiesInScene++;
            OnEnemiesCountChanged?.Invoke(totalEnemiesInScene);
        }
    }

    private void SpawnInitialCollectables()
    {
        for (int i = 0; i < initialColletablesToSpawn; ++i)
        {
            float x = UnityEngine.Random.Range(minLevelBoundries.x, maxLevelBoundries.x);
            float z = UnityEngine.Random.Range(minLevelBoundries.z, maxLevelBoundries.z);

            Vector3 pos = new Vector3(x, 0, z);

            collectablesManager.SpawnFromPool(pos, Quaternion.identity);
        }

        spawnedCollectables += initialColletablesToSpawn;
        totalBallsInScene += initialColletablesToSpawn;
        OnBallsCountChanged?.Invoke(totalBallsInScene);
        sinceLastSpawn = Time.time;
    }

    private void SpawnInitialEnemies()
    {
        for (int i = 0; i < initialEnemiesToSpawn; ++i)
        {
            float x = UnityEngine.Random.Range(minLevelBoundries.x, maxLevelBoundries.x);
            float z = UnityEngine.Random.Range(minLevelBoundries.z, maxLevelBoundries.z);

            Vector3 pos = new Vector3(x, 0, z);

            enemiesManager.SpawnFromPool(pos, Quaternion.identity);
        }

        spawnedEnemies += initialEnemiesToSpawn;
        sinceLastEnemySpawn = Time.time;
        totalEnemiesInScene += initialEnemiesToSpawn;
        OnEnemiesCountChanged?.Invoke(totalEnemiesInScene);
    }

    public void ColliededWithCollectable(GameObject other)
    {
        Debug.Log("collieded");

        totalCollected++;
        totalBallsInScene--;
        OnBallsCountChanged?.Invoke(totalBallsInScene);

        if (totalCollected == maxCollectablesInLevel)
        {
            EndGame(GameState.Won);
        }

        collectablesManager.ReturnToPool(other);
    }

    public void ColliededWithEnemy(GameObject other)
    {
        enemiesManager.ReturnToPool(other);
        totalEnemiesInScene--;
        OnEnemiesCountChanged?.Invoke(totalEnemiesInScene);
    }

    public void EndGame(GameState state)
    {
        Debug.Log($"You {state}");

        gameState = state;

        StopMoving();

        OnGameEnded?.Invoke(gameState);
    }

    public void StartGame()
    {
        currentLevel = levelManager.GetCurrentLevel();

        initialColletablesToSpawn = currentLevel.initialColletablesToSpawn;
        initialEnemiesToSpawn = currentLevel.initialEnemiesToSpawn;
        maxCollectablesInLevel = currentLevel.maxCollectablesInLevel;
        maxEnemiesInLevel = currentLevel.maxEnemiesInLevel;

        spawnedCollectables = 0;
        spawnedEnemies = 0;
        totalCollected = 0;
        totalBallsInScene = 0;
        totalEnemiesInScene = 0;
        sinceLastSpawn = 0;
        sinceLastEnemySpawn = 0;

        OnLevelNumberChange?.Invoke(currentLevel.levelNumber);

        collectablesManager.InitializPool(maxCollectablesInLevel);
        SpawnInitialCollectables();

        enemiesManager.InitializPool(maxEnemiesInLevel);
        SpawnInitialEnemies();

        gameState = GameState.Playing;
        AllowMoving();

        OnBallsCountChanged?.Invoke(totalBallsInScene);

        OnEnemiesCountChanged?.Invoke(totalEnemiesInScene);
    }

    public void UpdateSize(float size)
    {
        OnSizeChanged?.Invoke(size);

        if (size < minHealth)
        {
            EndGame(GameState.Lost);
        }
    }

    public void StopMoving()
    {
        playerMovement.UpdateMovebility(false);
    }

    public void AllowMoving()
    {
        playerMovement.UpdateMovebility(true);
    }
}

public enum GameState
{
    None,
    Playing,
    Won,
    Lost
}

