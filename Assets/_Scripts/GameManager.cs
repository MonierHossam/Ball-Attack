using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CollectablesManager))]
public class GameManager : Singleton<GameManager>
{
    [SerializeField] CollectablesManager collectablesManager;
    [SerializeField] EnemiesManager enemiesManager;

    [SerializeField] PlayerMovement playerMovement;
    LevelManager levelManager;

    [Header("Variables")]
    #region Variables 
    public GameState gameState;

    public LevelSO currentLevel;

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
    private float sinceLastSpawn;
    private float sinceLastEnemySpawn;
    #endregion


    #region Events
    public Action<float> OnSizeChanged;
    public Action<int> OnBallsCountChanged;
    public Action<GameState> OnGameEnded;
    public Action<int> OnLevelNumberChange;
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
            SpawnNewCollectable();
        }

        if (spawnedEnemies < maxEnemiesInLevel && Time.time - sinceLastEnemySpawn >= spawnInterval)
        {
            SpawnNewEnemy();
        }
    }

    private void SpawnNewCollectable()
    {
        float x = UnityEngine.Random.Range(-10, 10);
        float z = UnityEngine.Random.Range(-10, 10);

        Vector3 pos = new Vector3(x, 0, z);

        collectablesManager.SpawnFromPool(pos, Quaternion.identity);

        sinceLastSpawn = Time.time;
        spawnedCollectables++;
        totalBallsInScene++;
        OnBallsCountChanged?.Invoke(totalBallsInScene);
    }

    private void SpawnNewEnemy()
    {
        float x = UnityEngine.Random.Range(-10, 10);
        float z = UnityEngine.Random.Range(-10, 10);

        Vector3 pos = new Vector3(x, 0, z);

        enemiesManager.SpawnFromPool(pos, Quaternion.identity);

        sinceLastEnemySpawn = Time.time;
        spawnedEnemies++;
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
        totalBallsInScene += initialColletablesToSpawn;
        OnBallsCountChanged?.Invoke(totalBallsInScene);
        sinceLastSpawn = Time.time;
    }

    private void SpawnInitialEnemies()
    {
        for (int i = 0; i < initialEnemiesToSpawn; ++i)
        {
            float x = UnityEngine.Random.Range(-10, 10);
            float z = UnityEngine.Random.Range(-10, 10);

            Vector3 pos = new Vector3(x, 0, z);

            enemiesManager.SpawnFromPool(pos, Quaternion.identity);
        }

        spawnedEnemies += initialColletablesToSpawn;
        sinceLastEnemySpawn = Time.time;
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
